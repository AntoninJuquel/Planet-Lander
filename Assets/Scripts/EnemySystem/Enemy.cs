using System.Collections;
using UnityEngine;
using WeaponSystem;

namespace EnemySystem
{
    public class Enemy : MonoBehaviour
    {
        private SpriteRenderer _sr;
        private Rigidbody2D _rb;
        private Transform _target, _transform;
        private Weapon _weapon;
        private Coroutine _followTargetRoutine;
        private float _speed;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _weapon = GetComponentInChildren<Weapon>();
            _sr = GetComponent<SpriteRenderer>();
            _transform = transform;
        }

        private IEnumerator FollowTarget()
        {
            while (_target)
            {
                var dir = (_target.position - _transform.position).normalized;
                _transform.up = dir;
                _rb.velocity = _transform.up * _speed;
                _weapon.Shoot();
                yield return null;
            }
        }

        public void Setup(EnemyPreset enemyPreset, Transform target)
        {
            _speed = enemyPreset.speed;
            _weapon.AddWeapon(enemyPreset.weapon);
            _sr.color = enemyPreset.color;
            _sr.sprite = enemyPreset.sprite;
            SetTarget(target);
        }

        public void SetTarget(Transform target)
        {
            _target = target;
            if (_followTargetRoutine != null) StopCoroutine(_followTargetRoutine);
            _followTargetRoutine = StartCoroutine(FollowTarget());
        }
    }
}