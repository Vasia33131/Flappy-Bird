using UnityEngine;

public class InfiniteGround : MonoBehaviour
{
    public GameObject groundPrefab;
    public GameObject ceilingPrefab; // Префаб для "потолка"
    public int numberOfSegments = 3;
    public float segmentWidth = 10f;

    private Transform[] groundSegments;
    private Transform[] ceilingSegments;
    private int groundLeftIndex = 0;
    private int groundRightIndex = 0;
    private int ceilingLeftIndex = 0;
    private int ceilingRightIndex = 0;
    private float viewZone = 20f;
    private Transform player;
    private float groundYPosition;
    private float ceilingYPosition;
    private float segmentHeight;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        groundSegments = new Transform[numberOfSegments];
        ceilingSegments = new Transform[numberOfSegments];

        // Получаем размеры сегментов
        segmentHeight = groundPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        // Вычисляем позиции для земли и потолка
        float bottomEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        float topEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        groundYPosition = bottomEdge + segmentHeight / 2;
        ceilingYPosition = topEdge - segmentHeight / 2;

        // Создаем начальные сегменты земли и потолка
        for (int i = 0; i < numberOfSegments; i++)
        {
            // Земля
            groundSegments[i] = Instantiate(groundPrefab,
                new Vector3(i * segmentWidth, groundYPosition, 0),
                Quaternion.identity).transform;

            // Потолок (перевернутый)
            ceilingSegments[i] = Instantiate(ceilingPrefab,
                new Vector3(i * segmentWidth, ceilingYPosition, 0),
                Quaternion.Euler(0, 0, 180f)).transform;
        }

        groundLeftIndex = 0;
        groundRightIndex = numberOfSegments - 1;
        ceilingLeftIndex = 0;
        ceilingRightIndex = numberOfSegments - 1;
    }

    void Update()
    {
        if (player.position.x > (groundSegments[groundRightIndex].position.x - viewZone))
        {
            ScrollRight(groundSegments, ref groundRightIndex, ref groundLeftIndex, groundYPosition);
            ScrollRight(ceilingSegments, ref ceilingRightIndex, ref ceilingLeftIndex, ceilingYPosition);
        }

        if (player.position.x < (groundSegments[groundLeftIndex].position.x + viewZone))
        {
            ScrollLeft(groundSegments, ref groundLeftIndex, ref groundRightIndex, groundYPosition);
            ScrollLeft(ceilingSegments, ref ceilingLeftIndex, ref ceilingRightIndex, ceilingYPosition);
        }
    }

    void ScrollRight(Transform[] segments, ref int rightIndex, ref int leftIndex, float yPosition)
    {
        segments[leftIndex].position = new Vector3(
            segments[rightIndex].position.x + segmentWidth,
            yPosition,
            0
        );

        rightIndex = leftIndex;
        leftIndex = (leftIndex + 1) % numberOfSegments;
    }

    void ScrollLeft(Transform[] segments, ref int leftIndex, ref int rightIndex, float yPosition)
    {
        segments[rightIndex].position = new Vector3(
            segments[leftIndex].position.x - segmentWidth,
            yPosition,
            0
        );

        leftIndex = rightIndex;
        rightIndex = (rightIndex - 1 + numberOfSegments) % numberOfSegments;
    }
}