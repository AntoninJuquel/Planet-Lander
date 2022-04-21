using System.Collections;
using System.Collections.Generic;
using Managers.Event;
using UnityEngine;

namespace WeaponSystem
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private List<WeaponPreset> weapons;
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

        // private void Update()
        // {
        //     if (target)
        //     {
        //         var particleCount = _ps.particleCount;
        //         var particles = new ParticleSystem.Particle[particleCount];
        //         _ps.GetParticles(particles);
        //         for (var i = 0; i < particles.Length; i++)
        //         {
        //             particles[i].velocity += (target.position - particles[i].position).normalized * 10;
        //         }
        //
        //         _ps.SetParticles(particles, particleCount);
        //     }
        // }

        private void OnParticleCollision(GameObject other)
        {
            var events = _ps.GetCollisionEvents(other, _collisionEvents);
            for (var i = 0; i < events; i++)
            {
                Debug.Log("Hit " + other.name);
                EventHandler.Instance.Raise(new ProjectileHitEvent(other.transform, CurrentWeapon.damage));
            }
        }

        private IEnumerator ShootRoutine()
        {
            CurrentWeapon.state = State.Charging;

            yield return new WaitForSeconds(CurrentWeapon.chargeTime);

            CurrentWeapon.state = State.Shooting;

            foreach (var firePoint in CurrentWeapon.firePoints)
            {
                AdjustParticles(firePoint);
                _ps.Emit(CurrentWeapon.bulletPerBurst);
            }

            for (var i = 1; i < CurrentWeapon.burst && CurrentWeapon.magazine > 0; i++)
            {
                yield return new WaitForSeconds(60 / CurrentWeapon.burstRate);
                foreach (var firePoint in CurrentWeapon.firePoints)
                {
                    AdjustParticles(firePoint);
                    _ps.Emit(CurrentWeapon.bulletPerBurst);
                }
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
            var shape = _ps.shape;
            shape.arc = (1 - CurrentWeapon.accuracy) * 360f;
            shape.position = firePoint.position;
            shape.rotation = Vector3.forward * (90f + (firePoint.angle - shape.arc * .5f));
            shape.radiusThickness = .1f;

            var main = _ps.main;
            main.startSpeedMultiplier = CurrentWeapon.projectileSpeed;
            main.startLifetimeMultiplier = CurrentWeapon.projectileRange / CurrentWeapon.projectileSpeed;
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