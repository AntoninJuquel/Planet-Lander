using System.Collections;
using MessagingSystem;
using ReferenceSharing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Screen = ScreenNavigation.Screen;

namespace UI
{
    public class HUD : Screen
    {
        [SerializeField] private Reference<bool> levelCleared;
        [SerializeField] private Reference<float> maxFuelRef, fuelRef, altitudeRef, speedRef;
        [SerializeField] private Reference<int> maxHealthRef, healthRef;
        [SerializeField] private TextMeshProUGUI altitude, speed, land, wave;
        [SerializeField] private Image health, fuel;

        private void OnEnable()
        {
            EventManager.Instance.AddListener<NewWaveEvent>(NewWaveHandler);
            EventManager.Instance.AddListener<WaveClearedEvent>(WaveClearedHandler);
            EventManager.Instance.AddListener<MainMenuEvent>(MainMenuHandler);
            EventManager.Instance.AddListener<GameOverEvent>(GameOverHandler);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<NewWaveEvent>(NewWaveHandler);
            EventManager.Instance.RemoveListener<WaveClearedEvent>(WaveClearedHandler);
            EventManager.Instance.RemoveListener<MainMenuEvent>(MainMenuHandler);
            EventManager.Instance.RemoveListener<GameOverEvent>(GameOverHandler);
        }

        private void NewWaveHandler(NewWaveEvent e)
        {
            if (e.Delay == 0) return;
            var values = new string[e.Delay + 1];

            for (var i = 0; i < e.Delay + 1; i++)
            {
                values[i] = $"PROCHAINE VAGUE DANS\n{e.Delay - i}";
            }

            StartCoroutine(wave.AnimateText(1f, values, false));
        }

        private void WaveClearedHandler(WaveClearedEvent e)
        {
            if (levelCleared.Value)
            {
                StartCoroutine(land.AnimateText(2f, new[] {"LAND", ">LAND<", ">>LAND<<", ">>>LAND<<<"}));
            }
        }

        private void MainMenuHandler(MainMenuEvent e)
        {
            land.enabled = false;
        }

        private void GameOverHandler(GameOverEvent e)
        {
            land.enabled = false;
        }

        private void Update()
        {
            health.fillAmount = (float) healthRef.Value / (float) maxHealthRef.Value;
            fuel.fillAmount = fuelRef.Value / maxFuelRef.Value;
            altitude.text = $"{altitudeRef.Value} m";
            speed.text = $"{speedRef.Value} m.s<sup>-1";
        }
    }

    public static class TextMeshProExtension
    {
        public static IEnumerator AnimateText(this TextMeshProUGUI text, float rate, string[] values, bool loop = true)
        {
            text.enabled = true;
            var waiteForSeconds = new WaitForSeconds(1f / rate);
            for (var i = 0; i < values.Length && text.enabled; i = loop ? (i + 1) % values.Length : i + 1)
            {
                text.text = values[i];
                yield return waiteForSeconds;
            }

            text.enabled = false;
        }
    }
}