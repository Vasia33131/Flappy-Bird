using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip backgroundMusic;
    public AudioClip scoreSound;

    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 0.7f;
    private float globalVolume = 1f; // Глобальная громкость

    private AudioSource musicSource;
    private AudioSource sfxSource;

    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeAudioSources();
        UpdateVolumes();
    }

    void InitializeAudioSources()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void UpdateVolumes()
    {
        musicSource.volume = musicVolume * globalVolume;
        sfxSource.volume = sfxVolume * globalVolume;
    }

    public void PlayScoreSound()
    {
        if (scoreSound != null && sfxSource != null)
        {
            Debug.Log("Playing score sound!"); // Добавьте эту строку
            sfxSource.PlayOneShot(scoreSound);
        }
        else
        {
            Debug.LogWarning("Score sound or SFX source is missing!");
        }
    }

    // Методы для управления глобальной громкостью
    public void SetGlobalVolume(float volume)
    {
        globalVolume = volume;
        UpdateVolumes();
    }

    public float GetGlobalVolume()
    {
        return globalVolume;
    }

    // Методы для UI слайдеров (если нужно отдельное управление)
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        UpdateVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        UpdateVolumes();
    }
}