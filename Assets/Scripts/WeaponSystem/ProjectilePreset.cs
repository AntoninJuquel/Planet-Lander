using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(fileName = "New projectile", menuName = "Weapon System/Projectile", order = 0)]
    public class ProjectilePreset : ScriptableObject
    {
        public float size;
        public Sprite sprite;
        public Color color;
        public float gravity;
        public Gradient colorOverLifetime;
        public SubEmitter[] subEmitters = { };
    }

    [System.Serializable]
    public struct SubEmitter
    {
        public bool collision;
        public float rateOverTime, rateOverDistance, burstCount;

        public float speed;
        public float size;
        public Sprite sprite;
        public Color color;
        public float gravity;
        public Gradient colorOverLifetime;

        public ParticleSystemSubEmitterProperties subEmitterProperties;
        public ParticleSystemSubEmitterType subEmitterType;
    }
}