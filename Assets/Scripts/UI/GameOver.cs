using Managers.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Screen = Managers.ScreenNavigator.Screen;

namespace UI
{
    public class GameOver : Screen
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Button next;

        private void OnEnable()
        {
            EventHandler.Instance.AddListener<GameOverEvent>(GameOverHandler);
        }

        private void OnDisable()
        {
            EventHandler.Instance.RemoveListener<GameOverEvent>(GameOverHandler);
        }

        private void GameOverHandler(GameOverEvent e)
        {
            title.text = (e.Win ? "VICTOIRE" : "DEFAITE");
            next.interactable = e.Win;
        }
    }
}