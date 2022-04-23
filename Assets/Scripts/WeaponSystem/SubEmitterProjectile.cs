using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    public class SubEmitterProjectile : MonoBehaviour
    {
        private ParticleSystem _ps;
        private readonly List<ParticleCollisionEvent> _collisionEvents = new List<ParticleCollisionEvent>();

        private void Awake()
        {
            _ps = GetComponent<ParticleSystem>();
        }

        private void OnParticleCollision(GameObject other)
        {
            var events = _ps.GetCollisionEvents(other, _collisionEvents);
            for (var i = 0; i < events; i++)
            {
                Debug.Log("Sub Hit " + other.name);
                //EventHandler.Instance.Raise(new ProjectileHitEvent(other.transform, CurrentWeapon.damage));
            }
        }
    }
}