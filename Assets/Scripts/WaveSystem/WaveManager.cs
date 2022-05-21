using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace WaveSystem
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private Wave[] waves;
        [SerializeField] private UnityEvent onNewWave, onWaveCleared, onWaveFinished;
        [SerializeField] private UnityEvent<GameObject> onWaveSpawn;
        private int _killToConfirmWave, _currentKillToConfirmWave;
        private IEnumerable<IHandleWave> _waveHandlers;
        private Coroutine _encounter;

        private void Awake()
        {
            _waveHandlers = FindObjectsOfType<MonoBehaviour>().OfType<IHandleWave>();
        }

        private void OnEnable()
        {
            foreach (var waveHandler in _waveHandlers)
            {
                onNewWave.AddListener(waveHandler.OnNewWave);
                onWaveCleared.AddListener(waveHandler.OnWaveCleared);
                onWaveSpawn.AddListener(waveHandler.OnWaveSpawn);
                onWaveFinished.AddListener(waveHandler.OnWaveFinished);

                waveHandler.OnStartWave += StartWave;
                waveHandler.OnStopWave += StopWave;
            }
        }

        private void OnDisable()
        {
            foreach (var waveHandler in _waveHandlers)
            {
                onNewWave.RemoveListener(waveHandler.OnNewWave);
                onWaveCleared.RemoveListener(waveHandler.OnWaveCleared);
                onWaveSpawn.RemoveListener(waveHandler.OnWaveSpawn);
                onWaveFinished.RemoveListener(waveHandler.OnWaveFinished);

                waveHandler.OnStartWave -= StartWave;
                waveHandler.OnStopWave -= StopWave;
            }
        }

        private IEnumerator SpawnWaves(Wave wavePreset)
        {
            _killToConfirmWave = _currentKillToConfirmWave = 0;

            for (var waveIndex = 0; waveIndex < wavePreset.waves.Length; waveIndex++)
            {
                var wave = wavePreset.waves[waveIndex];
                _killToConfirmWave += wave.enemyNumber;
                onNewWave?.Invoke();
                yield return new WaitForSeconds(wave.timeDelay);

                for (var i = 0; i < wave.enemyNumber; i++)
                {
                    yield return new WaitForSeconds(1f / wave.spawnRate);
                    onWaveSpawn?.Invoke(wave.enemies[Random.Range(0, wave.enemies.Length)]);
                }

                if (wave.waitKill || waveIndex == wavePreset.waves.Length)
                {
                    yield return new WaitUntil(() => _currentKillToConfirmWave >= _killToConfirmWave);
                    _currentKillToConfirmWave = 0;
                    _killToConfirmWave = 0;
                }

                onWaveCleared?.Invoke();
                yield return new WaitForSeconds(wave.waitTime);
            }

            onWaveFinished?.Invoke();
        }

        private void StartWave(object sender, int index)
        {
            if (_encounter != null)
            {
                return;
            }

            _encounter = StartCoroutine(SpawnWaves(waves[index % waves.Length]));
        }

        private void StopWave(object sender, EventArgs args)
        {
            if (_encounter == null)
            {
                return;
            }

            StopCoroutine(_encounter);
            _encounter = null;
        }
    }
}