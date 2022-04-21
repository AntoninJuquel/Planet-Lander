using Managers.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI altitude, speed, timer;
        [SerializeField] private Image health, fuel;
        private Transform _player;

        private void OnEnable()
        {
            EventHandler.Instance.AddListener<PlayerSpawnEvent>(PlayerSpawnHandler);
            EventHandler.Instance.AddListener<EntityDamagedEvent>(EntityDamagedHandler);
            EventHandler.Instance.AddListener<SpaceshipMetricsEvent>(SpaceshipMetricsHandler);
        }

        private void OnDisable()
        {
            EventHandler.Instance.RemoveListener<PlayerSpawnEvent>(PlayerSpawnHandler);
            EventHandler.Instance.RemoveListener<EntityDamagedEvent>(EntityDamagedHandler);
            EventHandler.Instance.RemoveListener<SpaceshipMetricsEvent>(SpaceshipMetricsHandler);
        }

        private void PlayerSpawnHandler(PlayerSpawnEvent e)
        {
            _player = e.Player;
        }

        private void EntityDamagedHandler(EntityDamagedEvent e)
        {
            if (e.Transform != _player) return;
            health.fillAmount = (float) e.Health / (float) e.MaxHealth;
        }

        private void SpaceshipMetricsHandler(SpaceshipMetricsEvent e)
        {
            if (e.Transform != _player) return;
            altitude.text = $"{e.Altitude} m";
            speed.text = $"{e.Speed} m.s<sup>-1";
            fuel.fillAmount = e.Fuel / e.MaxFuel;
        }
    }
}