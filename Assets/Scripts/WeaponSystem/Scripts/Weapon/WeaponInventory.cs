using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WeaponSystem
{
    public class WeaponInventory : MonoBehaviour
    {
        [SerializeField] private List<Weapon> weapons;
        public UnityEvent<Weapon, Weapon, Weapon> onWeaponChanged;
        private int _weaponIndex;
        private IHandleWeapon _handleWeapon;
        private Weapon CurrentWeapon => weapons[_weaponIndex];
        private Weapon NextWeapon => weapons[(_weaponIndex + 1) % weapons.Count];
        private Weapon PreviousWeapon => weapons[(_weaponIndex + weapons.Count - 1) % weapons.Count];
        private bool HasWeapon => weapons.Count > 0;

        private void Awake()
        {
            _handleWeapon = GetComponent<IHandleWeapon>();

            for (var i = 0; i < weapons.Count; i++)
            {
                var weaponName = weapons[i].name;
                weapons[i] = Instantiate(weapons[i]);
                weapons[i].name = weaponName;
            }

            if (_handleWeapon != null) return;

            Debug.LogWarning("IWeaponUser interface is not implemented on parent disabling the component", gameObject);
            enabled = false;
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => HasWeapon);
            onWeaponChanged?.Invoke(PreviousWeapon, CurrentWeapon, NextWeapon);
        }

        private void OnEnable()
        {
            if (_handleWeapon == null) return;
            _handleWeapon.OnSwitchWeapon += OnSwitchHandleWeapon;
        }

        private void OnDisable()
        {
            if (_handleWeapon == null) return;
            _handleWeapon.OnSwitchWeapon -= OnSwitchHandleWeapon;
        }

        private void OnSwitchHandleWeapon(object sender, int delta)
        {
            if (!HasWeapon) return;
            CurrentWeapon.lastTimeHeld = Time.time;
            _weaponIndex = delta < 0 ? (_weaponIndex + weapons.Count - 1) % weapons.Count : (_weaponIndex + 1) % weapons.Count;
            onWeaponChanged?.Invoke(PreviousWeapon, CurrentWeapon, NextWeapon);
        }

        public void Add(Weapon weapon)
        {
            var newWeapon = Instantiate(weapon);
            newWeapon.name = weapon.name;
            weapons.Add(newWeapon);
        }
    }
}