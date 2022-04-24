using System.Collections;
using MessagingSystem;
using ReferenceSharing;
using UnityEngine;

namespace PlayerSystem
{
    public class Spaceship : MonoBehaviour
    {
        [SerializeField] private float forceSpeed, torqueSpeed, crashSpeed, maxSpeed, burnRate;
        [SerializeField] private Reference<float> fuelRef, fuelBurntRef, maxFuelRef, speedRef, altitudeRef;
        private float _forceInput, _torqueInput;
        private bool _landed;
        private Rigidbody2D _rb;
        private SpaceshipLandedEvent _spaceshipLandedEvent;
        private SpaceshipTookOffEvent _spaceshipTookOffEvent;
        private ParticleSystem _ps;

        private void Awake()
        {
            _ps = GetComponentInChildren<ParticleSystem>();
            _rb = GetComponent<Rigidbody2D>();
            _spaceshipLandedEvent = new SpaceshipLandedEvent(transform);
            _spaceshipTookOffEvent = new SpaceshipTookOffEvent(transform);

            fuelRef.Value = maxFuelRef;
        }

        private void FixedUpdate()
        {
            if (fuelRef > 0)
            {
                _rb.AddForce(transform.up * forceSpeed * _forceInput);
                if (_forceInput != 0)
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

                _rb.angularVelocity = -_torqueInput * torqueSpeed * (_forceInput != 0 ? .5f : 1);
            }

            altitudeRef.Value = Mathf.Ceil(transform.position.y * 100);
            speedRef.Value = Mathf.Ceil(_rb.velocity.magnitude * 100);

            _rb.drag = _forceInput;
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
            _landed = false;
            EventManager.Instance.Raise(_spaceshipTookOffEvent);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!_landed && _rb.velocity == Vector2.zero)
            {
                _landed = true;
                EventManager.Instance.Raise(_spaceshipLandedEvent);
            }
        }

        public void AddForce(float input)
        {
            _forceInput = input;
        }

        public void AddTorque(float input)
        {
            _torqueInput = input;
        }
    }
}