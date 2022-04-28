using System.Collections;
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
        [SerializeField] private Reference<bool> levelCleared;
        private int _currentKill;
        private Dictionary<Transform, Enemy> _enemies = new Dictionary<Transform, Enemy>();
        private Transform _player;
        private int PresetIndex => levelRef.Value % wavePresets.Length;
        private Coroutine _spawnRoutine;
        private Camera _mainCamera;

        private void OnEnable()
        {
            EventManager.Instance.AddListener<StartGameEvent>(StartGameHandler);
            EventManager.Instance.AddListener<PlayerSpawnEvent>(PlayerSpawnHandler);
            EventManager.Instance.AddListener<EntityKilledEvent>(EntityKilledHandler);
            EventManager.Instance.AddListener<MainMenuEvent>(MainMenuHandler);
            EventManager.Instance.AddListener<GameOverEvent>(GameOverHandler);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<StartGameEvent>(StartGameHandler);
            EventManager.Instance.RemoveListener<PlayerSpawnEvent>(PlayerSpawnHandler);
            EventManager.Instance.RemoveListener<EntityKilledEvent>(EntityKilledHandler);
            EventManager.Instance.RemoveListener<MainMenuEvent>(MainMenuHandler);
            EventManager.Instance.RemoveListener<GameOverEvent>(GameOverHandler);
        }

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void SpawnEnemy(Enemy enemy, Vector3 position)
        {
            var newEnemy = Instantiate(enemy, position, Quaternion.identity).GetComponent<Enemy>();
            newEnemy.SetTarget(_player);
            _enemies.Add(newEnemy.transform, newEnemy);
        }

        private void DestroyEnemy(Transform enemy, bool killCount)
        {
            Destroy(_enemies[enemy].gameObject);
            _enemies.Remove(enemy);
            _currentKill++;
            if (killCount)
                kills.Value++;
        }

        private void KillAll()
        {
            if (_spawnRoutine != null) StopCoroutine(_spawnRoutine);
            var enemies = _enemies.Keys.ToArray();

            for (var i = enemies.Length - 1; i >= 0; i--)
            {
                DestroyEnemy(enemies[i], false);
            }

            _enemies = new Dictionary<Transform, Enemy>();
        }

        private void StartEncounter()
        {
            levelCleared.Value = false;
            waveNumber.Value = 0;
            kills.Value = 0;
            _currentKill = 0;
            _spawnRoutine = StartCoroutine(SpawnWaves(wavePresets[PresetIndex]));
        }

        private IEnumerator SpawnWaves(WavePreset wavePreset)
        {
            var killToConfirmWave = 0;
            foreach (var wave in wavePreset.waves)
            {
                waveNumber.Value++;
                killToConfirmWave += wave.enemyNumber;
                EventManager.Instance.Raise(new NewWaveEvent(wave.timeDelay, waveNumber.Value == wavePreset.waves.Length));
                yield return new WaitForSeconds(wave.timeDelay);

                for (var i = 0; i < wave.enemyNumber; i++)
                {
                    yield return new WaitForSeconds(1f / wave.spawnRate);
                    var choice = Random.Range(0, wave.enemies.Length);
                    var position = (Vector2) _mainCamera.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(1.5f, 2f)));
                    SpawnEnemy(wave.enemies[choice], position);
                }

                if (wave.waitKill || waveNumber.Value == wavePreset.waves.Length)
                {
                    yield return new WaitUntil(() => _currentKill >= killToConfirmWave);
                    _currentKill = 0;
                    killToConfirmWave = 0;
                }

                levelCleared.Value = waveNumber.Value == wavePreset.waves.Length;
                EventManager.Instance.Raise(new WaveClearedEvent(wave.waitTime));
                yield return new WaitForSeconds(wave.waitTime);
            }

            Debug.Log("Level finished !");
        }

        private void StartGameHandler(StartGameEvent e)
        {
            KillAll();
            StartEncounter();
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
                DestroyEnemy(e.Transform, e.KillReg);
            }
        }

        private void MainMenuHandler(MainMenuEvent e)
        {
            KillAll();
        }

        private void GameOverHandler(GameOverEvent e)
        {
            KillAll();
        }
    }
}