using UnityEngine;

namespace EnemySystem
{
    [CreateAssetMenu(fileName = "New wave preset", menuName = "Wave", order = 0)]
    public class WavePreset : ScriptableObject
    {
        public Wave[] waves;
    }

    [System.Serializable]
    public struct Wave
    {
        public int enemyNumber, timeDelay, waitTime;
        public float spawnRate;
        public Enemy[] enemies;
        public bool waitKill;
    }
}