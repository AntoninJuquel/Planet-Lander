﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MessagingSystem;
using ReferenceSharing;
using UnityEngine;

namespace EnemySystem
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private WavePreset[] wavePresets;
        [SerializeField] private Reference<int> kills, waveNumber, levelRef;
        private int _currentKill;
        private Dictionary<Transform, Enemy> _enemies = new Dictionary<Transform, Enemy>();
        private Transform _player;
        private int PresetIndex => levelRef.Value % wavePresets.Length;

        private void OnEnable()
        {
            EventManager.Instance.AddListener<StartGameEvent>(StartGameHandler);
            EventManager.Instance.AddListener<PlayerSpawnEvent>(PlayerSpawnHandler);
            EventManager.Instance.AddListener<EntityKilledEvent>(EntityKilledHandler);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<StartGameEvent>(StartGameHandler);
            EventManager.Instance.RemoveListener<PlayerSpawnEvent>(PlayerSpawnHandler);
            EventManager.Instance.RemoveListener<EntityKilledEvent>(EntityKilledHandler);
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

        private void NewWave()
        {
            waveNumber.Value = 0;
            _currentKill = 0;
            kills.Value = 0;
            StartCoroutine(SpawnWave(wavePresets[PresetIndex]));
        }

        private IEnumerator SpawnWave(WavePreset wavePreset)
        {
            var killToConfirmWave = 0;
            foreach (var wave in wavePreset.Waves)
            {
                waveNumber.Value++;
                killToConfirmWave += wave.enemyNumber;
                EventManager.Instance.Raise(new NewWaveEvent());
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

                EventManager.Instance.Raise(new WaveClearedEvent(waveNumber == wavePreset.Waves.Length));
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
            NewWave();
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