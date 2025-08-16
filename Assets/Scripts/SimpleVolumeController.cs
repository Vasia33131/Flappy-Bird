using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))] // Гарантирует, что слайдер будет присутствовать
public class SimpleVolumeController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Image volumeIcon;

    [Header("Volume Sprites")]
    [Tooltip("0 - mute, 1 - low, 2 - medium, 3 - high volume")]
    [SerializeField] private Sprite[] volumeSprites = new Sprite[4]; // Инициализация массива

    private void Awake()
    {
        // Автоматическое получение ссылок, если они не назначены
        if (volumeSlider == null) volumeSlider = GetComponent<Slider>();
    }

    private void Start()
    {
        // Инициализация с проверками
        InitializeVolumeController();
    }

    private void InitializeVolumeController()
    {
        // Загрузка сохраненной громкости с проверкой
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        volumeSlider.value = savedVolume;

        // Настройка слушателя событий
        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeSlider.onValueChanged.AddListener(UpdateVolume);

        // Первоначальное обновление
        UpdateVolume(savedVolume);
    }

    private void UpdateVolume(float value)
    {
        // Применение громкости
        AudioListener.volume = value;

        // Обновление иконки с защитой от ошибок
        UpdateVolumeIcon(value);

        // Сохранение настроек
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();
    }

    private void UpdateVolumeIcon(float volume)
    {
        // Если нет иконки или спрайтов - выходим
        if (volumeIcon == null || volumeSprites == null || volumeSprites.Length == 0)
        {
            return;
        }

        // Определяем индекс спрайта с защитой от переполнения
        int spriteIndex = volume <= 0 ? 0 :
                         volume < 0.33f ? 1 :
                         volume < 0.66f ? 2 : 3;

        // Ограничиваем индекс размером массива
        spriteIndex = Mathf.Clamp(spriteIndex, 0, volumeSprites.Length - 1);

        // Проверяем, что спрайт существует
        if (volumeSprites[spriteIndex] != null)
        {
            volumeIcon.sprite = volumeSprites[spriteIndex];
        }
    }

    public void ToggleMute()
    {
        float newVolume = AudioListener.volume > 0 ? 0 : PlayerPrefs.GetFloat("LastVolume", 0.75f);

        if (AudioListener.volume > 0)
        {
            PlayerPrefs.SetFloat("LastVolume", AudioListener.volume);
        }

        AudioListener.volume = newVolume;
        volumeSlider.value = newVolume;
        UpdateVolumeIcon(newVolume);

        PlayerPrefs.Save();
    }
}