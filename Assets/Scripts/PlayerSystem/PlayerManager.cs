using System.Collections;
using Entities;
using Managers.Event;
using UnityEngine;

namespace PlayerSystem
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private int lives;
        [SerializeField] private Player playerPrefab;
        private Player _player;

        private void OnEnable()
        {
            EventHandler.Instance.AddListener<StartGameEvent>(StartGameHandler);
            EventHandler.Instance.AddListener<EntityKilledEvent>(EntityKilledHandler);
        }

        private void OnDisable()
        {
            EventHandler.Instance.RemoveListener<StartGameEvent>(StartGameHandler);
            EventHandler.Instance.RemoveListener<EntityKilledEvent>(EntityKilledHandler);
        }

        private void SpawnPlayer()
        {
            if (_player)
            {
                Destroy(_player.gameObject);
            }

            _player = Instantiate(playerPrefab, Vector3.up * 15, Quaternion.identity);
        }

        private void StartGameHandler(StartGameEvent e)
        {
            SpawnPlayer();
        }

        private void EntityKilledHandler(EntityKilledEvent e)
        {
            if (e.Transform != _player.transform) return;
            lives--;
            if (lives > 0)
                SpawnPlayer();
            EventHandler.Instance.Raise(new PlayerDeathEvent(lives == 0));
        }
    }
}