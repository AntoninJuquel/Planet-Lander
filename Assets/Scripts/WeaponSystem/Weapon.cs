using System.Collections;
using System.Collections.Generic;
using MessagingSystem;
using ReferenceSharing;
using UnityEngine;

namespace WeaponSystem
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private List<WeaponPreset> weapons;
        [SerializeField] private ParticleSystem subEmitter;
        [SerializeField] private Reference<int> shots, hits;
        [SerializeField] private Reference<float> accuracy;
        private ParticleSystem _ps;
        private int _index;
        private bool _switching;
        private WeaponPreset CurrentWeapon => weapons[_index];
        private readonly List<ParticleCollisionEvent> _collisionEvents = new List<ParticleCollisionEvent>();

        private void Awake()
        {
            _ps = GetComponent<ParticleSystem>();
            for (var i = 0; i < weapons.Count; i++)
            {
                weapons[i] = Instantiate(weapons[i]);
            }
        }

        private void OnParticleCollision(GameObject other)
        {
            var events = _ps.GetCollisionEvents(other, _collisionEvents);
            for (var i = 0; i < events; i++)
            {
                if (other.CompareTag("Enemy"))
                    hits.Value++;
                Debug.Log("Hit " + other.name);
                EventManager.Instance.Raise(new ProjectileHitEvent(other.transform, CurrentWeapon.damage));
            }

            accuracy.Value = (float) hits.Value / (float) shots.Value * 100f;
        }

        private void Fire()
        {
            EventManager.Instance.Raise(new WeaponShotEvent());

            foreach (var firePoint in CurrentWeapon.firePoints)
            {
                AdjustParticles(firePoint);
                _ps.Emit(CurrentWeapon.bulletPerBurst);
            }

            CurrentWeapon.magazine--;
            shots.Value++;
            accuracy.Value = (float) hits.Value / (float) shots.Value * 100f;
        }

        private IEnumerator ShootRoutine()
        {
            CurrentWeapon.state = State.Charging;

            yield return new WaitForSeconds(CurrentWeapon.chargeTime);

            CurrentWeapon.state = State.Shooting;

            for (var i = 0; i < CurrentWeapon.burst && CurrentWeapon.magazine > 0; i++)
            {
                Fire();
                yield return new WaitForSeconds(60 / CurrentWeapon.burstRate);
            }

            if (CurrentWeapon.magazine <= 0)
                yield return ReloadRoutine();
            else
            {
                CurrentWeapon.state = State.WaitingRpm;
                yield return new WaitForSeconds(60 / CurrentWeapon.fireRate);

                // if (!CurrentWeapon.automatic && Input.GetMouseButton(0))
                // {
                //     CurrentWeapon.state = State.WaitingBtnUp;
                //     yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
                // }

                CurrentWeapon.state = State.ReadyToShoot;
            }
        }

        private IEnumerator ReloadRoutine()
        {
            CurrentWeapon.state = State.Reloading;
            yield return new WaitForSeconds(CurrentWeapon.reloadTime);
            CurrentWeapon.magazine = CurrentWeapon.capacity;
            CurrentWeapon.state = State.ReadyToShoot;
        }

        private IEnumerator SwitchRoutine(int delta)
        {
            var oldIndex = _index;
            _index = _index + delta < 0 ? weapons.Count - 1 : (_index + delta) % weapons.Count;
            _switching = true;
            yield return new WaitForSeconds(weapons[oldIndex].switchSpeed + CurrentWeapon.switchSpeed);
            switch (CurrentWeapon.state)
            {
                case State.ReadyToShoot:
                case State.Charging:
                case State.Shooting:
                    break;
                case State.WaitingRpm:
                    yield return new WaitForSeconds(60 / CurrentWeapon.fireRate);
                    break;
                case State.WaitingBtnUp:
                    if (Input.GetMouseButton(0))
                        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
                    break;
                case State.Reloading:
                    yield return ReloadRoutine();
                    break;
            }

            _switching = false;
            CurrentWeapon.state = State.ReadyToShoot;
        }

        private void AdjustParticles(FirePoint firePoint)
        {
            var main = _ps.main;
            main.startLifetimeMultiplier = CurrentWeapon.projectileRange / CurrentWeapon.projectileSpeed;
            main.startSpeedMultiplier = CurrentWeapon.projectileSpeed;
            main.startSizeMultiplier = CurrentWeapon.projectile.size;
            main.startColor = CurrentWeapon.projectile.color;
            main.gravityModifierMultiplier = CurrentWeapon.projectile.gravity;

            var shape = _ps.shape;
            shape.arc = (1 - CurrentWeapon.accuracy) * 360f;
            shape.position = firePoint.position;
            shape.rotation = Vector3.forward * (90f + (firePoint.angle - shape.arc * .5f));
            shape.radiusThickness = .1f;

            var textureSheet = _ps.textureSheetAnimation;
            textureSheet.SetSprite(0, CurrentWeapon.projectile.sprite);

            var colorOverLifetime = _ps.colorOverLifetime;
            if (CurrentWeapon.projectile.colorOverLifetime.alphaKeys.Length > 0 || CurrentWeapon.projectile.colorOverLifetime.colorKeys.Length > 0)
            {
                colorOverLifetime.enabled = true;
                colorOverLifetime.color = CurrentWeapon.projectile.colorOverLifetime;
            }
            else
            {
                colorOverLifetime.enabled = false;
            }

            var subEmitters = _ps.subEmitters;
            if (CurrentWeapon.projectile.subEmitters.Length > 0)
            {
                subEmitters.enabled = true;
                for (var i = 0; i < CurrentWeapon.projectile.subEmitters.Length; i++)
                {
                    var subEmitterStruct = CurrentWeapon.projectile.subEmitters[i];
                    ParticleSystem subPs;
                    if (subEmitters.subEmittersCount > i)
                        subPs = subEmitters.GetSubEmitterSystem(i);
                    else
                    {
                        subEmitters.AddSubEmitter(subEmitter, subEmitterStruct.subEmitterType, subEmitterStruct.subEmitterProperties, 1);
                        subPs = subEmitters.GetSubEmitterSystem(i);
                    }

                    var subMain = subPs.main;
                    //subMain.startLifetimeMultiplier = CurrentWeapon.projectileRange / CurrentWeapon.projectileSpeed;
                    subMain.startSpeedMultiplier = subEmitterStruct.speed;
                    subMain.startSizeMultiplier = subEmitterStruct.size;
                    subMain.startColor = subEmitterStruct.color;
                    subMain.gravityModifierMultiplier = subEmitterStruct.gravity;

                    var subTextureSheet = subPs.textureSheetAnimation;
                    subTextureSheet.SetSprite(0, subEmitterStruct.sprite);

                    var subEmission = subPs.emission;
                    subEmission.rateOverDistanceMultiplier = subEmitterStruct.rateOverDistance;
                    subEmission.rateOverTimeMultiplier = subEmitterStruct.rateOverTime;
                    subEmission.SetBurst(0, new ParticleSystem.Burst(0, subEmitterStruct.burstCount));

                    var subColorOverLifetime = subPs.colorOverLifetime;
                    if (subEmitterStruct.colorOverLifetime.alphaKeys.Length > 0 || subEmitterStruct.colorOverLifetime.colorKeys.Length > 0)
                    {
                        subColorOverLifetime.enabled = true;
                        subColorOverLifetime.color = subEmitterStruct.colorOverLifetime;
                    }
                    else
                    {
                        subColorOverLifetime.enabled = false;
                    }
                }
            }
            else
            {
                subEmitters.enabled = true;
            }
        }

        public void Shoot()
        {
            if (weapons.Count == 0 || _switching || CurrentWeapon.state != State.ReadyToShoot) return;
            if (CurrentWeapon.magazine <= 0)
            {
                StartCoroutine(ReloadRoutine());
                return;
            }

            StartCoroutine(ShootRoutine());
        }

        public void Reload()
        {
            if (CurrentWeapon.magazine >= CurrentWeapon.capacity || CurrentWeapon.state == State.Reloading || _switching) return;
            StartCoroutine(ReloadRoutine());
        }

        public void SwitchWeapon(int delta)
        {
            StopAllCoroutines();
            StartCoroutine(SwitchRoutine(delta));
        }

        public void AddWeapon(WeaponPreset weaponPreset)
        {
            if (weaponPreset == null) return;
            weapons.Add(Instantiate(weaponPreset));
            SwitchWeapon(1);
        }

        private void OnDrawGizmos()
        {
            if (weapons.Count == 0) return;
            _ps = GetComponent<ParticleSystem>();
            foreach (var firePoint in CurrentWeapon.firePoints)
            {
                var shape = _ps.shape;
                shape.arc = (1 - CurrentWeapon.accuracy) * 360f;
                shape.position = firePoint.position;
                shape.rotation = Vector3.forward * (90f + (firePoint.angle - shape.arc * .5f));
            }
        }
    }
}