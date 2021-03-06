using MessagingSystem;
using ReferenceSharing;
using UnityEngine;

namespace PlayerSystem
{
    public class Spaceship : MonoBehaviour
    {
        [SerializeField] private float forceSpeed, torqueSpeed, crashSpeed, maxSpeed, burnRate;
        [SerializeField] private Reference<float> fuelRef, fuelBurntRef, speedRef, altitudeRef, forceInputRef, torqueInputRef, maxFuelRef;
        [SerializeField] private Reference<bool> landed, hasFuel;
        private Rigidbody2D _rb;
        private SpaceshipLandedEvent _spaceshipLandedEvent;
        private SpaceshipTookOffEvent _spaceshipTookOffEvent;
        private ParticleSystem _ps;

        private void Awake()
        {
            landed.Value = false;
            _ps = GetComponentInChildren<ParticleSystem>();
            _rb = GetComponent<Rigidbody2D>();
            _spaceshipLandedEvent = new SpaceshipLandedEvent(transform);
            _spaceshipTookOffEvent = new SpaceshipTookOffEvent(transform);
        }

        private void FixedUpdate()
        {
            if (hasFuel.Value)
            {
                _rb.AddForce(transform.up * forceSpeed * forceInputRef);
                if (forceInputRef != 0)
                {
                    fuelRef.Value -= burnRate * Time.deltaTime;
                    fuelBurntRef.Value += burnRate * Time.deltaTime;
                    _ps.Emit(1);
                    _rb.AddForce(Vector2.up * Mathf.Abs(Physics2D.gravity.y), ForceMode2D.Force);
                    if (_rb.velocity.magnitude > maxSpeed)
                    {
                        _rb.velocity = _rb.velocity.normalized * maxSpeed;
                    }
                }

                _rb.angularVelocity = -torqueInputRef * torqueSpeed * (forceInputRef != 0 ? .5f : 1);
            }

            hasFuel.Value = fuelRef.Value > 0;

            altitudeRef.Value = Mathf.Ceil(transform.position.y * 100);
            speedRef.Value = Mathf.Ceil(_rb.velocity.magnitude * 100);

            _rb.drag = forceInputRef;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            var hitVelocity = col.relativeVelocity;
            if (hitVelocity.magnitude >= crashSpeed)
            {
                EventManager.Instance.Raise(new SpaceshipCrashedEvent(transform, col.relativeVelocity.magnitude));
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            landed.Value = false;
            EventManager.Instance.Raise(_spaceshipTookOffEvent);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (landed.Value || _rb.velocity != Vector2.zero) return;
            landed.Value = true;
            EventManager.Instance.Raise(_spaceshipLandedEvent);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Fuel")) return;
            fuelRef.Value = maxFuelRef.Value;
        }
    }
}