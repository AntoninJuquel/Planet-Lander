using UnityEngine;

namespace WorldGeneration
{
    [CreateAssetMenu(fileName = "New world preset", menuName = "World", order = 0)]
    public class WorldPreset : ScriptableObject
    {
        public float gravity;
        public Vector2 step, height;
        public WorldDeformation[] worldDeformations;
    }

    [System.Serializable]
    public struct WorldDeformation
    {
        public float chance;
        public Vector2 width, depth, resolution;
    }
}