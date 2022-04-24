using MessagingSystem;
using ReferenceSharing;
using UnityEngine;

namespace PlayerSystem
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private int maxLives;
        [SerializeField] private Reference<int> lives;
        [SerializeField] private Reference<float> fuelRef, maxFuelRef;
        [SerializeField] private Player playerPrefab;
        private Player _player;

        private void OnEnable()
        {
            EventManager.Instance.AddListener<StartGameEvent>(StartGameHandler);
            EventManager.Instance.AddListener<EntityKilledEvent>(EntityKilledHandler);
            EventManager.Instance.AddListener<MainMenuEvent>(MainMenuHandler);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<StartGameEvent>(StartGameHandler);
            EventManager.Instance.RemoveListener<EntityKilledEvent>(EntityKilledHandler);
            EventManager.Instance.RemoveListener<MainMenuEvent>(MainMenuHandler);
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
            lives.Value = maxLives;
            fuelRef.Value = maxFuelRef.Value;
            Invoke(nameof(SpawnPlayer), .1f);
        }

        private void EntityKilledHandler(EntityKilledEvent e)
        {
            if (e.Transform != _player.transform) return;
            lives.Value--;
            if (lives > 0)
                SpawnPlayer();
            else
                Destroy(_player.gameObject);
            EventManager.Instance.Raise(new PlayerDeathEvent(lives == 0));
        }

        private void MainMenuHandler(MainMenuEvent e)
        {
            if (_player)
            {
                Destroy(_player.gameObject);
            }
        }
    }
}