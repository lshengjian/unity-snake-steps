using System;

using UnityEngine;
namespace QFramework.MyGame
{
    public class KeyInput : MonoBehaviour, IController
    {
        public Vector2Int direction = Vector2Int.right;
        public float cooldown = 0.5f;

        private Vector2Int m_input = Vector2Int.right;

        private float m_nextUpdate = 0f;

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return SnakeArchitecture.Interface;
        }
        private RoleData m_config;

        void Start()
        {
            var gameModel = this.GetModel<GameModel>();
            var pd = gameModel.GetPlayerData(this.name);

            m_config = pd.config;
            pd.Score.Register(v =>
            {
                if (v < 100)
                    return;
                cooldown -= 0.0002f;
                if (cooldown < 0.01f)
                    cooldown = 0.01f;
            });

        }


        private void Update()
        {

            // Only allow turning up or down while moving in the x-axis
            if (direction.x != 0f)
            {
                if (Input.GetKeyDown(m_config.up))
                {
                    m_input = Vector2Int.up;
                }
                else if (Input.GetKeyDown(m_config.down))
                {
                    m_input = Vector2Int.down;
                }
            }
            // Only allow turning left or right while moving in the y-axis
            else if (direction.y != 0f)
            {
                if (Input.GetKeyDown(m_config.right))
                {
                    m_input = Vector2Int.right;
                }
                else if (Input.GetKeyDown(m_config.left))
                {
                    m_input = Vector2Int.left;
                }
            }
        }

        private void FixedUpdate()
        {

            // Wait until the next update before proceeding
            if (Time.time < m_nextUpdate)
                return;
            direction = m_input;
            CmdMove cmd = new CmdMove(this.name, direction);
            this.SendCommand(cmd);
            m_nextUpdate = Time.time + cooldown;
        }


    }

}
