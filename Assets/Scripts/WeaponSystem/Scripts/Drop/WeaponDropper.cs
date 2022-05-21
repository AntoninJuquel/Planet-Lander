using UnityEngine;

namespace WeaponSystem
{
    public class WeaponDropper : MonoBehaviour
    {
        [SerializeField] private float chance;
        [SerializeField] private WeaponDrop weaponDropPrefab;

        public void Drop(Vector2 position)
        {
            if (Random.value <= chance)
                Instantiate(weaponDropPrefab, position, Quaternion.identity);
        }
    }
}