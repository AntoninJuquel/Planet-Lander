using MessagingSystem;
using ReferenceSharing;
using UnityEngine;
using UnityEngine.InputSystem;
using WeaponSystem;

namespace PlayerSystem
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Reference<Vector3> offsetRef;
        [SerializeField] private Reference<float> forceInput, torqueInput;
        [SerializeField] private Weapon mainWeapon, counterMeasures, specialWeapon;
        private Gamepad _input;
        private PlayerSpawnEvent _playerSpawnEvent;

        private void Awake()
        {
            _input = Gamepad.current;
            _playerSpawnEvent = new PlayerSpawnEvent(transform);
            mainWeapon = GetComponentInChildren<Weapon>();
        }

        private void Start()
        {
            EventManager.Instance.Raise(_playerSpawnEvent);
        }

        private void Update()
        {
            forceInput.Value = _input.rightTrigger.ReadValue();
            torqueInput.Value = _input.leftStick.ReadValue().x;
            offsetRef.Value = _input.rightStick.ReadValue();

            if (_input.buttonSouth.isPressed)
            {
                mainWeapon.Shoot();
            }

            if (_input.buttonEast.isPressed)
            {
                counterMeasures.Shoot();
            }

            if (_input.buttonWest.isPressed)
            {
                specialWeapon.Shoot();
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Weapon"))
            {
                mainWeapon.SwitchWeapon(1);
            }
        }
    }
}