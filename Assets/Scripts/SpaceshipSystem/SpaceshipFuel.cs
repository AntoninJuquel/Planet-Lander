using ReferenceSharing;
using UnityEngine;

namespace SpaceshipSystem
{
    public class SpaceshipFuel : MonoBehaviour
    {
        private Spaceship Data => SpaceshipManager.Data;
        [SerializeField] private Reference<float> verticalInputRef, fuelRef, maxFuelRef, fuelBurntRef;

        private void Update()
        {
            if (verticalInputRef.Value != 0 && fuelRef.Value > 0)
            {
                fuelRef.Value -= Time.deltaTime * Data.fuelBurnRate;
                fuelBurntRef.Value += Time.deltaTime * Data.fuelBurnRate;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Fuel"))
            {
                fuelRef.Value = maxFuelRef.Value;
                Destroy(col.gameObject);
            }
        }
    }
}