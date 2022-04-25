using AudioSystem;
using MessagingSystem;
using ReferenceSharing;
using UnityEngine;

[System.Serializable]
public struct OneShotAudio
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioSource source;
    [SerializeField] [Range(0, 1)] private float scale;
    [SerializeField] private bool music;

    public void Play()
    {
        Play(Random.Range(0, clips.Length));
    }

    public void Play(int index)
    {
        index %= clips.Length;
        if (music)
            AudioManager.PlayOneShotMusic(source, clips[index], scale);
        else
            AudioManager.PlayOneShotSound(source, clips[index], scale);
    }
}

[System.Serializable]
public struct LoopingAudio
{
    [SerializeField] private AudioSource[] sources;
    [SerializeField] [Range(0, 1)] private float scale;
    [SerializeField] private bool music;
    private int _playingIndex;

    public void Play()
    {
        Play(Random.Range(0, sources.Length));
    }

    public void Play(int index)
    {
        index %= sources.Length;
        _playingIndex = index;
        if (music)
            AudioManager.PlayLoopingMusic(sources[index], scale);
        else
            AudioManager.PlayLoopingSound(sources[index], scale);
    }

    public void Stop()
    {
        if (music)
            AudioManager.StopLoopingMusic(sources[_playingIndex]);
        else
            AudioManager.StopLoopingSound(sources[_playingIndex]);
    }
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Reference<float> forceInput;
    [SerializeField] private Reference<int> levelRef;
    [SerializeField] private LoopingAudio musicAudio, spaceshipEngineAudio, thrusterAudio;
    [SerializeField] private OneShotAudio weaponShotAudio, projectileHitAudio, entityKilledAudio, spaceshipCrashedAudio, jingleAudio;

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

    private void TogglePauseHandler(TogglePauseEvent e)
    {
        if (e.Paused)
            AudioManager.PauseAll();
        else
            AudioManager.ResumeAll();
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.MusicVolume = value;
    }

    public void SetSoundVolume(float value)
    {
        AudioManager.SoundVolume = value;
    }
}