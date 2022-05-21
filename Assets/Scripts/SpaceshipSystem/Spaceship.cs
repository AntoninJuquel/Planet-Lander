using UnityEngine;

namespace SpaceshipSystem
{
    [CreateAssetMenu]
    public class Spaceship : ScriptableObject
    {
        public Sprite sprite;
        public int maxHealth;
        public float force;
        public float torqueSpeed;
        public float maxLandAngle;
        public float maxLandVelocity;
        public float fuelBurnRate;
        public float minParticleRate;
        public float maxParticleRate;
    }
}