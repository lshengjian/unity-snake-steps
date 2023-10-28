using UnityEngine;

namespace Mirror.MyGame
{
    public class TailNetwork : NetworkBehaviour
    {
        [SyncVar]
        SnakeMovement owner;
        public SnakeMovement Owner
        {
            get { return owner; }
            private set { owner = value; }
        }

        [SyncVar]
        GameObject target;
        public GameObject Target
        {
            get { return target; }
            private set { target = value; }
        }

        void Update()
        {
            if (isServer)
            {
            }
        }
        public override void OnStartServer()
        {
            Owner = connectionToClient.identity.GetComponent<SnakeMovement>();
            var tails = Owner.GetComponent<TailSpawner>().Tails;
            Target = tails.Count == 0 ? Owner.gameObject : tails[tails.Count - 1];
            tails.Add(gameObject);




        }

        public override void OnStartClient()
        {
            var r = GetComponent<SpriteRenderer>();
            var player = Owner.GetComponent<Player>();
            var idx = player.index + 1;
            var tails = Owner.GetComponent<TailSpawner>().Tails;
            this.name = $"Player{idx}-{tails.Count}";
            r.color = player.color;

            this.transform.position = Target.transform.position-Vector3.right;


        }
    }
}