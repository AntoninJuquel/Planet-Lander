using System.Collections;
using UnityEngine;
using WeaponSystem;

namespace EnemySystem.Behaviours
{
    public class TacticalEnemy : Enemy
    {
        private Weapon _weapon;

        protected override void Awake()
        {
            base.Awake();
            _weapon = GetComponentInChildren<Weapon>();
        }

        protected override IEnumerator AttackRoutine()
        {
            var fixedUpdate = new WaitForFixedUpdate();
            while (Target && InAttackRange)
            {
                var position = Target.position + (Vector3) Random.insideUnitCircle * range;
                var time = Vector2.Distance(position, transform.position) / speed;
                transform.up = (position - transform.position).normalized;

                while (time > 0 && InAttackRange)
                {
                    Rb.velocity = (position - transform.position).normalized * speed;
                    time -= Time.fixedDeltaTime;
                    yield return fixedUpdate;
                }

                time = 2f;
                while (time > 0 && InAttackRange)
                {
                    Rb.velocity = Vector2.zero;
                    LookTarget();
                    _weapon.Shoot();
                    time -= Time.fixedDeltaTime;
                    yield return fixedUpdate;
                }
            }

            State = State.Chasing;
        }
    }
}