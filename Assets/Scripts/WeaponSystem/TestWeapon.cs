using UnityEngine;
using UnityEngine.InputSystem;

namespace WeaponSystem
{
    public class TestWeapon : MonoBehaviour
    {
        private Weapon _weapon;
        private Gamepad _gamepad;

        private void Awake()
        {
            _weapon = GetComponent<Weapon>();
            _gamepad = Gamepad.current;
        }

        private void Update()
        {
            if (_gamepad.rightTrigger.isPressed)
            {
                _weapon.Shoot();
            }
        }
    }
}