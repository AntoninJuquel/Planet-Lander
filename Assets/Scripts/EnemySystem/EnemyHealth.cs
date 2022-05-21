using HealthSystem;

namespace EnemySystem
{
    public class EnemyHealth : Health
    {
        private void Awake()
        {
            health.Value = maxHealth.Value;
        }
    }
}