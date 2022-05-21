using System;
using ReferenceSharing;
using UnityEngine;
using UnityEngine.InputSystem;
using WeaponSystem;

namespace SpaceshipSystem
{
    public class SpaceshipAttack : MonoBehaviour, IHandleWeapon
    {
        [SerializeField] private InputAction selectWeapon, shoot;
        [SerializeField] private Reference<string> previousWeaponRef, currentWeaponRef, nextWeaponRef;
        [SerializeField] private Reference<int> shotsRef, hitRef;
        [SerializeField] private Reference<float> accuracyRef;
        public bool PullTrigger { get; private set; }
        public event EventHandler<int> OnSwitchWeapon;

        private void OnEnable()
        {
            shoot.Enable();
            selectWeapon.Enable();


            selectWeapon.performed += OnSelectWeaponPerformed;
        }

        private void OnDisable()
        {
            shoot.Disable();
            selectWeapon.Disable();

            selectWeapon.performed -= OnSelectWeaponPerformed;
        }

        private void Update()
        {
            PullTrigger = shoot.IsPressed();
        }

        private void OnSelectWeaponPerformed(InputAction.CallbackContext c)
        {
            OnSwitchWeapon?.Invoke(this, (int) c.ReadValue<float>());
        }

        private void UpdateAccuracy()
        {
            accuracyRef.Value = 100f * hitRef.Value / shotsRef.Value;
        }

        public void OnWeaponChanged(Weapon previousWeapon, Weapon currenWeapon, Weapon nextWeapon)
        {
            previousWeaponRef.Value = previousWeapon.name;
            currentWeaponRef.Value = currenWeapon.name;
            nextWeaponRef.Value = nextWeapon.name;
        }

        public void OnShot()
        {
            shotsRef.Value++;
            UpdateAccuracy();
        }

        public void OnHit()
        {
            hitRef.Value++;
            UpdateAccuracy();
        }
    }
}