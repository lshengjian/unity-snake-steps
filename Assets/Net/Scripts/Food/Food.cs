using System.Collections;
using UnityEngine;
using System;
namespace Mirror.MyGame
{
    public class Food : NetworkBehaviour
    {
        [SerializeField] GameObject particlePrefab;

        public static event Action<GameObject> ServerOnFoodEaten;
        [ServerCallback]
        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            ServerParticles();
            ServerOnFoodEaten?.Invoke(other.gameObject);
            NetworkServer.Destroy(gameObject);

        }


        void ServerParticles()
        {
            GameObject boom = Instantiate
                        (particlePrefab, transform.position,Quaternion.identity);
            NetworkServer.Spawn(boom);
      
        }

       
    }
}
