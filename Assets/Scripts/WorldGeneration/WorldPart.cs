using System.Collections.Generic;
using GameUtilities;
using UnityEngine;

namespace WorldGeneration
{
    public class WorldPart : MonoBehaviour
    {
        private EdgeCollider2D _col;
        private LineRenderer _lr;

        private void Awake()
        {
            _col = GetComponent<EdgeCollider2D>();
            _lr = GetComponent<LineRenderer>();
        }

        public void Generate(WorldPreset worldPreset)
        {
            Generate(worldPreset, _lr.GetPosition(0).x, _lr.GetPosition(_lr.positionCount - 1).x);
        }

        public void Generate(WorldPreset worldPreset, float startX, float endX)
        {
            var points = new List<Vector2>();
            points.Add(new Vector2(startX, 0));

            for (var x = startX + Random.Range(worldPreset.step.x, worldPreset.step.y); x < endX; x += Random.Range(worldPreset.step.x, worldPreset.step.y))
            {
                var y = Random.Range(worldPreset.height.x, worldPreset.height.y);
                points.Add(new Vector2(x, y));

                foreach (var worldDeformation in worldPreset.worldDeformations)
                {
                    if (Random.value <= worldDeformation.chance)
                        AddDeformation(ref x, y, points, worldDeformation, endX);
                }
            }

            points.Add(new Vector2(endX, 0));

            _lr.positionCount = points.Count;
            _lr.SetPositions(Convert.Vector2ArrayToVector3Array(points.ToArray()));
            _col.points = points.ToArray();
        }

        private void AddDeformation(ref float x, float y, List<Vector2> points, WorldDeformation worldDeformation, float endX)
        {
            var width = Random.Range(worldDeformation.width.x, worldDeformation.width.y);
            var resolution = Random.Range(worldDeformation.resolution.x, worldDeformation.resolution.y);
            var angleStep = width / resolution;
            var iStep = 2f / resolution;
            var altitude = Random.Range(worldDeformation.depth.x, worldDeformation.depth.y);

            for (var i = -1f; i <= 1f; i += iStep, x += angleStep)
            {
                var offset = altitude * (i * i - 1);
                if (x >= endX) return;
                points.Add(new Vector2(x, y - offset));
            }
        }
    }
}