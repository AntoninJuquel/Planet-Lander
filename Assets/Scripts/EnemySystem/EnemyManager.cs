using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers.Event;
using ReferenceSharing;
using UnityEngine;

namespace EnemySystem
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private WavePreset[] wavePresets;
        [SerializeField] private Reference<int> kills, waveNumber;
        private int _currentKill;
        private Dictionary<Transform, Enemy> _enemies = new Dictionary<Transform, Enemy>();
        private Transform _player;

        private void OnEnable()
        {
            EventHandler.Instance.AddListener<StartGameEvent>(StartGameHandler);
            EventHandler.Instance.AddListener<PlayerSpawnEvent>(PlayerSpawnHandler);
            EventHandler.Instance.AddListener<EntityKilledEvent>(EntityKilledHandler);
        }

        private void OnDisable()
        {
            EventHandler.Instance.RemoveListener<StartGameEvent>(StartGameHandler);
            EventHandler.Instance.RemoveListener<PlayerSpawnEvent>(PlayerSpawnHandler);
            EventHandler.Instance.RemoveListener<EntityKilledEvent>(EntityKilledHandler);
        }

        private void SpawnEnemy(Enemy enemy, Vector3 position)
        {
            var newEnemy = Instantiate(enemy, position, Quaternion.identity).GetComponent<Enemy>();
            newEnemy.SetTarget(_player);
            _enemies.Add(newEnemy.transform, newEnemy);
        }

        private void DestroyEnemy(Transform enemy)
        {
            Destroy(_enemies[enemy].gameObject);
            _enemies.Remove(enemy);
            _currentKill++;
            kills.Value++;
        }

        private void NewWave(int index)
        {
            waveNumber.Value = 0;
            _currentKill = 0;
            kills.Value = 0;
            StartCoroutine(SpawnWave(wavePresets[index]));
        }

        private IEnumerator SpawnWave(WavePreset wavePreset)
        {
            var killToConfirmWave = 0;
            foreach (var wave in wavePreset.Waves)
            {
                waveNumber.Value++;
                killToConfirmWave += wave.enemyNumber;
                EventHandler.Instance.Raise(new NewWaveEvent());
                yield return new WaitForSeconds(wave.timeDelay);

                for (var i = 0; i < wave.enemyNumber; i++)
                {
                    yield return new WaitForSeconds(1f / wave.spawnRate);
                    var choice = Random.Range(0, wave.enemies.Length);
                    SpawnEnemy(wave.enemies[choice], Vector3.up * 20);
                }

                if (wave.waitKill)
                {
                    yield return new WaitUntil(() => _currentKill >= killToConfirmWave);
                    _currentKill = 0;
                    killToConfirmWave = 0;
                }

                EventHandler.Instance.Raise(new WaveClearedEvent(waveNumber == wavePreset.Waves.Length));
                yield return new WaitForSeconds(wave.waitTime);
            }

            Debug.Log("Level finished !");
        }

        private void StartGameHandler(StartGameEvent e)
        {
            var enemies = _enemies.Keys.ToArray();

            for (var i = enemies.Length - 1; i >= 0; i--)
            {
                DestroyEnemy(enemies[i]);
            }

            _enemies = new Dictionary<Transform, Enemy>();
            NewWave(e.Level % wavePresets.Length);
        }

        private void PlayerSpawnHandler(PlayerSpawnEvent e)
        {
            _player = e.Player;
            foreach (var kvp in _enemies)
            {
                kvp.Value.SetTarget(_player);
            }
        }

        private void EntityKilledHandler(EntityKilledEvent e)
        {
            if (_enemies.ContainsKey(e.Transform))
            {
                DestroyEnemy(e.Transform);
            }
        }
    }
}