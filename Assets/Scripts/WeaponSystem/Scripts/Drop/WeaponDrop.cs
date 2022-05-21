using TMPro;
using UnityEngine;

namespace WeaponSystem
{
    public class WeaponDrop : MonoBehaviour
    {
        [SerializeField] private Weapon[] weapons;
        private Weapon _weapon;

        private void Start()
        {
            _weapon = weapons[Random.Range(0, weapons.Length)];
            GetComponent<TextMeshPro>().text = _weapon.name;
        }

        public Weapon Loot()
        {
            Destroy(gameObject);
            return _weapon;
        }
    }
}