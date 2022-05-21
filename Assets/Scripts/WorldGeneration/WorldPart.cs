using System.Collections.Generic;
using UnityEngine;

namespace WorldGeneration
{
    public static class Convert
    {
        public static Vector3[] Vector2ArrayToVector3Array(this Vector2[] vector2) => System.Array.ConvertAll(vector2, v2 => new Vector3(v2.x, v2.y));
        public static Vector2[] Vector3ArrayToVector2Array(this Vector3[] vector3) => System.Array.ConvertAll(vector3, v3 => new Vector2(v3.x, v3.y));
    }

    [RequireComponent(typeof(EdgeCollider2D))]
    [RequireComponent(typeof(LineRenderer))]
    public class WorldPart : MonoBehaviour
    {
        private EdgeCollider2D _col;
        private LineRenderer _lr;

        private void Awake()
        {
            _col = GetComponent<EdgeCollider2D>();
            _lr = GetComponent<LineRenderer>();
        }

        public void Generate(World world, float startX, float endX)
        {
            var points = new List<Vector2>();
            points.Add(new Vector2(startX, 0));

            for (var x = startX + Random.Range(world.step.x, world.step.y); x < endX; x += Random.Range(world.step.x, world.step.y))
            {
                var y = Random.Range(world.height.x, world.height.y);
                points.Add(new Vector2(x, y));

                foreach (var worldDeformation in world.worldDeformations)
                {
                    if (Random.value <= worldDeformation.chance)
                        AddDeformation(ref x, y, points, worldDeformation, endX);
                }
            }

            points.Add(new Vector2(endX, 0));

            _lr.positionCount = points.Count;
            _lr.SetPositions(points.ToArray().Vector2ArrayToVector3Array());
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