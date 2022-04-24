using MessagingSystem;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private float _startTime;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void StartGameHandler(StartGameEvent e)
    {
        _startTime = Time.time;
    }
}