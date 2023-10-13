using UnityEngine;
using System.Collections.Generic;
using System;

//https://zhuanlan.zhihu.com/p/620002450

public delegate void DispalySnake(List<Vector2> segments);
public interface ISnakeController
{
    public event DispalySnake OnSnakeChanged;
    public void Reset();

    public Vector2 GetPosition(int index);
    public bool IsOccupies(int x, int y);
    public void Traverse(Vector2 wall, Vector2 direction);
    public void Move(Vector2Int direction);
    public void Grow();
    public bool IsOver();
}

public class SnakeController : ISnakeController
{
    public event DispalySnake OnSnakeChanged = (List<Vector2> segments) => { };

    public int InitSegments { get; private set; }

    private List<Vector2> m_segments = new List<Vector2>();
    public SnakeController(int initialSize = 4)
    {
        InitSegments = initialSize >= 2 ? initialSize : 2;
        Reset();
    }
    public void Reset()
    {
        m_segments.Clear();
        m_segments.Add(Vector2.zero);
        for (int i = 0; i < InitSegments - 1; i++)
            Grow();
        OnSnakeChanged.Invoke(m_segments);
    }
    public bool IsOver()
    {
        bool rt = false;
        var head = m_segments[0];
        for (int i = 1; i < m_segments.Count; i++)
        {
            if (head == m_segments[i])
            {
                rt = true;
                break;
            }
        }
        return rt;
    }
    public Vector2 GetPosition(int index)
    {
        return m_segments[index];

    }
    public void Traverse(Vector2 wall, Vector2 direction)
    {
        Debug.Log(wall);
        Vector3 position = m_segments[0];

        if (direction.x != 0f)
        {
            position.x = Mathf.RoundToInt(-wall.x + direction.x);
        }
        else if (direction.y != 0f)
        {
            position.y = Mathf.RoundToInt(-wall.y + direction.y);
        }
        m_segments[0] = position;

    }
    public bool IsOccupies(int x, int y)
    {
        foreach (Vector2 segment in m_segments)
        {
            if (Mathf.RoundToInt(segment.x) == x &&
                Mathf.RoundToInt(segment.y) == y)
            {
                return true;
            }
        }

        return false;
    }
    public void Move(Vector2Int direction)
    {
        for (int i = m_segments.Count - 1; i > 0; i--)
        {
            m_segments[i] = m_segments[i - 1];
        }

        int x = Mathf.RoundToInt(m_segments[0].x) + direction.x;
        int y = Mathf.RoundToInt(m_segments[0].y) + direction.y;
        m_segments[0] = new Vector2(x, y);
        OnSnakeChanged.Invoke(m_segments);
    }
    public void Grow()
    {
        Vector3 segment = m_segments[m_segments.Count - 1];//copy new data!
        m_segments.Add(segment);
    }
}
