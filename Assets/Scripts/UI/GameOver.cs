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
        private Transform _player;

        private void OnEnable()
        {
            EventHandler.Instance.AddListener<PlayerSpawnEvent>(PlayerSpawnHandler);
            EventHandler.Instance.AddListener<GameOverEvent>(GameOverHandler);
        }

        private void OnDisable()
        {
            EventHandler.Instance.RemoveListener<PlayerSpawnEvent>(PlayerSpawnHandler);
            EventHandler.Instance.RemoveListener<GameOverEvent>(GameOverHandler);
        }

        private void PlayerSpawnHandler(PlayerSpawnEvent e)
        {
            _player = e.Player;
        }

        private void GameOverHandler(GameOverEvent e)
        {
            title.text = (e.Win ? "VICTOIRE" : "DEFAITE");
            next.interactable = e.Win;
        }
    }
}