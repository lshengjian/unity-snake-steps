using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MyGame
{
    public class TailSpawner : NetworkBehaviour
    {
        const float delay = 0.2f;
        [SerializeField] GameObject tailPrefab;
        public List<GameObject> Tails { get; } = new List<GameObject>();
        //  [SyncVar] 
        float timer = 0;
         public  static bool isOver{get;set;}=false;

void Start(){
     TailSpawner.isOver=false;
}
        public override void OnStartServer()
        {
            Food.ServerOnFoodEaten += AddTail;
            // 
        }

        public override void OnStopServer()
        {
            Food.ServerOnFoodEaten -= AddTail;
        }

        void AddTail(GameObject playerWhoAte)
        {
            if (playerWhoAte != gameObject) return;
            var tailInstance = Instantiate(tailPrefab);
            //   if (Tails.Count==0) tailInstance.transform.localScale=Vector3.one*1.
            NetworkServer.Spawn(tailInstance, connectionToClient);//设置为本玩家的附属品

        }
        void Update()
        {
            if (isServer&&!isOver)
            {
                timer += Time.deltaTime;
                if (timer >= delay)
                {
                    timer = 0;
                    for (int i = Tails.Count - 1; i >= 0; i--)
                    {
                        var tail = Tails[i].GetComponent<TailNetwork>();
                        tail.transform.position = tail.Target.transform.position;

                    }
                    var move = GetComponent<SnakeMovement>();
                    var dir = move.direction;
                    move.transform.position += new Vector3(dir.x, dir.y, 0);


                }
            }
        }
    }
}