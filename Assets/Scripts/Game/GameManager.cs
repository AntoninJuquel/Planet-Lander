using System;
using ReferenceSharing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game
{
    public enum State
    {
        Menu,
        Playing,
        Paused,
        GameOver
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Reference<int> levelRef, livesRef;
        [SerializeField] private Reference<bool> landedRef, waveFinished;
        [SerializeField] private UnityEvent<int> onStartGame;
        [SerializeField] private UnityEvent onPauseGame, onResumeGame, onMainMenu, onGameOver, onWin, onLose;
        [SerializeField] private InputAction pause;
        private State _state;

        private bool Playing => _state == State.Playing;
        private bool Paused => _state == State.Paused;

        private void Start()
        {
            onMainMenu?.Invoke();
        }

        private void OnEnable()
        {
            pause.Enable();

            pause.performed += PauseGame;

            livesRef.AddEventListener(CheckGameOver);
            landedRef.AddEventListener(CheckGameOver);
            waveFinished.AddEventListener(CheckGameOver);
        }

        private void OnDisable()
        {
            pause.Disable();

            pause.performed -= PauseGame;

            livesRef.RemoveEventListener(CheckGameOver);
            landedRef.RemoveEventListener(CheckGameOver);
            waveFinished.RemoveEventListener(CheckGameOver);
        }

        private void CheckGameOver(object sender, int value)
        {
            GameOver();
        }

        private void CheckGameOver(object sender, bool value)
        {
            GameOver();
        }

        private void GameOver()
        {
            if (livesRef.Value <= 0)
            {
                onGameOver?.Invoke();
                onLose?.Invoke();
                _state = State.GameOver;
            }
            else if (landedRef.Value && waveFinished.Value)
            {
                onGameOver?.Invoke();
                onWin?.Invoke();
                _state = State.GameOver;
            }
        }

        private void PauseGame(InputAction.CallbackContext c)
        {
            if (!Playing) return;

            if (Paused)
            {
                ResumeGame();
            }
            else
            {
                Time.timeScale = 0;
                _state = State.Paused;
                onPauseGame?.Invoke();
            }
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
            _state = State.Playing;
            onResumeGame?.Invoke();
        }

        public void MainMenu()
        {
            Time.timeScale = 1;
            _state = State.Menu;
            onMainMenu?.Invoke();
        }


        public void StartGame(int index)
        {
            levelRef.Value = index;
            _state = State.Playing;
            onStartGame?.Invoke(index + 1);
            Time.timeScale = 1;
        }

        public void StartGame()
        {
            StartGame(levelRef.Value);
        }

        public void NextLevel()
        {
            levelRef.Value++;
            StartGame();
        }

        public void QuitGame() => Application.Quit();
    }
}