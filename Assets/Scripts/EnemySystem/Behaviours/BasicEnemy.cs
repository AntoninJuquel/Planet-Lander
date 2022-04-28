using System;
using System.Collections;
using UnityEngine;
using WeaponSystem;

namespace EnemySystem.Behaviours
{
    public class BasicEnemy : Enemy
    {
        [SerializeField] private float attackTime;
        private DistanceJoint2D _distance;
        private Weapon _weapon;
        private float _mult = 1f;

        protected override void Awake()
        {
            base.Awake();
            _distance = GetComponent<DistanceJoint2D>();
            _weapon = GetComponentInChildren<Weapon>();

            _distance.distance = range - 1f;
        }

        protected override IEnumerator ChaseRoutine()
        {
            _distance.enabled = false;
            return base.ChaseRoutine();
        }

        protected override IEnumerator AttackRoutine()
        {
            _distance.enabled = true;
            var fixedUpdate = new WaitForFixedUpdate();
            var timer = Time.time;
            while (Target && InAttackRange)
            {
                _distance.connectedAnchor = Target.position;
                LookTarget();
                Rb.velocity = transform.right * _mult * speed;

                if (Time.time - timer >= attackTime)
                {
                    _weapon.Shoot();
                    timer = Time.time;
                }

                yield return fixedUpdate;
            }

            State = State.Chasing;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            _mult *= -1f;
        }
    }
}