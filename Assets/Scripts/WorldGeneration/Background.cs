using MessagingSystem;
using ReferenceSharing;
using UnityEngine;

namespace WorldGeneration
{
    public class Background : MonoBehaviour
    {
        [SerializeField] private Reference<int> levelRef;
        [SerializeField] private WorldPreset[] worldPresets;
        [SerializeField] private float scrollSpeed;
        private Material _material;
        private Vector3 _offset;
        private WorldPreset CurrentWorldPreset => worldPresets[levelRef.Value % worldPresets.Length];

        private void OnEnable()
        {
            EventManager.Instance.AddListener<MainMenuEvent>(MainMenuHandler);
            EventManager.Instance.AddListener<StartGameEvent>(StartGameHandler);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<MainMenuEvent>(MainMenuHandler);
            EventManager.Instance.RemoveListener<StartGameEvent>(StartGameHandler);
        }

        private void Awake()
        {
            _material = GetComponent<SpriteRenderer>().material;
        }

        private void Update()
        {
            var offset = _offset + transform.position * scrollSpeed;

            foreach (var noiseSettings in CurrentWorldPreset.noiseSettings)
            {
                _material.SetVector($"_{noiseSettings.name}Offset", offset);
            }
        }

        private void MainMenuHandler(MainMenuEvent e)
        {
            foreach (var noiseSettings in CurrentWorldPreset.noiseSettings)
            {
                _material.SetColor($"_{noiseSettings.name}Color", Color.black);
            }
        }

        private void StartGameHandler(StartGameEvent e)
        {
            _offset = new Vector3(Random.Range(-999f, 999f), Random.Range(-999f, 999f));

            foreach (var noiseSettings in CurrentWorldPreset.noiseSettings)
            {
                _material.SetColor($"_{noiseSettings.name}Color", CurrentWorldPreset.backgroundColor);
                _material.SetFloat($"_{noiseSettings.name}Strength", noiseSettings.strength);
                _material.SetFloat($"_{noiseSettings.name}Roughness", noiseSettings.roughness);
            }
        }
    }
}