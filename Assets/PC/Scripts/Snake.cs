using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Snake : MonoBehaviour
{
    public GameObject segmentPrefab;
    public int initialSize = 4;

    private List<Transform> m_segments = new List<Transform>();

    public BindableProperty<int> score = new BindableProperty<int>();
    RoleData m_config;
    public RoleData config => m_config;
    ISnakeController m_controller;
    public ISnakeController controller => m_controller;
    KeyInput m_keymgr;
    public Detector obstacleChecker;
    public Detector foodChecker;

    Game m_game;
    private void Awake()
    {
        m_config = Resources.Load<RoleData>(this.name);
        m_controller = new SnakeController(initialSize, this.transform.localPosition);
        m_controller.OnSnakeChanged += DispalySnake;
    }
    void Start()
    {
        m_game = FindAnyObjectByType<Game>();
        m_segments.Add(this.transform);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = m_config.color;
        m_keymgr = GetComponent<KeyInput>();
        obstacleChecker = GetComponent<Detector>();

        m_keymgr.OnOneStep += Move;
        obstacleChecker.OnTouched += OnHitBody;
        foodChecker.OnTouched += OnTouchFood;
    }
    void OnTouchFood(Transform snake, Transform food)
    {
        Debug.Log(snake.name + " get food");
        food.localPosition = m_game.GetFreePosition();
        // Snake s = snake.GetComponent<Snake>();
        AddScore();

    }
    void OnHitBody(Transform s, Transform body)
    {

        if (score.Value == 0)
            return;
        Debug.Log(s.name + " Hit Body");
        score.Value = 0;
        m_controller.Reset();



    }
    void Move()
    {
        m_controller.Move(m_keymgr.direction);

    }

    public void AddScore()
    {
        score.Value += 10;
        m_controller.Grow();

    }


    void Destroy()
    {
        m_keymgr.OnOneStep -= Move;
        obstacleChecker.OnTouched -= OnHitBody;

    }



    void DispalySnake(List<Vector2> segments)
    {
        // if (m_score == 0)
        //     return;
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
            sr.color = m_config.color;
        }

        for (int i = 0; i < segments.Count; i++)
        {
            if (m_segments[i])
                m_segments[i].localPosition = segments[i];
        }
    }

    public bool IsOccupies(int x, int y)
    {
        return m_controller.IsOccupies(x, y);
    }

}
