using System;
using System.Collections.Generic;
using ChunkSystem;
using ReferenceSharing;
using UnityEngine;

namespace WorldGeneration
{
    public class WorldManager : MonoBehaviour, IHandleChunk
    {
        public event EventHandler OnChunkStart;
        [SerializeField] private Reference<int> levelRef;
        [SerializeField] private WorldPart worldPartPrefab;
        [SerializeField] private World[] worldPresets;
        private Dictionary<Vector2, WorldPart> _worldParts = new();
        private int PresetIndex => levelRef.Value % worldPresets.Length;

        public void ChunkCreatedHandler(Bounds e)
        {
            if (e.center.y != 0) return;
            var start = e.center.x - e.extents.x;
            var end = e.center.x + e.extents.x;
            GenerateWorldPart(start, end, e.center.x);
        }

        public void ChunkEnabledHandler(Bounds e)
        {
            if (e.center.y != 0) return;
            _worldParts[e.center].gameObject.SetActive(true);
        }

        public void ChunkDisabledHandler(Bounds e)
        {
            if (e.center.y != 0) return;
            _worldParts[e.center].gameObject.SetActive(false);
        }

        private void GenerateWorldPart(float startX, float endX, float centerX)
        {
            Vector2 position = Vector3.right * centerX;
            var worldPart = Instantiate(worldPartPrefab, Vector3.zero, Quaternion.identity, transform);
            worldPart.Generate(worldPresets[PresetIndex], startX, endX);
            _worldParts.Add(position, worldPart);
        }

        public void StartWorldManager()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            _worldParts = new Dictionary<Vector2, WorldPart>();

            OnChunkStart?.Invoke(this, null);
            Physics2D.gravity = Vector2.up * worldPresets[PresetIndex].gravity;
        }
    }
}