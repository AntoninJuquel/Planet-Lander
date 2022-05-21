using UnityEngine;
using UnityEngine.Events;

namespace WeaponSystem
{
    public class WeaponBarrel : MonoBehaviour
    {
        [SerializeField] private AmmunitionCore ammunition;
        public UnityEvent<Vector3, Weapon> onProjectileShoot;
        public UnityEvent onAmmunitionHit, onAmmunitionCollision;

        public void Shoot(Weapon weapon)
        {
            var weaponBarrelEmissions = weapon.barrels;

            foreach (var weaponBarrelEmission in weaponBarrelEmissions)
            {
                var minRadius = (1 - weaponBarrelEmission.barrel.thickness) * weaponBarrelEmission.radius;
                var maxRadius = weaponBarrelEmission.radius;

                var shootAngle = Quaternion.Euler(0, 0, weaponBarrelEmission.angle) * transform.right;
                for (var i = 0; i < weaponBarrelEmission.emissionCount; i++)
                {
                    var shootArc = 0f;
                    switch (weaponBarrelEmission.barrel.arcMode)
                    {
                        case ArcMode.Curve:
                            var time = weaponBarrelEmission.barrel.spreadCurve.keys[^1].time;
                            shootArc = (weaponBarrelEmission.barrel.spreadCurve.Evaluate(weapon.time) - .5f) * weaponBarrelEmission.barrel.arc;
                            weapon.time = (weapon.time + time / weaponBarrelEmission.barrel.arc) % time;
                            break;
                        case ArcMode.Random:
                            shootArc = Random.Range(-weaponBarrelEmission.barrel.arc, weaponBarrelEmission.barrel.arc) * .5f;
                            break;
                        case ArcMode.Spread:
                            var div = weaponBarrelEmission.emissionCount % 2 == 0 || weaponBarrelEmission.emissionCount == 1 ? weaponBarrelEmission.emissionCount : weaponBarrelEmission.emissionCount - 1;
                            var step = weaponBarrelEmission.barrel.arc / div;
                            var start = -weaponBarrelEmission.barrel.arc * .5f + (weaponBarrelEmission.emissionCount % 2 == 0 || weaponBarrelEmission.emissionCount == 1 ? step * .5f : 0);
                            shootArc = i * step + start;
                            break;
                    }

                    var direction = Quaternion.Euler(0, 0, shootArc) * shootAngle;
                    var position = transform.position + weaponBarrelEmission.position + direction.normalized * Random.Range(minRadius, maxRadius);

                    var projectileCore = Instantiate(ammunition, position, Quaternion.identity);
                    projectileCore.transform.right = direction;
                    projectileCore.Initialize(weapon.ammunition, weapon.damage, onAmmunitionHit, onAmmunitionCollision);

                    onProjectileShoot?.Invoke(position, weapon);
                }
            }
        }

#if UNITY_EDITOR
        // [Range(0f, 1f)] public float randomArc, randomRadius;
        // public Barrel barrel;
        //
        // private void OnDrawGizmos()
        // {
        //     transform.up = Quaternion.Euler(0, 0, barrel.angle) * Vector3.up;
        //     transform.localPosition = barrel.position;
        //
        //     var start = Quaternion.Euler(0, 0, -barrel.spread * .5f) * transform.up;
        //     var end = Quaternion.Euler(0, 0, barrel.spread * .5f) * transform.up;
        //
        //     var minRadius = (1 - barrel.thickness) * barrel.radius;
        //     var maxRadius = barrel.radius;
        //
        //     var direction = Quaternion.Euler(0, 0, randomArc * barrel.spread) * start;
        //     var position = transform.position + direction.normalized * (minRadius + randomRadius * (maxRadius - minRadius));
        //
        //     UnityEditor.Handles.DrawWireArc(transform.position, transform.forward, start, barrel.spread, minRadius, 2f);
        //     UnityEditor.Handles.DrawWireArc(transform.position, transform.forward, start, barrel.spread, maxRadius, 2f);
        //
        //
        //     Gizmos.color = Color.green;
        //     Gizmos.DrawRay(transform.position, start * barrel.radius);
        //     Gizmos.DrawRay(transform.position, end * barrel.radius);
        //
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawRay(transform.position, direction * barrel.radius);
        //     Gizmos.DrawWireSphere(position, .1f);
        // }
#endif
    }
}