using UnityEngine;

namespace WaveSystem
{
    [CreateAssetMenu]
    public class Wave : ScriptableObject
    {
        public WaveGroup[] waves;
    }
    
    [System.Serializable]
    public struct WaveGroup
    {
        public int enemyNumber, timeDelay;
        public float spawnRate;
        public GameObject[] enemies;
        public float waitTime;
        public bool waitKill;
    }
}
