using UnityEngine;

namespace HealthSystem
{
    public class HealthDropper : MonoBehaviour
    {
        [SerializeField] private float chance;
        [SerializeField] private GameObject healthDropPrefab;

        public void Drop(Vector2 position)
        {
            if (Random.value <= chance)
                Instantiate(healthDropPrefab, position, Quaternion.identity);
        }
    }
}