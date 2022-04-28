using AudioSystem;
using MessagingSystem;
using ReferenceSharing;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Serialize Fields

    [SerializeField] private Reference<float> forceInput;
    [SerializeField] private Reference<int> levelRef;
    [SerializeField] private LoopingAudio musicAudio, spaceshipEngineAudio, thrusterAudio;
    [SerializeField] private OneShotAudio weaponShotAudio, projectileHitAudio, entityKilledAudio, spaceshipCrashedAudio, jingleAudio;

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        EventManager.Instance.AddListener<MainMenuEvent>(MainMenuHandler);
        EventManager.Instance.AddListener<StartGameEvent>(StartGameHandler);
        EventManager.Instance.AddListener<GameOverEvent>(GameOverHandler);
        EventManager.Instance.AddListener<WeaponShotEvent>(WeaponShotHandler);
        EventManager.Instance.AddListener<ProjectileHitEvent>(ProjectileHitHandler);
        EventManager.Instance.AddListener<EntityKilledEvent>(EntityKilledHandler);
        EventManager.Instance.AddListener<SpaceshipCrashedEvent>(SpaceshipCrashedHandler);
        EventManager.Instance.AddListener<TogglePauseEvent>(TogglePauseHandler);
        EventManager.Instance.AddListener<NewWaveEvent>(NewWaveHandler);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<MainMenuEvent>(MainMenuHandler);
        EventManager.Instance.RemoveListener<StartGameEvent>(StartGameHandler);
        EventManager.Instance.RemoveListener<GameOverEvent>(GameOverHandler);
        EventManager.Instance.RemoveListener<WeaponShotEvent>(WeaponShotHandler);
        EventManager.Instance.RemoveListener<ProjectileHitEvent>(ProjectileHitHandler);
        EventManager.Instance.RemoveListener<EntityKilledEvent>(EntityKilledHandler);
        EventManager.Instance.RemoveListener<SpaceshipCrashedEvent>(SpaceshipCrashedHandler);
        EventManager.Instance.RemoveListener<TogglePauseEvent>(TogglePauseHandler);
        EventManager.Instance.RemoveListener<NewWaveEvent>(NewWaveHandler);
    }

    private void Awake()
    {
        AudioManager.PauseSoundsOnApplicationPause = false;
    }

    private bool _thrusterPlaying;

    private void Update()
    {
        if (forceInput.Value == 0)
        {
            _thrusterPlaying = false;
            thrusterAudio.Stop();
        }
        else if (!_thrusterPlaying)
        {
            _thrusterPlaying = true;
            thrusterAudio.Play();
        }
    }

    #endregion

    #region Event Handlers

    private void MainMenuHandler(MainMenuEvent e)
    {
        spaceshipEngineAudio.Stop();
        musicAudio.Play(0);
    }

    private void StartGameHandler(StartGameEvent e)
    {
        spaceshipEngineAudio.Play();
        musicAudio.Play(levelRef.Value + 1);
    }

    private void TogglePauseHandler(TogglePauseEvent e)
    {
        if (e.Paused)
            AudioManager.PauseAll();
        else
            AudioManager.ResumeAll();
    }

    private void GameOverHandler(GameOverEvent e)
    {
        AudioManager.StopAll();
        jingleAudio.Play(e.Win ? 0 : 1);
    }

    private void WeaponShotHandler(WeaponShotEvent e)
    {
        weaponShotAudio.Play();
    }

    private void ProjectileHitHandler(ProjectileHitEvent e)
    {
        projectileHitAudio.Play();
    }

    private void EntityKilledHandler(EntityKilledEvent e)
    {
        entityKilledAudio.Play();
    }

    private void SpaceshipCrashedHandler(SpaceshipCrashedEvent e)
    {
        spaceshipCrashedAudio.Play();
    }

    private void NewWaveHandler(NewWaveEvent e)
    {
        if (e.LastWave)
        {
            musicAudio.Play(4);
        }
    }

    #endregion

    #region Public Methods

    public void SetMusicVolume(float value)
    {
        AudioManager.MusicVolume = value;
    }

    public void SetSoundVolume(float value)
    {
        AudioManager.SoundVolume = value;
    }

    #endregion
}