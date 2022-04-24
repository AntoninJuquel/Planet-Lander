using MessagingSystem;
using Managers.ScreenNavigator;

namespace UI
{
    public class ScreenManager : StackNavigator
    {
        private void OnEnable()
        {
            EventManager.Instance.AddListener<StartGameEvent>(StartGameHandler);
            EventManager.Instance.AddListener<TogglePauseEvent>(TogglePauseHandler);
            EventManager.Instance.AddListener<MainMenuEvent>(MainMenuHandler);
            EventManager.Instance.AddListener<GameOverEvent>(GameOverHandler);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<StartGameEvent>(StartGameHandler);
            EventManager.Instance.RemoveListener<TogglePauseEvent>(TogglePauseHandler);
            EventManager.Instance.RemoveListener<MainMenuEvent>(MainMenuHandler);
            EventManager.Instance.RemoveListener<GameOverEvent>(GameOverHandler);
        }

        private void TogglePauseHandler(TogglePauseEvent e)
        {
            if (e.Paused)
            {
                Navigate("Pause");
            }
            else
            {
                Navigate("HUD");
            }
        }

        private void StartGameHandler(StartGameEvent e)
        {
            Navigate("HUD");
        }

        private void MainMenuHandler(MainMenuEvent e)
        {
            Navigate("Menu");
        }

        private void GameOverHandler(GameOverEvent e)
        {
            Navigate("GameOver");
        }
    }
}