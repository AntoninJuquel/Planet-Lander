using Managers.Event;
using UnityEngine;

namespace Entities
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        private int _health;
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
            Setup(maxHealth);
        }

        private void Die()
        {
            EventHandler.Instance.Raise(_entityKilledEvent);
        }

        private void TakeDamage(int amount)
        {
            _health -= amount;
            EventHandler.Instance.Raise(new EntityDamagedEvent(transform, _health, maxHealth));
            if (_health <= 0) Die();
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

        public void Setup(int health)
        {
            maxHealth = health;
            _health = maxHealth;
        }
    }
}