using UnityEngine;

namespace WeaponSystem
{
    public class AmmunitionDisplacement : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Ammunition _ammunition;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(Ammunition ammunition)
        {
            _rb.drag = ammunition.drag;
            _rb.sharedMaterial = ammunition.physicsMaterial;
            _rb.gravityScale = ammunition.gravity;

            _rb.velocity = transform.right * ammunition.speed;

            _ammunition = ammunition;
        }

        private void Update()
        {
            transform.right = _rb.velocity.normalized;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            _rb.velocity *= (1 - _ammunition.dampen);
        }
    }
}