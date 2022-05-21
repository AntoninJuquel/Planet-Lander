using ReferenceSharing;
using UnityEngine;

namespace WorldGeneration
{
    public class Background : MonoBehaviour
    {
        [SerializeField] private Reference<int> levelRef;
        [SerializeField] private World[] worldPresets;
        [SerializeField] private float scrollSpeed;
        [SerializeField] private Material material;
        private Vector3 _offset;
        private World CurrentWorld => worldPresets[levelRef.Value % worldPresets.Length];

        private void Start()
        {
            StopBackground();
        }

        private void Update()
        {
            var offset = _offset + transform.position * scrollSpeed;

            foreach (var noiseSettings in CurrentWorld.noiseSettings)
            {
                material.SetVector($"_{noiseSettings.name}Offset", offset);
            }
        }

        public void StartBackground()
        {
            _offset = new Vector3(Random.Range(-999f, 999f), Random.Range(-999f, 999f));

            foreach (var noiseSettings in CurrentWorld.noiseSettings)
            {
                material.SetColor($"_{noiseSettings.name}Color", CurrentWorld.backgroundColor);
                material.SetFloat($"_{noiseSettings.name}Strength", noiseSettings.strength);
                material.SetFloat($"_{noiseSettings.name}Roughness", noiseSettings.roughness);
            }
        }

        public void StopBackground()
        {
            foreach (var noiseSettings in CurrentWorld.noiseSettings)
            {
                material.SetColor($"_{noiseSettings.name}Color", Color.black);
            }
        }
    }
}