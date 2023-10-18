
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] GameObject foodPrefab;

    [SerializeField] int total = 2;
    Game m_game;

    void Start()
    {
        m_game = FindAnyObjectByType<Game>();
        for (int i = 0; i < total; i++)
            SpawnFood();
    }
    public void SpawnFood()
    {
        var pos = m_game.GetFreePosition();
         Instantiate(foodPrefab, pos, foodPrefab.transform.rotation);

    }
}
