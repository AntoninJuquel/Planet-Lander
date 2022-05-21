using UnityEngine;

namespace SpaceshipSystem
{
    public class FuelDropper : MonoBehaviour
    {
        [SerializeField] private float chance;
        [SerializeField] private GameObject fuelDropPrefab;

        public void Drop(Vector2 position)
        {
            if (Random.value <= chance)
                Instantiate(fuelDropPrefab, position, Quaternion.identity);
        }
    }
}