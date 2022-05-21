using System;
using ReferenceSharing;
using UnityEngine;

namespace SpaceshipSystem
{
    public class SpaceshipCollision : MonoBehaviour
    {
        public event EventHandler OnCrash;
        private Spaceship Data => SpaceshipManager.Data;
        [SerializeField] private Reference<bool> landedRef;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (!col.gameObject.CompareTag("World")) return;

            var angle = Vector2.Angle(transform.up, col.contacts[0].normal);
            var collisionSpeed = col.relativeVelocity.magnitude;

            if (angle < Data.maxLandAngle && collisionSpeed < Data.maxLandVelocity)
            {
                Land(col);
            }
            else
            {
                OnCrash?.Invoke(this, null);
                Debug.Log(angle);
                Debug.Log(collisionSpeed);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            landedRef.Value = false;
        }

        private void Land(Collision2D col)
        {
            transform.up = col.contacts[0].normal;
            _rb.velocity = Vector2.zero;
            landedRef.Value = true;
        }
    }
}