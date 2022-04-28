using System.Collections;
using UnityEngine;
using WeaponSystem;

namespace EnemySystem.Behaviours
{
    public class BoomerEnemy : Enemy
    {
        private Camera _mainCamera;
        private Weapon _weapon;
        [SerializeField] private Vector3[] _viewportPositions = {new Vector3(.1f, .9f, 10), new Vector3(.9f, .9f, 10)};
        private int _index;

        protected override void Awake()
        {
            base.Awake();
            _mainCamera = Camera.main;
            _weapon = GetComponentInChildren<Weapon>();
        }

        protected override IEnumerator ChaseRoutine()
        {
            var fixedUpdate = new WaitForFixedUpdate();
            var position = _mainCamera.ViewportToWorldPoint(_viewportPositions[_index]);
            var direction = (position - transform.position).normalized;
            var distance = Vector2.Distance(transform.position, position);
            var time = distance / speed;
            transform.up = direction;

            while (time > 0)
            {
                Rb.velocity = direction * speed;
                time -= Time.fixedDeltaTime;
                yield return fixedUpdate;
            }

            _index = (_index + 1) % _viewportPositions.Length;
            State = State.Attacking;
        }

        protected override IEnumerator AttackRoutine()
        {
            Rb.velocity = Vector2.zero;
            transform.up = TargetDirection;
            yield return new WaitForSeconds(.5f);
            _weapon.Shoot();
            yield return new WaitForSeconds(.5f);

            State = State.Chasing;
        }
    }
}