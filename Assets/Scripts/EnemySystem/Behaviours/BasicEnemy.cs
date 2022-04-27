using System;
using System.Collections;
using UnityEngine;
using WeaponSystem;

namespace EnemySystem.Behaviours
{
    public class BasicEnemy : Enemy
    {
        private DistanceJoint2D _distance;
        private Weapon _weapon;

        protected override void Awake()
        {
            base.Awake();
            _distance = GetComponent<DistanceJoint2D>();
            _weapon = GetComponentInChildren<Weapon>();

            _distance.distance = range - .1f;
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
            while (Target && InAttackRange)
            {
                _distance.connectedAnchor = Target.position;
                LookTarget();
                Rb.velocity = transform.right * speed;
                _weapon.Shoot();

                yield return fixedUpdate;
            }

            State = State.Chasing;
        }
    }
}