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
        public List<FirePoint> firePoints = new List<FirePoint>();
        public bool automatic;
        public float criticalChance;
        public int criticalDamage;

        [Header("Speed")] public float fireRate;
        public float burstRate;
        public float reloadTime;
        public float chargeTime;
        public float switchSpeed;

        [Header("Ammo")] public int magazine;
        public int capacity;

        [Header("Projectile")] public ProjectilePreset projectile;
        public float projectileSpeed;
        public float projectileRange;
        public float accuracy;

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
    }
}