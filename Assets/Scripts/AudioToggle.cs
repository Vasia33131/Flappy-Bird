using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AudioToggle : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private Sprite soundOnIcon;
    [SerializeField] private Sprite soundOffIcon;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Button toggleButton;
    [SerializeField] private float iconSwitchDuration = 0.2f;

    private bool isSoundEnabled = true;
    private static AudioToggle instance;
    private Dictionary<AudioSource, bool> audioSourcesState = new Dictionary<AudioSource, bool>();

    private void Awake()
    {
        // Реализация синглтона для сохранения между сценами
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Загрузка сохраненных настроек звука
        LoadAudioSettings();
    }

    private void Start()
    {
        // Инициализация кнопки
        toggleButton.onClick.AddListener(ToggleAudio);
        UpdateButtonIcon();
        ApplyAudioState();
    }

    public void ToggleAudio()
    {
        isSoundEnabled = !isSoundEnabled;
        ApplyAudioState();
        SaveAudioSettings();
        StartCoroutine(SwitchIconAnimation());
    }

    private void ApplyAudioState()
    {
        if (isSoundEnabled)
        {
            EnableAudio();
        }
        else
        {
            DisableAudio();
        }
    }

    private void DisableAudio()
    {
        // Сохраняем состояние всех аудиоисточников перед выключением
        audioSourcesState.Clear();
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSourcesState[audioSource] = audioSource.isPlaying;
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }

        // Отключаем звук
        AudioListener.volume = 0;
    }

    private void EnableAudio()
    {
        // Включаем звук
        AudioListener.volume = 1;

        // Восстанавливаем состояние аудиоисточников
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSourcesState.TryGetValue(audioSource, out bool wasPlaying) && wasPlaying)
            {
                audioSource.Play();
            }
        }
    }

    private void UpdateButtonIcon()
    {
        buttonImage.sprite = isSoundEnabled ? soundOnIcon : soundOffIcon;
    }

    private IEnumerator SwitchIconAnimation()
    {
        float elapsed = 0;
        Vector3 startScale = buttonImage.transform.localScale;
        Vector3 targetScale = startScale * 0.8f;

        // Анимация уменьшения
        while (elapsed < iconSwitchDuration / 2)
        {
            buttonImage.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / (iconSwitchDuration / 2));
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Смена иконки
        UpdateButtonIcon();

        // Анимация увеличения
        elapsed = 0;
        while (elapsed < iconSwitchDuration / 2)
        {
            buttonImage.transform.localScale = Vector3.Lerp(targetScale, startScale, elapsed / (iconSwitchDuration / 2));
            elapsed += Time.deltaTime;
            yield return null;
        }

        buttonImage.transform.localScale = startScale;
    }

    private void LoadAudioSettings()
    {
        isSoundEnabled = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
    }

    private void SaveAudioSettings()
    {
        PlayerPrefs.SetInt("SoundEnabled", isSoundEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Для переключения звука из других скриптов
    public static void SetAudioEnabled(bool enabled)
    {
        if (instance != null)
        {
            instance.isSoundEnabled = enabled;
            instance.ApplyAudioState();
            instance.UpdateButtonIcon();
            instance.SaveAudioSettings();
        }
    }
}