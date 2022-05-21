using UnityEngine;
using UnityEngine.Events;

namespace WeaponSystem
{
    public class WeaponLooter : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Weapon> onLoot;
        private WeaponInventory _weaponInventory;

        private void Awake()
        {
            _weaponInventory = GetComponentInChildren<WeaponInventory>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var isWeapon = col.GetComponent<WeaponDrop>();
            if (isWeapon)
            {
                _weaponInventory.Add(isWeapon.Loot());
                onLoot?.Invoke(isWeapon.Loot());
            }
        }
    }
}