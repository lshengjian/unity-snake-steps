using UnityEngine;

namespace Mirror.MyGame
{
    public class SnakeMovement : NetworkBehaviour
    {

       // GameObject overUI;
        [SyncVar]
        public Vector2Int direction = Vector2Int.right;

     public override void OnStartClient()
        {
            base.OnStartClient();
         // overUI=GameObject.Find("Canvas").transform.GetChild(1).gameObject;
           // overUI =transform.parent.Find("Canvas/GameOver").gameObject;
          //  Debug.Log(overUI.name);
        }




        [Command]
        void SetDir(Vector2Int dir)
        {
            direction = dir;

        }
        [ServerCallback]
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Tail"))
            {
                //  Debug.Log("Hit Tail");
                RpcGameOver();

            }


        }
        [ClientRpc]
        void RpcGameOver()
        {

           // Debug.Log("Hit Tail");
           
          //  overUI.SetActive(true);
          //  Time.timeScale=0f;
            CanvasUI.SetActive(false);
            this.enabled=false;
            TailSpawner.isOver=true;
            // Invoke("BackToMenu", 0.2f);
        }

        public void Close()
        {
            if (NetworkServer.active && NetworkClient.isConnected)
                NetworkManager.singleton.StopHost();
            else NetworkManager.singleton.StopClient();
        }


        //[ClientCallback]
        void Update()
        {


            if (!Application.isFocused) return;

            var dir = direction;
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


