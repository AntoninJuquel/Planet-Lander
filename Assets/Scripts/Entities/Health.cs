using MessagingSystem;
using ReferenceSharing;
using UnityEngine;

namespace Entities
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private Reference<int> maxHealth, health;
        private EntityKilledEvent _entityKilledEvent;

        private void OnEnable()
        {
            EventManager.Instance.AddListener<ProjectileHitEvent>(ProjectileHitHandler);
            EventManager.Instance.AddListener<SpaceshipCrashedEvent>(SpaceshipCrashedHandler);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<ProjectileHitEvent>(ProjectileHitHandler);
            EventManager.Instance.RemoveListener<SpaceshipCrashedEvent>(SpaceshipCrashedHandler);
        }

        private void Awake()
        {
            _entityKilledEvent = new EntityKilledEvent(transform);
            health.Value = maxHealth.Value;
        }

        private void Die()
        {
            EventManager.Instance.Raise(_entityKilledEvent);
        }

        private void TakeDamage(int amount)
        {
            health.Value -= amount;
            EventManager.Instance.Raise(new EntityDamagedEvent(transform, health.Value, maxHealth.Value));
            if (health.Value <= 0) Die();
        }

        private void ProjectileHitHandler(ProjectileHitEvent e)
        {
            if (e.Hit != transform) return;
            TakeDamage(e.Damage);
        }

        private void SpaceshipCrashedHandler(SpaceshipCrashedEvent e)
        {
            if (e.Transform != transform) return;
            TakeDamage(Mathf.CeilToInt(e.HitForce));
        }
    }
}