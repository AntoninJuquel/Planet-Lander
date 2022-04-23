using Managers.Event;
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
            EventHandler.Instance.AddListener<ProjectileHitEvent>(ProjectileHitHandler);
            EventHandler.Instance.AddListener<SpaceshipCrashedEvent>(SpaceshipCrashedHandler);
        }

        private void OnDisable()
        {
            EventHandler.Instance.RemoveListener<ProjectileHitEvent>(ProjectileHitHandler);
            EventHandler.Instance.RemoveListener<SpaceshipCrashedEvent>(SpaceshipCrashedHandler);
        }

        private void Awake()
        {
            _entityKilledEvent = new EntityKilledEvent(transform);
            health.Value = maxHealth;
        }

        private void Die()
        {
            EventHandler.Instance.Raise(_entityKilledEvent);
        }

        private void TakeDamage(int amount)
        {
            health.Value -= amount;
            EventHandler.Instance.Raise(new EntityDamagedEvent(transform, health.Value, maxHealth));
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