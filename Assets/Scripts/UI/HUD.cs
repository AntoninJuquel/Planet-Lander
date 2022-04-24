using MessagingSystem;
using ReferenceSharing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Screen = Managers.ScreenNavigator.Screen;

namespace UI
{
    public class HUD : Screen
    {
        [SerializeField] private Reference<float> maxFuelRef, fuelRef, altitudeRef, speedRef;
        [SerializeField] private Reference<int> maxHealthRef, healthRef;
        [SerializeField] private TextMeshProUGUI altitude, speed;
        [SerializeField] private Image health, fuel;
        private Transform _player;

        private void OnEnable()
        {
            EventManager.Instance.AddListener<PlayerSpawnEvent>(PlayerSpawnHandler);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<PlayerSpawnEvent>(PlayerSpawnHandler);
        }

        private void PlayerSpawnHandler(PlayerSpawnEvent e)
        {
            _player = e.Player;
            fuel.fillAmount = 1;
        }

        private void Update()
        {
            health.fillAmount = (float) healthRef.Value / (float) maxHealthRef.Value;
            fuel.fillAmount = fuelRef.Value / maxFuelRef.Value;
            altitude.text = $"{altitudeRef.Value} m";
            speed.text = $"{speedRef.Value} m.s<sup>-1";
        }
    }
}