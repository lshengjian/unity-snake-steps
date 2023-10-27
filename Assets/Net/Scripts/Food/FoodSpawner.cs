
using UnityEngine;
namespace Mirror.MyGame
{
    public class FoodSpawner : NetworkBehaviour
    {
        [SerializeField] GameObject foodPrefab;

        //public int spawnCounter;

        public override void OnStartServer()
        {
            //spawnCounter = 0;
            SpawnFood(gameObject);
            Food.ServerOnFoodEaten += SpawnFood;
        }

        public override void OnStopServer()
        {
            Food.ServerOnFoodEaten -= SpawnFood;
        }

        [Server]
        void SpawnFood(GameObject playerWhoAte)
        {
            var pos = new Vector3(
                Random.Range(-4f, 4f),
                Random.Range(-4f, 4f),
                0f);
            var foodInstance = Instantiate(foodPrefab, pos, foodPrefab.transform.rotation);
            NetworkServer.Spawn(foodInstance);

            // if (spawnCounter % 2 != 0)
            // {
            //     int randomAxis = Random.Range(0, 2);
            //     foodInstance.AddComponent<MovingApple2>();
            // }
            // spawnCounter++;
        }
    }
}

// public class MovingApple2 : MonoBehaviour
// {
//     private Vector3 startPos;
//     private Vector3 endPos;
//     private float moveSpeed = 2f;
//
//     private void Start()
//     {
//         int axis = Random.Range(0, 2);
//         int direction = 1;
//         var random = Random.Range(-1f, -14f);
//         
//         switch (axis)
//         {
//             case 0:
//                 startPos = new Vector3(-1f * direction, 1f, random);
//                 endPos = new Vector3(-14f * direction, 1f, random);
//                 break;
//             case 1:
//                 startPos = new Vector3(random, 1f, -1f * direction);
//                 endPos = new Vector3(random, 1f, -14f * direction);
//                 break;
//         }
//         
//         transform.position = startPos;
//         StartCoroutine(MoveApple());
//     }
//
//     private IEnumerator MoveApple()
//     {
//         while (true)
//         {
//             transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);
//             if (transform.position == endPos)
//             {
//                 (startPos, endPos) = (endPos, startPos);
//             }
//             yield return null;
//         }
//     }
// }