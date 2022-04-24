using MessagingSystem;
using ReferenceSharing;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Reference<int> level;
    [SerializeField] private Reference<float> timer;
    [SerializeField] private Reference<bool> landed, levelCleared, hasLife, hasFuel;
    [SerializeField] private GameState state;
    private bool Playing => state == GameState.Playing;
    private bool Paused => state == GameState.Paused;

    private Transform _player;
    private float _startTime;

    private void OnEnable()
    {
        EventManager.Instance.AddListener<PlayerSpawnEvent>(PlayerSpawnHandler);
        EventManager.Instance.AddListener<PlayerDeathEvent>(PlayerDeathHandler);
        EventManager.Instance.AddListener<SpaceshipLandedEvent>(SpaceshipLandedHandler);
        EventManager.Instance.AddListener<WaveClearedEvent>(WaveClearedHandler);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<PlayerSpawnEvent>(PlayerSpawnHandler);
        EventManager.Instance.RemoveListener<PlayerDeathEvent>(PlayerDeathHandler);
        EventManager.Instance.RemoveListener<SpaceshipLandedEvent>(SpaceshipLandedHandler);
        EventManager.Instance.RemoveListener<WaveClearedEvent>(WaveClearedHandler);
    }

    private void Start()
    {
        MainMenu();
    }

    private void Update()
    {
        if (Gamepad.current.startButton.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    private void PlayerSpawnHandler(PlayerSpawnEvent e)
    {
        _player = e.Player;
    }

    private void PlayerDeathHandler(PlayerDeathEvent e)
    {
        CheckGameOver();
    }

    private void SpaceshipLandedHandler(SpaceshipLandedEvent e)
    {
        if (e.Transform != _player) return;
        CheckGameOver();
    }

    private void WaveClearedHandler(WaveClearedEvent e)
    {
        CheckGameOver();
    }

    private void CheckGameOver()
    {
        timer.Value = Time.time - _startTime;

        

        if (!hasLife.Value || (landed.Value && !hasFuel.Value))
        {
            EventManager.Instance.Raise(new GameOverEvent(false));
        }
        else if (landed.Value && levelCleared.Value)
        {
            EventManager.Instance.Raise(new GameOverEvent(true));
        }
    }

    private void StartGame()
    {
        StartGame(level.Value);
    }

    public void StartGame(int levelIndex)
    {
        _startTime = Time.time;

        level.Value = levelIndex;
        state = GameState.Playing;
        Time.timeScale = 1;
        EventManager.Instance.Raise(new StartGameEvent());
    }

    public void TogglePause()
    {
        if (Playing)
        {
            state = GameState.Paused;
            Time.timeScale = 0;
            EventManager.Instance.Raise(new TogglePauseEvent(true));
        }
        else if (Paused)
        {
            state = GameState.Playing;
            Time.timeScale = 1;
            EventManager.Instance.Raise(new TogglePauseEvent(false));
        }
    }

    public void MainMenu()
    {
        state = GameState.Menu;
        EventManager.Instance.Raise(new MainMenuEvent());
    }

    public void NextGame()
    {
        level.Value++;
        StartGame();
    }

    public void RestartGame()
    {
        StartGame();
    }

    public void StopGame()
    {
        EventManager.Instance.Raise(new StopGameEvent());
        Application.Quit();
    }
}

public enum GameState
{
    Menu,
    Playing,
    Paused,
    Win,
    Lose,
}