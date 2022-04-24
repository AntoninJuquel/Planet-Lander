using MessagingSystem;
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
            EventManager.Instance.AddListener<StartGameEvent>(StartGameHandler);
            EventManager.Instance.AddListener<EntityKilledEvent>(EntityKilledHandler);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<StartGameEvent>(StartGameHandler);
            EventManager.Instance.RemoveListener<EntityKilledEvent>(EntityKilledHandler);
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
            Invoke(nameof(SpawnPlayer), .1f);
        }

        private void EntityKilledHandler(EntityKilledEvent e)
        {
            if (e.Transform != _player.transform) return;
            lives--;
            if (lives > 0)
                SpawnPlayer();
            else
                Destroy(_player.gameObject);
            EventManager.Instance.Raise(new PlayerDeathEvent(lives == 0));
        }
    }
}