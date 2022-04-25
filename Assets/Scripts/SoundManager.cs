using System.Collections;
using AudioSystem;
using MessagingSystem;
using ReferenceSharing;
using UnityEngine;
using UnityEngine.InputSystem;


public class SoundManager : MonoBehaviour
{
    [SerializeField] private Reference<float> forceInput;
    [SerializeField] private Reference<int> levelRef;
    [SerializeField] private AudioClip winJingleClip, loseJingleClip;
    [SerializeField] private AudioClip[] weaponShotClips, projectileHitClips, thrusterClips, entityKilledClips, spaceshipCrashedClips;
    [SerializeField] private AudioSource menuThemeSource, jingleAudioSource, spaceshipThrusterAudioSource, weaponShotAudioSource, projectileHitAudioSource, entityKilledAudioSource, spaceshipCrashedAudioSource;
    [SerializeField] private AudioSource[] musicAudioSources, spaceshipEngineAudioSources;
    private int _spaceShipEngineIndex;

    private void OnEnable()
    {
        EventManager.Instance.AddListener<MainMenuEvent>(MainMenuHandler);
        EventManager.Instance.AddListener<StartGameEvent>(StartGameHandler);
        EventManager.Instance.AddListener<GameOverEvent>(GameOverHandler);
        EventManager.Instance.AddListener<WeaponShotEvent>(WeaponShotHandler);
        EventManager.Instance.AddListener<ProjectileHitEvent>(ProjectileHitHandler);
        EventManager.Instance.AddListener<EntityKilledEvent>(EntityKilledHandler);
        EventManager.Instance.AddListener<SpaceshipCrashedEvent>(SpaceshipCrashedHandler);
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
    }

    private void Awake()
    {
        AudioManager.PauseSoundsOnApplicationPause = false;
    }

    private void Update()
    {
        spaceshipThrusterAudioSource.volume = forceInput.Value;
    }

    private void PlayMusic(int index)
    {
        PlayMusic(musicAudioSources[index]);
    }

    private void PlayMusic(AudioSource musicAudioSource)
    {
        musicAudioSource.PlayLoopingMusicManaged(.25f, 1f, false);
    }

    private void PlayJingle(AudioClip jingleAudioCLip)
    {
        jingleAudioSource.PlayOneShotSoundManaged(jingleAudioCLip, .5f);
    }

    private void PlaySpaceshipEngine()
    {
        _spaceShipEngineIndex = Random.Range(0, spaceshipEngineAudioSources.Length);
        spaceshipEngineAudioSources[_spaceShipEngineIndex].PlayLoopingSoundManaged(.05f, 1f);
    }

    private void MainMenuHandler(MainMenuEvent e)
    {
        spaceshipEngineAudioSources[_spaceShipEngineIndex].Stop();
        PlayMusic(menuThemeSource);
        spaceshipThrusterAudioSource.Stop();
    }

    private void StartGameHandler(StartGameEvent e)
    {
        PlayMusic(levelRef.Value % musicAudioSources.Length);
        spaceshipThrusterAudioSource.clip = thrusterClips[Random.Range(0, thrusterClips.Length)];
        spaceshipThrusterAudioSource.Play();
        PlaySpaceshipEngine();
    }

    private void GameOverHandler(GameOverEvent e)
    {
        AudioManager.StopAll();
        PlayJingle(e.Win ? winJingleClip : loseJingleClip);
        spaceshipThrusterAudioSource.Stop();
    }

    private void WeaponShotHandler(WeaponShotEvent e)
    {
        AudioManager.PlayOneShotSound(weaponShotAudioSource, weaponShotClips[Random.Range(0, weaponShotClips.Length)], 1f);
    }

    private void ProjectileHitHandler(ProjectileHitEvent e)
    {
        AudioManager.PlayOneShotSound(projectileHitAudioSource, projectileHitClips[Random.Range(0, projectileHitClips.Length)], 1f);
    }

    private void EntityKilledHandler(EntityKilledEvent e)
    {
        AudioManager.PlayOneShotSound(entityKilledAudioSource, entityKilledClips[Random.Range(0, entityKilledClips.Length)], 1f);
    }

    private void SpaceshipCrashedHandler(SpaceshipCrashedEvent e)
    {
        AudioManager.PlayOneShotSound(spaceshipCrashedAudioSource, spaceshipCrashedClips[Random.Range(0, spaceshipCrashedClips.Length)], 1f);
    }
}