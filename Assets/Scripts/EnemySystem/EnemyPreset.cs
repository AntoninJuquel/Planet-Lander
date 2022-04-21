using UnityEngine;
using WeaponSystem;

namespace EnemySystem
{
    [CreateAssetMenu(fileName = "New enemy preset", menuName = "Enemy", order = 0)]
    public class EnemyPreset : ScriptableObject
    {
        public WeaponPreset weapon;
        public Sprite sprite;
        public Color color;
        public float speed;
        public int health;
    }
}