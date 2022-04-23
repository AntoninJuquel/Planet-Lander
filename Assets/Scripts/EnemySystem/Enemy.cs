using Entities;
using UnityEngine;

namespace EnemySystem
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] protected float Speed, Range;

        protected Rigidbody2D Rb;
        protected Transform Target;
        protected bool InAttackRange => Vector2.Distance(Target.position, transform.position) <= Range;
        protected Vector2 TargetDirection => (Target.position - transform.position).normalized;

        protected virtual void Awake()
        {
            Rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            Attack();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, Range);
        }

        protected abstract void Attack();
        protected abstract void Move();

        public void SetTarget(Transform target)
        {
            Target = target;
        }
    }
}