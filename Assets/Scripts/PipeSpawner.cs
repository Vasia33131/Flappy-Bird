using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject pipePrefab;
    public float pipeGap = 3f;
    public float spawnRate = 2f;

    [Header("Sorting Settings")]
    public string pipesSortingLayer = "Pipes";
    public int bottomPipeOrder = 1;
    public int topPipeOrder = 2;
    public int scoreTriggerOrder = 0;

    [Tooltip("Родительский объект для всех создаваемых труб")]
    public Transform pipesParent;

    private Camera mainCam;
    private float timer;

    void Start()
    {
        mainCam = Camera.main;
        if (pipesParent == null) pipesParent = transform;
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver) return;

        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            SpawnPipes();
            timer = 0;
        }
    }

    void SpawnPipes()
    {
        float x = mainCam.ViewportToWorldPoint(new Vector3(1.1f, 0)).x;
        float y = Random.Range(-2f, 2f);

        CreatePipe(x, y + pipeGap / 2, 180, topPipeOrder);    // Верхняя труба
        CreatePipe(x, y - pipeGap / 2, 0, bottomPipeOrder);   // Нижняя труба
        CreateScoreTrigger(x, y, scoreTriggerOrder);
    }

    void CreatePipe(float x, float y, float rotation, int sortingOrder)
    {
        GameObject pipe = Instantiate(pipePrefab, new Vector3(x, y, 0),
            Quaternion.Euler(0, 0, rotation), pipesParent);

        SpriteRenderer renderer = pipe.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sortingLayerName = pipesSortingLayer;
            renderer.sortingOrder = sortingOrder;
        }
    }

    void CreateScoreTrigger(float x, float y, int sortingOrder)
    {
        GameObject trigger = new GameObject("ScoreTrigger");
        trigger.transform.SetParent(pipesParent);
        trigger.transform.position = new Vector3(x, y, 0);

        // Добавляем SpriteRenderer для визуализации (можно отключить)
        SpriteRenderer sr = trigger.AddComponent<SpriteRenderer>();
        sr.sprite = null; // Нет спрайта по умолчанию
        sr.sortingLayerName = pipesSortingLayer;
        sr.sortingOrder = sortingOrder;
        sr.enabled = false; // Делаем невидимым

        BoxCollider2D collider = trigger.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(0.5f, pipeGap * 0.8f);

        trigger.AddComponent<ScoreTrigger>();
        Destroy(trigger, 10f);
    }
}