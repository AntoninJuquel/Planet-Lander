using Entities;
using Managers.Event;
using UnityEngine;
using UnityEngine.InputSystem;
using WeaponSystem;

namespace PlayerSystem
{
    public class Player : MonoBehaviour
    {
        private Gamepad _input;
        private PlayerSpawnEvent _playerSpawnEvent;
        private Weapon _weapon;
        private Spaceship _spaceship;
        
        private void Awake()
        {
            _input = Gamepad.current;
            _playerSpawnEvent = new PlayerSpawnEvent(transform);
            _weapon = GetComponentInChildren<Weapon>();
            _spaceship = GetComponent<Spaceship>();
        }

        private void Start()
        {
            EventHandler.Instance.Raise(_playerSpawnEvent);
        }

        private void Update()
        {
            _spaceship.AddForce(_input.rightTrigger.ReadValue());
            _spaceship.AddTorque(_input.leftStick.ReadValue().x);

            if (_input.buttonSouth.isPressed)
            {
                _weapon.Shoot();
            }
        }
    }
}