using HealthSystem;

namespace SpaceshipSystem
{
    public class SpaceshipHealth : Health
    {
        private Spaceship Data => SpaceshipManager.Data;

        private void Awake()
        {
            health.Value = maxHealth.Value = Data.maxHealth;
        }
    }
}