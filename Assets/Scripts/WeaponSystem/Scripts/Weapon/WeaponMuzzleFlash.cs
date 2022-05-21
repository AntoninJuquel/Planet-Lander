using UnityEngine;

namespace WeaponSystem
{
    [RequireComponent(typeof(WeaponBarrel))]
    public class WeaponMuzzleFlash : MonoBehaviour
    {
        [SerializeField] private new ParticleSystem particleSystem;
        private WeaponBarrel _weaponBarrel;

        private void Awake()
        {
            _weaponBarrel = GetComponent<WeaponBarrel>();
        }

        private void OnEnable()
        {
            _weaponBarrel.onProjectileShoot.AddListener(Flash);
        }

        private void OnDisable()
        {
            _weaponBarrel.onProjectileShoot.RemoveListener(Flash);
        }

        public void Flash(Vector3 position, Weapon weapon)
        {
            var muzzleFlash = weapon.muzzleFlash;

            var main = particleSystem.main;
            main.startLifetime = muzzleFlash.lifetime;
            main.startColor = muzzleFlash.color;
            main.startSpeed = muzzleFlash.speed;
            main.startSize = muzzleFlash.size;

            var shape = particleSystem.shape;
            shape.arc = muzzleFlash.arc;
            shape.radius = muzzleFlash.radius;
            shape.radiusThickness = muzzleFlash.radiusThickness;
            shape.arcMode = muzzleFlash.arcMode;

            var textureSheetAnimation = particleSystem.textureSheetAnimation;
            textureSheetAnimation.SetSprite(0, muzzleFlash.sprite);


            particleSystem.transform.position = position;
            particleSystem.Emit((int) Random.Range(muzzleFlash.count.constantMin, muzzleFlash.count.constantMax));
        }
    }
}