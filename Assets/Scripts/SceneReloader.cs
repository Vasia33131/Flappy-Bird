using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    // Метод для перезагрузки текущей сцены
    public void ReloadCurrentScene()
    {
        // Получаем индекс текущей активной сцены
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Перезагружаем сцену по её индексу
        SceneManager.LoadScene(currentSceneIndex);

        // Альтернативный вариант (если нужно перезагрузить по имени):
        // string currentSceneName = SceneManager.GetActiveScene().name;
        // SceneManager.LoadScene(currentSceneName);
    }
}