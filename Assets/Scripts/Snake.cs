using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(BoxCollider2D))]
public class Snake : MonoBehaviour
{
    public GameObject segmentPrefab;
    public TextMeshProUGUI score_text;


    public RoleData config;
    public Vector2Int direction = Vector2Int.right;
    public float speed = 20f;
    public float speedMultiplier = 1f;
    public int initialSize = 4;


    private List<Transform> m_segments = new List<Transform>();
    private Vector2Int m_input;
    private float m_nextUpdate;

    private Game m_game;
    private int m_score = 0;

    private ISnakeController m_controller;
    private void Awake()
    {
        m_segments.Add(this.transform);
        m_controller = new SnakeController(initialSize);
        m_controller.OnSnakeChanged += DispalySnake;
    }
    void Start()
    {
        m_game = FindAnyObjectByType<Game>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color =  config.color;
        score_text.color=config.color;
       // print(Color.red);

    }

    private void Update()
    {


        // Only allow turning up or down while moving in the x-axis
        if (direction.x != 0f)
        {
            if (Input.GetKeyDown(config.up))
            {
                m_input = Vector2Int.up;
            }
            else if (Input.GetKeyDown(config.down))
            {
                m_input = Vector2Int.down;
            }
        }
        // Only allow turning left or right while moving in the y-axis
        else if (direction.y != 0f)
        {
            if (Input.GetKeyDown(config.right))
            {
                m_input = Vector2Int.right;
            }
            else if (Input.GetKeyDown(config.left))
            {
                m_input = Vector2Int.left;
            }
        }
    }

    private void FixedUpdate()
    {

        // Wait until the next update before proceeding
        if (Time.time < m_nextUpdate)
        {
            return;
        }

        // Set the new direction based on the input
        if (m_input != Vector2Int.zero)
        {
            direction = m_input;
        }

        m_controller.Move(direction);

        // Set the next update time based on the speed
        m_nextUpdate = Time.time + (1f / (speed * speedMultiplier));
        if (m_controller.IsOver())
        {
            m_controller.Reset();

        }
    }


    void DispalySnake(List<Vector2> segments)
    {
        // Debug.Log(segments.Count + " " + m_segments.Count);
        if (m_segments.Count > segments.Count)
        {
            foreach (var t in m_segments)
            {
                if (t == transform)
                    continue;
                Destroy(t.gameObject);
            }
            m_segments.Clear();
            this.transform.localPosition = segments[0];
            m_segments.Add(this.transform);

        }
        int cnt = segments.Count - m_segments.Count;

        for (int i = 0; i < cnt; i++)
        {
            var obj = Instantiate(segmentPrefab);
            m_segments.Add(obj.transform);
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            sr.color = config.color;
        }

        for (int i = 0; i < segments.Count; i++)
        {
            m_segments[i].localPosition = segments[i];
        }
    }

    public bool IsOccupies(int x, int y)
    {
        return m_controller.IsOccupies(x, y);
    }
    void UpdateScore(int new_score)
    {
        if (score_text == null)
            return;
        m_score = new_score;
        score_text.text = m_score.ToString();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            m_controller.Grow();
            UpdateScore(m_score + 5);
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            // Debug.Log("hit Obstacle ");
            int s = m_score - 10;
            if (s < 0) s = 0;
            UpdateScore(s);
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            if (m_game.moveThroughWalls)
            {
                
                m_controller.Traverse(other.transform.localPosition, direction);
            }
            else
            {
                m_controller.Reset();

                UpdateScore(0);
                Debug.Log("Reset ");
            }
        }
    }



}
