using UnityEngine;

namespace WorldGeneration
{
    [CreateAssetMenu(fileName = "New world preset", menuName = "World", order = 0)]
    public class World : ScriptableObject
    {
        public float gravity;
        public Vector2 step, height;
        public WorldDeformation[] worldDeformations;

        public NoiseSettings[] noiseSettings;
        public Color backgroundColor;
    }

    [System.Serializable]
    public struct WorldDeformation
    {
        public float chance;
        public Vector2 width, depth, resolution;
    }

    [System.Serializable]
    public struct NoiseSettings
    {
        public string name;
        public float strength;
        public float roughness;
    }
}