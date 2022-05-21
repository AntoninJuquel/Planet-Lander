using ReferenceSharing;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceshipSystem
{
    public class SpaceshipMovements : MonoBehaviour
    {
        private Spaceship Data => SpaceshipManager.Data;
        [SerializeField] private InputAction horizontal, vertical;
        [SerializeField] private Reference<float> horizontalInputRef, verticalInputRef, fuelRef, altitudeRef, speedRef;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            horizontal.Enable();
            vertical.Enable();
        }

        private void OnDisable()
        {
            horizontal.Disable();
            vertical.Disable();
        }

        private void Update()
        {
            horizontalInputRef.Value = horizontal.ReadValue<float>();
            verticalInputRef.Value = vertical.ReadValue<float>();
            speedRef.Value = _rb.velocity.magnitude * 100f;
            altitudeRef.Value = transform.position.y * 100f;
        }

        private void FixedUpdate()
        {
            Move();
            Rotate();
            CounterGravity();
        }

        private void Move()
        {
            if (fuelRef.Value > 0)
            {
                _rb.AddForce(transform.up * (Data.force * verticalInputRef.Value));
                _rb.drag = verticalInputRef.Value;
            }
        }

        private void Rotate()
        {
            if (fuelRef.Value > 0)
                _rb.angularVelocity = -horizontalInputRef.Value * Data.torqueSpeed * (verticalInputRef.Value != 0 ? .5f : 1);
        }

        private void CounterGravity()
        {
            if (verticalInputRef != 0 && fuelRef.Value > 0)
                _rb.AddForce(Vector2.up * Mathf.Abs(Physics2D.gravity.y), ForceMode2D.Force);
        }
    }
}