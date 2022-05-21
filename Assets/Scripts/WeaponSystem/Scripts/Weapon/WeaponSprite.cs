using UnityEngine;

namespace WeaponSystem
{
    [RequireComponent(typeof(WeaponCore))]
    public class WeaponSprite : MonoBehaviour
    {
        private SpriteRenderer _sr;
        private WeaponCore _weaponCore;
        private WeaponInventory _weaponInventory;

        private void Awake()
        {
            _weaponCore = GetComponent<WeaponCore>();
            _weaponInventory = GetComponent<WeaponInventory>();
            _sr = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _weaponInventory.onWeaponChanged.AddListener(OnWeaponChanged);
        }

        private void OnDisable()
        {
            _weaponInventory.onWeaponChanged.RemoveListener(OnWeaponChanged);
        }

        private void OnWeaponChanged(Weapon previousWeapon, Weapon currenWeapon, Weapon nextWeapon)
        {
            _sr.sprite = currenWeapon.sprite;
        }
    }
}