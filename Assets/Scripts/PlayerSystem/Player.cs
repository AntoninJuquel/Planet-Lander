using MessagingSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using WeaponSystem;

namespace PlayerSystem
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Weapon mainWeapon, counterMeasures;
        private Gamepad _input;
        private PlayerSpawnEvent _playerSpawnEvent;
        private Spaceship _spaceship;

        private void Awake()
        {
            _input = Gamepad.current;
            _playerSpawnEvent = new PlayerSpawnEvent(transform);
            mainWeapon = GetComponentInChildren<Weapon>();
            _spaceship = GetComponent<Spaceship>();
        }

        private void Start()
        {
            EventManager.Instance.Raise(_playerSpawnEvent);
        }

        private void Update()
        {
            _spaceship.AddForce(_input.rightTrigger.ReadValue());
            _spaceship.AddTorque(_input.leftStick.ReadValue().x);

            if (_input.buttonSouth.isPressed)
            {
                mainWeapon.Shoot();
            }

            if (_input.buttonEast.isPressed)
            {
                counterMeasures.Shoot();
            }
        }
    }
}