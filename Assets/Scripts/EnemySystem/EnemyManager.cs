﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers.Event;
using UnityEngine;

namespace EnemySystem
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private WavePreset[] wavePresets;
        private int _waveNumber, _totalKill, _currentKill;
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

        private void SpawnEnemy(EnemyPreset enemyPreset, Vector3 position)
        {
            var newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity).GetComponent<Enemy>();
            newEnemy.Setup(enemyPreset, _player);
            _enemies.Add(newEnemy.transform, newEnemy);
        }

        private void DestroyEnemy(Transform enemy)
        {
            Destroy(_enemies[enemy].gameObject);
            _enemies.Remove(enemy);
            _currentKill++;
            _totalKill++;
        }

        private void NewWave(int index)
        {
            _currentKill = 0;
            _totalKill = 0;
            StartCoroutine(SpawnWave(wavePresets[index]));
        }

        private IEnumerator SpawnWave(WavePreset wavePreset)
        {
            var killToConfirmWave = 0;
            foreach (var wave in wavePreset.Waves)
            {
                _waveNumber++;
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

                EventHandler.Instance.Raise(new WaveClearedEvent(_waveNumber == wavePreset.Waves.Length));
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
            NewWave(e.Level);
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