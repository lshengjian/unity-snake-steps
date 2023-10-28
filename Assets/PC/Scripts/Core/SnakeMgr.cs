using UnityEngine;
using System.Collections.Generic;


//https://zhuanlan.zhihu.com/p/620002450
namespace QFramework.MyGame
{
    public delegate void DispalySnake(List<Vector2> segments);
    public interface ISnakeMgr
    {
        public event DispalySnake OnSnakeChanged;
        public void Reset(Vector2 pos);
        public Vector2 HeadPosition { get; set; }
        public int NumSegments { get; }
        public Vector2 GetPosition(int index);//just for test
      //  public void Move(Vector2 direction);//just for test
        public bool IsOccupies(int x, int y);


        public void Grow();
        public void BreakTail();
        public bool IsHitSelf();
    }

    public class SnakeMgr : ISnakeMgr
    {
        public event DispalySnake OnSnakeChanged = (List<Vector2> segments) => { };

        public int InitSegments { get; private set; }

        private List<Vector2> m_segments = new List<Vector2>();
        public int NumSegments => m_segments.Count;
        public SnakeMgr(int initialSize,Vector2 pos)
        {
            InitSegments = initialSize >= 2 ? initialSize : 2;
           // m_segments.Add(pos);
            Reset(pos);
        }
        public Vector2 HeadPosition
        {
            get { return m_segments[0]; }
            set
            {
                if (value != m_segments[0])
                {
                    for (int i = m_segments.Count - 1; i > 0; i--)
                    {
                        m_segments[i] = m_segments[i - 1];
                    }

                    int x = Mathf.RoundToInt(value.x);
                    int y = Mathf.RoundToInt(value.y);
                    m_segments[0] = new Vector2(x, y);
                    OnSnakeChanged.Invoke(m_segments);

                }
            }


        }
        public void Reset(Vector2 pos)
        {
            m_segments.Clear();
            m_segments.Add(pos);
            for (int i = 0; i < InitSegments - 1; i++)
                Grow();
            OnSnakeChanged.Invoke(m_segments);
        }
        public bool IsHitSelf()
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
        public void Move(Vector2 direction){
            var head = m_segments[0];
            for (int i = m_segments.Count-1; i>0; i--)
                      m_segments[i]=m_segments[i-1];
             m_segments[0]=head+direction;
              OnSnakeChanged.Invoke(m_segments);
         
        }

        public void Grow()
        {
            Vector3 segment = m_segments[m_segments.Count - 1];//copy new data!
            m_segments.Add(segment);
        }
        public void BreakTail()
        {
            if (m_segments.Count > InitSegments)
            {
                m_segments.RemoveAt(m_segments.Count - 1);
            }
        }
    }
}