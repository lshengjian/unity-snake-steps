using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(BoxCollider2D))]
public class Snake : MonoBehaviour
{
    public GameObject segmentPrefab;


    public int initialSize = 4;


    private List<Transform> m_segments = new List<Transform>();

    private int m_score = 0;
    public int Score => m_score;
    RoleData m_config;
    public RoleData config => m_config;
    private ISnakeController m_controller;
    public ISnakeController controller => m_controller;
    private KeyInput m_keymgr;
    private void Awake()
    {
         m_config = Resources.Load<RoleData>(this.name);
        m_segments.Add(this.transform);
        m_controller = new SnakeController(initialSize, this.transform.localPosition);
        m_controller.OnSnakeChanged += DispalySnake;
       
    }
    void Start()
    {
        

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = m_config.color;
        m_keymgr = GetComponent<KeyInput>();
        m_keymgr.OnOneStep += Move;

    }

    void Move()
    {
        m_controller.Move(m_keymgr.direction);

    }

    public void GetFood()
    {
        m_score += 10;
        m_controller.Grow();

    }


    void Destroy()
    {
        m_keymgr.OnOneStep -= Move;

    }
    private void FixedUpdate()
    {


        if (m_controller.IsHitSelf())
        {
            m_controller.Reset();
            m_score = 0;


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
            sr.color = m_config.color;
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

}
