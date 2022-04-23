using System.Collections;
using Managers.Event;
using UnityEngine;

namespace PlayerSystem
{
    public class Spaceship : MonoBehaviour
    {
        [SerializeField] private float forceSpeed, torqueSpeed, crashSpeed, maxSpeed, maxFuel, burnRate;
        private float _forceInput, _torqueInput, _fuel;
        private bool _landed;
        private Rigidbody2D _rb;
        private WaitForFixedUpdate _fixedUpdate;
        private SpaceshipLandedEvent _spaceshipLandedEvent;
        private SpaceshipTookOffEvent _spaceshipTookOffEvent;
        private ParticleSystem _ps;

        private void Awake()
        {
            _ps = GetComponentInChildren<ParticleSystem>();
            _rb = GetComponent<Rigidbody2D>();
            _fixedUpdate = new WaitForFixedUpdate();
            _spaceshipLandedEvent = new SpaceshipLandedEvent(transform);
            _spaceshipTookOffEvent = new SpaceshipTookOffEvent(transform);

            _fuel = maxFuel;
        }

        private IEnumerator Start()
        {
            while (gameObject.activeSelf)
            {
                if (_fuel > 0)
                {
                    _rb.AddForce(transform.up * forceSpeed * _forceInput);
                    if (_forceInput != 0)
                    {
                        _fuel -= burnRate * Time.deltaTime;
                        _ps.Emit(1);
                        _rb.AddForce(Vector2.up * Mathf.Abs(Physics2D.gravity.y), ForceMode2D.Force);
                        if (_rb.velocity.magnitude > maxSpeed)
                        {
                            _rb.velocity = _rb.velocity.normalized * maxSpeed;
                        }
                    }

                    _rb.angularVelocity = -_torqueInput * torqueSpeed * (_forceInput != 0 ? .5f : 1);
                }

                var altitude = Mathf.Ceil(transform.position.y * 100);
                var speed = Mathf.Ceil(_rb.velocity.magnitude * 100);

                EventHandler.Instance.Raise(new SpaceshipMetricsEvent(transform, altitude, speed, _fuel, maxFuel));
                _rb.drag = _forceInput;
                yield return _fixedUpdate;
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            var hitVelocity = col.relativeVelocity;
            if (hitVelocity.magnitude >= crashSpeed)
            {
                EventHandler.Instance.Raise(new SpaceshipCrashedEvent(transform, col.relativeVelocity.magnitude));
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            _landed = false;
            EventHandler.Instance.Raise(_spaceshipTookOffEvent);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!_landed && _rb.velocity == Vector2.zero)
            {
                _landed = true;
                EventHandler.Instance.Raise(_spaceshipLandedEvent);
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