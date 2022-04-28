using System.Collections;
using Entities;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace EnemySystem.Behaviours
{
    public class KamikazeEnemy : Enemy
    {
        private Health _health;

        protected override void Awake()
        {
            base.Awake();
            _health = GetComponent<Health>();
        }

        protected override IEnumerator AttackRoutine()
        {
            var fixedUpdate = new WaitForFixedUpdate();
            while (Target && InAttackRange)
            {
                transform.up = TargetDirection;
                Rb.velocity = transform.up * (speed * 4);
                yield return fixedUpdate;
            }

            State = State.Chasing;
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            _health.Die(false);
        }
    }
}