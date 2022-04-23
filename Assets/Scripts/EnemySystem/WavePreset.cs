using UnityEngine;

namespace EnemySystem
{
    [CreateAssetMenu(fileName = "New wave preset", menuName = "Wave", order = 0)]
    public class WavePreset : ScriptableObject
    {
        public Wave[] Waves;
    }

    [System.Serializable]
    public struct Wave
    {
        public int enemyNumber;
        public float spawnRate;
        public Enemy[] enemies;
        public float timeDelay;
        public bool waitKill;
        public float waitTime;
    }
}