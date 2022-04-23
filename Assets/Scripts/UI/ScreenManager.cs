using Managers.Event;
using Managers.ScreenNavigator;

namespace UI
{
    public class ScreenManager : StackNavigator
    {
        private void OnEnable()
        {
            EventHandler.Instance.AddListener<StartGameEvent>(StartGameHandler);
            EventHandler.Instance.AddListener<TogglePauseEvent>(TogglePauseHandler);
            EventHandler.Instance.AddListener<MainMenuEvent>(MainMenuHandler);
            EventHandler.Instance.AddListener<GameOverEvent>(GameOverHandler);
        }

        private void OnDisable()
        {
            EventHandler.Instance.RemoveListener<StartGameEvent>(StartGameHandler);
            EventHandler.Instance.RemoveListener<TogglePauseEvent>(TogglePauseHandler);
            EventHandler.Instance.RemoveListener<MainMenuEvent>(MainMenuHandler);
            EventHandler.Instance.RemoveListener<GameOverEvent>(GameOverHandler);
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