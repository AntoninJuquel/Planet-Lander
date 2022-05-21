using System;
using ReferenceSharing;
using UnityEngine;
using WeaponSystem;

namespace HealthSystem
{
    public abstract class Health : MonoBehaviour, ITakeHit
    {
        [SerializeField] protected Reference<int> maxHealth, health;
        [SerializeField] private bool canHeal;
        public event EventHandler OnDie, OnTakeHit;

        protected virtual void Die()
        {
            OnDie?.Invoke(this, null);
        }

        public virtual void Hit(int amount)
        {
            health.Value -= amount;
            OnTakeHit?.Invoke(this, null);

            if (health.Value <= 0) Die();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!canHeal) return;

            if (col.CompareTag("Health"))
            {
                health.Value = maxHealth.Value;
                Destroy(col.gameObject);
            }
        }
    }
}