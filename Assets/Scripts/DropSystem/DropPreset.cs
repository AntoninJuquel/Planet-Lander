using UnityEngine;

namespace DropSystem
{
    [CreateAssetMenu(fileName = "New drop preset", menuName = "Drop", order = 0)]
    public class DropPreset : ScriptableObject
    {
        [Range(0f, 1f)] public float chance;
        public Color color;
        public Sprite sprite;
        public string tag;
    }
}