using UnityEngine;

namespace EnemySystem.Behaviours
{
    public class TacticalEnemy : Enemy
    {
        private DistanceJoint2D _distance;

        protected override void Awake()
        {
            base.Awake();
            _distance = GetComponent<DistanceJoint2D>();
            _distance.distance = Range - .1f;
        }

        protected override void Attack()
        {
        }

        protected override void Move()
        {
            transform.up = TargetDirection;
            if (InAttackRange)
            {
                _distance.enabled = true;
                _distance.connectedAnchor = Target.position;
                Rb.velocity = transform.right * Speed;
            }
            else
            {
                _distance.enabled = false;
                Rb.velocity = transform.up * Speed;
            }
        }
    }
}