using UnityEngine;
namespace Mirror.MyGame
{
    public class SnakeMovement : NetworkBehaviour
    {


        [SyncVar]
        public Vector2Int direction = Vector2Int.right;






        [Command]
        void SetDir(Vector2Int dir)
        {
            direction = dir;

        }
          [ServerCallback]
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Tail")){
                Time.timeScale=0f;

            }
           

        }

        //[ClientCallback]
        void Update()
        {


            if (!Application.isFocused) return;

            var dir =direction;
            // movement for local player
            if (isLocalPlayer)
            {

               if (direction.x != 0f)
                {
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        dir = Vector2Int.up;

                    }
                    else if (Input.GetKeyDown(KeyCode.S))
                    {
                        dir = Vector2Int.down;

                    }
                }
                else if (direction.y != 0f)
                {
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        dir = Vector2Int.right;

                    }
                    else if (Input.GetKeyDown(KeyCode.A))
                    {
                        dir = Vector2Int.left;

                    }
                }
                if (dir != direction)
                    SetDir(dir);


            }
        }
    }
}


