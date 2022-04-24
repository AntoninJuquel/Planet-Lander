using System.Collections.Generic;
using MessagingSystem;
using ReferenceSharing;
using UnityEngine;

namespace WorldGeneration
{
    public class World : MonoBehaviour
    {
        [SerializeField] private Reference<int> levelRef;
        [SerializeField] private WorldPart worldPartPrefab;
        [SerializeField] private WorldPreset[] _worldPresets;
        private Dictionary<Vector2, WorldPart> _worldParts = new Dictionary<Vector2, WorldPart>();
        private int PresetIndex => levelRef.Value % _worldPresets.Length;

        private void OnEnable()
        {
            EventManager.Instance.AddListener<StartGameEvent>(StartGameHandler);
            EventManager.Instance.AddListener<NewChunkEvent>(NewChunkHandler);
            EventManager.Instance.AddListener<EnableChunkEvent>(EnableChunkHandler);
            EventManager.Instance.AddListener<DisableChunkEvent>(DisableChunkHandler);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<StartGameEvent>(StartGameHandler);
            EventManager.Instance.RemoveListener<NewChunkEvent>(NewChunkHandler);
            EventManager.Instance.RemoveListener<EnableChunkEvent>(EnableChunkHandler);
            EventManager.Instance.RemoveListener<DisableChunkEvent>(DisableChunkHandler);
        }

        private void NewChunkHandler(NewChunkEvent e)
        {
            if (e.Position.y != 0) return;
            var start = e.Position.x - e.Size * .5f;
            var end = e.Position.x + e.Size * .5f;
            GenerateWorldPart(start, end, e.Position.x);
        }

        private void EnableChunkHandler(EnableChunkEvent e)
        {
            if (e.Position.y != 0) return;
            _worldParts[e.Position].gameObject.SetActive(true);
        }

        private void DisableChunkHandler(DisableChunkEvent e)
        {
            if (e.Position.y != 0) return;
            _worldParts[e.Position].gameObject.SetActive(false);
        }

        private void StartGameHandler(StartGameEvent e)
        {
            foreach (var kvp in _worldParts)
            {
                kvp.Value.Generate(_worldPresets[PresetIndex]);
            }

            Generate(PresetIndex);
        }

        private void Generate(int index)
        {
            Physics2D.gravity = Vector2.up * _worldPresets[index].gravity;
        }

        private void GenerateWorldPart(float startX, float endX, float centerX)
        {
            Vector2 position = Vector3.right * centerX;
            var worldPart = Instantiate(worldPartPrefab, Vector3.zero, Quaternion.identity, transform);
            worldPart.Generate(_worldPresets[PresetIndex], startX, endX);
            _worldParts.Add(position, worldPart);
        }
    }
}