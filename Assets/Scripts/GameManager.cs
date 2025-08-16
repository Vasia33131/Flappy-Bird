using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public TMP_Text scoreText;
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;
    public TMP_Text startText; // Добавили текст для начального экрана

    public bool IsGameOver { get; private set; }
    public bool IsGameStarted { get; private set; } // Добавили флаг начала игры
    private int score = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        ResetGameState();
        Time.timeScale = 0f; // Останавливаем время в начале игры
        startText.gameObject.SetActive(true); // Показываем текст "Нажми"
        StartCoroutine(BlinkStartText()); // Запускаем моргание текста
    }

    public void StartGame()
    {
        if (!IsGameStarted)
        {
            IsGameStarted = true;
            Time.timeScale = 1f; // Возобновляем время
            startText.gameObject.SetActive(false); // Скрываем текст
        }
    }

    public void AddScore()
    {
        if (!IsGameOver && IsGameStarted)
        {
            score++;
            UpdateScoreText();
        }
    }

    public void GameOver()
    {
        IsGameOver = true;
        gameOverPanel.SetActive(true);
        finalScoreText.text = $"Счет: {score}";
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    private void ResetGameState()
    {
        score = 0;
        IsGameOver = false;
        IsGameStarted = false;
        gameOverPanel.SetActive(false);
        UpdateScoreText();
    }

    private System.Collections.IEnumerator BlinkStartText()
    {
        while (!IsGameStarted)
        {
            startText.alpha = 0f;
            yield return new WaitForSecondsRealtime(0.5f); // Используем WaitForSecondsRealtime, так как время остановлено
            startText.alpha = 1f;
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}