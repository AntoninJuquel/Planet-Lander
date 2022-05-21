using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(menuName = "WeaponSystem/Muzzle Flash")]
    public class MuzzleFlash : ScriptableObject
    {
        public ParticleSystem.MinMaxCurve count;
        public ParticleSystem.MinMaxCurve size;
        public ParticleSystem.MinMaxCurve lifetime;
        public ParticleSystem.MinMaxCurve speed;
        public float radius;
        [Range(0, 1)] public float radiusThickness;
        [Range(0, 360)] public float arc;
        public ParticleSystemShapeMultiModeValue arcMode;
        public Sprite sprite;
        public ParticleSystem.MinMaxGradient  color;
    }
}