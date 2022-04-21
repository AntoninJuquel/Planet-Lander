using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(fileName = "New projectile", menuName = "Weapon System/Projectile", order = 0)]
    public class ProjectilePreset : ScriptableObject
    {
        [Header("Attack")] public int damage;
        public float criticalChance;
        public int criticalDamage;

        [Header("Travel")] public float speed;
        public float speedMult = 1;
        public float range;

        public ProjectilePreset ApplyWeaponModifiers(WeaponPreset weapon)
        {
            var instance = Instantiate(this);
            
            instance.damage += weapon.damage;
            instance.criticalChance += weapon.criticalChance;
            instance.criticalDamage += weapon.criticalDamage;
            instance.range += weapon.projectileRange;
            instance.speed = (speed + weapon.projectileSpeed) * speedMult;

            return instance;
        }
    }
}