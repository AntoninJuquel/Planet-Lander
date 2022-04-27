using System;
using System.Collections;
using UnityEngine;

namespace EnemySystem
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] protected float speed, range;
        protected Rigidbody2D Rb;
        protected Transform Target;
        protected Coroutine StateRoutine;
        private State _sate;

        protected State State
        {
            get => _sate;
            set
            {
                if (StateRoutine != null) StopCoroutine(StateRoutine);
                HandleState(_sate, value);
                _sate = value;
            }
        }

        protected bool InAttackRange => Vector2.Distance(Target.position, transform.position) <= range;
        protected Vector2 TargetDirection => (Target.position - transform.position).normalized;

        protected virtual void Awake()
        {
            Rb = GetComponent<Rigidbody2D>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, range);
        }

        public void SetTarget(Transform target)
        {
            Target = target;
            State = State.Chasing;
        }

        protected void LookTarget() => transform.up = TargetDirection;

        protected virtual void HandleState(State oldState, State newState)
        {
            switch (newState)
            {
                case State.Default:
                    break;
                case State.Chasing:
                    StateRoutine = StartCoroutine(ChaseRoutine());
                    break;
                case State.Attacking:
                    StateRoutine = StartCoroutine(AttackRoutine());
                    break;
                default:
                    return;
            }
        }

        protected virtual IEnumerator ChaseRoutine()
        {
            var fixedUpdate = new WaitForFixedUpdate();

            while (Target)
            {
                transform.up = TargetDirection;
                Rb.velocity = TargetDirection * speed;

                if (InAttackRange)
                {
                    State = State.Attacking;
                }

                yield return fixedUpdate;
            }
        }

        protected abstract IEnumerator AttackRoutine();
    }

    public enum State
    {
        Default,
        Chasing,
        Attacking
    }
}