using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(fileName = "New weapon", menuName = "Weapon System/Weapon", order = 0)]
    public class WeaponPreset : ScriptableObject
    {
        [Header("Attack")] public int damage;
        public int burst;
        public int bulletPerBurst;
        public bool automatic;
        public float criticalChance;
        public int criticalDamage;

        public List<FirePoint> firePoints = new List<FirePoint>();
        
        [Header("Speed")] public float fireRate;
        public float burstRate;
        public float reloadTime;
        public float chargeTime;
        public float switchSpeed;

        [Header("Ammo")] public int magazine;
        public int capacity;
        
        public State state;
    }

    public enum State
    {
        ReadyToShoot,
        Shooting,
        Charging,
        WaitingRpm,
        WaitingBtnUp,
        Reloading
    }

    [System.Serializable]
    public struct FirePoint
    {
        public Vector2 position;
        public float angle;
        [Range(0f, 1f)] public float thickness;
        [Range(0f, 360f)] public float spread;
        public float radius;
        public ParticleSystemShapeMultiModeValue arcMode;
        
        [Header("Projectile")] public ProjectilePreset projectile;
        public float projectileSpeed;
        public float projectileRange;
    }
}