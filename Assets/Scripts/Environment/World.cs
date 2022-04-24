using System.Collections.Generic;
using System.Linq;
using Managers.Event;
using ReferenceSharing;
using UnityEngine;

namespace Environment
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
            EventHandler.Instance.AddListener<StartGameEvent>(StartGameHandler);
            EventHandler.Instance.AddListener<NewChunkEvent>(NewChunkHandler);
            EventHandler.Instance.AddListener<EnableChunkEvent>(EnableChunkHandler);
            EventHandler.Instance.AddListener<DisableChunkEvent>(DisableChunkHandler);
        }

        private void OnDisable()
        {
            EventHandler.Instance.RemoveListener<StartGameEvent>(StartGameHandler);
            EventHandler.Instance.RemoveListener<NewChunkEvent>(NewChunkHandler);
            EventHandler.Instance.RemoveListener<EnableChunkEvent>(EnableChunkHandler);
            EventHandler.Instance.RemoveListener<DisableChunkEvent>(DisableChunkHandler);
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