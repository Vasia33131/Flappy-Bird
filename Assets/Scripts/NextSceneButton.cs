using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneButton : MonoBehaviour
{
    public void LoadNextScene()
    {
        // Получаем индекс текущей сцены
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Загружаем следующую сцену (текущий индекс + 1)
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}