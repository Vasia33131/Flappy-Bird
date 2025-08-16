using UnityEngine;

public class BirdController : MonoBehaviour
{
    public float jumpForce = 5f;
    public float forwardSpeed = 2f;
    public float rotationSpeed = 5f;
    public float maxRotation = 35f;
    public float minRotation = -90f;

    private Rigidbody2D rb;
    private bool isDead = false;
    private GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f; // Начинаем с нулевой гравитации
        }

        gameManager = GameManager.Instance;

        var existingCollider = GetComponent<Collider2D>();
        if (existingCollider == null)
        {
            var collider = gameObject.AddComponent<CircleCollider2D>();
            collider.radius = 0.5f;
        }
    }

    void Update()
    {
        if (!isDead && gameManager != null)
        {
            // Если игра еще не началась, проверяем ввод для старта
            if (!gameManager.IsGameStarted && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
            {
                rb.gravityScale = 1f; // Включаем гравитацию
                gameManager.StartGame();
                Jump(); // Делаем первый прыжок при старте
                return;
            }

            // Если игра началась и не окончена
            if (gameManager.IsGameStarted && !gameManager.IsGameOver)
            {
                // Движение вперед
                transform.Translate(Vector2.right * forwardSpeed * Time.deltaTime);

                // Прыжок
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }

                // Поворот птицы
                float rotationZ = Mathf.Clamp(rb.velocity.y * rotationSpeed, minRotation, maxRotation);
                transform.rotation = Quaternion.Euler(0, 0, rotationZ);
            }
        }
    }

    private void Jump()
    {
        rb.velocity = Vector2.up * jumpForce;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDead && (collision.gameObject.CompareTag("Pipe") || collision.gameObject.CompareTag("Ground")))
        {
            isDead = true;
            gameManager?.GameOver();
            rb.velocity = Vector2.zero;
        }
    }
}