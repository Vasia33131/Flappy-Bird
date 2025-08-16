using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    public void LoadMainMenu()
    {
        // Уничтожаем GameManager и AudioManager при возврате в меню
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        if (AudioManager.Instance != null)
        {
            Destroy(AudioManager.Instance.gameObject);
        }

        // Загружаем сцену главного меню
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}