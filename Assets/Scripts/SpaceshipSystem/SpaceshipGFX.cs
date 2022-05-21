using ReferenceSharing;
using UnityEngine;

namespace SpaceshipSystem
{
    public class SpaceshipGFX : MonoBehaviour
    {
        private Spaceship Data => SpaceshipManager.Data;
        [SerializeField] private Reference<float> verticalInputRef, fuelRef;
        private ParticleSystem.EmissionModule _emission;
        private SpriteRenderer _sr;

        private void Awake()
        {
            _emission = GetComponent<ParticleSystem>().emission;
            _sr = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _sr.sprite = Data.sprite;
        }

        private void Update()
        {
            _emission.enabled = verticalInputRef.Value != 0 && fuelRef.Value > 0;
            _emission.rateOverDistanceMultiplier = Data.minParticleRate + verticalInputRef.Value * (Data.maxParticleRate - Data.minParticleRate);
        }
    }
}