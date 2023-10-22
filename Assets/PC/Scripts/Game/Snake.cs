using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace QFramework.MyGame
{
    // [RequireComponent(typeof(BoxCollider2D))]
    public class Snake : MonoBehaviour, IController
    {
        public GameObject segmentPrefab;
        public Sprite headSprite;
        public Sprite bodySprite;
        private List<Transform> m_segments = new List<Transform>();
        RoleData m_config;

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return SnakeArchitecture.Interface;
        }



        void MoveSnake(EventSnakeMoved e)
        {
            if (e.name != this.name)
                return;
            List<Vector2> data = this.GetSystem<ISysMove>().GetPositions(e.name);
            DispalySnake(data);
        }
        void Start()
        {

            m_config = Resources.Load<RoleData>(this.name);
            this.RegisterEvent<EventSnakeMoved>(MoveSnake)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }


        void DispalySnake(List<Vector2> segments)
        {
            if (m_segments.Count != segments.Count)
            {
                foreach (var t in m_segments)
                    Destroy(t.gameObject);
                m_segments.Clear();
                for (int i = 0; i < segments.Count; i++)
                {
                    var obj = Instantiate(segmentPrefab, this.transform);
                    SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                    sr.color = m_config.color;
                    m_segments.Add(obj.transform);
                    if (i == 0)
                    {
                        obj.transform.localScale = Vector3.one * 1.2f;
                        sr.sprite = headSprite;
                    }
                    else
                    {
                        sr.sprite = bodySprite;
                    }

                }
            }

            for (int i = 0; i < segments.Count; i++)
                m_segments[i].localPosition = segments[i];

        }
    }
}