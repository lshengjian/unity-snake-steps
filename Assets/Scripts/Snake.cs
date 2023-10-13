using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Snake : MonoBehaviour
{
    public GameObject segmentPrefab;
    public Vector2Int direction = Vector2Int.right;
    public float speed = 20f;
    public float speedMultiplier = 1f;
    public int initialSize = 4;
    public bool moveThroughWalls = false;

    private List<Transform> m_segments = new List<Transform>();
    private Vector2Int m_input;
    private float m_nextUpdate;

    private GameObject[] m_walls;

    private ISnakeController m_controller;
    private void Awake()
    {
        m_segments.Add(this.transform);
        m_controller = new SnakeController(initialSize);
        m_controller.OnSnakeChanged += DispalySnake;
    }

    private void Start()
    {


        m_walls = GameObject.FindGameObjectsWithTag("Wall");
        SetWallColor();
    }

    void SetWallColor()
    {
        foreach (GameObject wall in m_walls)
        {
            SpriteRenderer sp = wall.GetComponent<SpriteRenderer>();
            sp.material.color = moveThroughWalls ? Color.green : Color.gray;
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            moveThroughWalls = !moveThroughWalls;
            SetWallColor();
        }

        // Only allow turning up or down while moving in the x-axis
        if (direction.x != 0f)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                m_input = Vector2Int.up;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                m_input = Vector2Int.down;
            }
        }
        // Only allow turning left or right while moving in the y-axis
        else if (direction.y != 0f)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                m_input = Vector2Int.right;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            m_controller.Grow();
            //Debug.Log("Food ");
        }
        // else if (other.gameObject.CompareTag("Obstacle"))
        // {
        //      m_controller.Reset();
        // }
        else if (other.gameObject.CompareTag("Wall"))
        {
            if (moveThroughWalls)
            {
                // Debug.Log("Traverse Wall ");
                m_controller.Traverse(other.transform.localPosition, direction);
            }
            else
            {
                m_controller.Reset();
                Debug.Log("Reset ");
            }
        }
    }



}
