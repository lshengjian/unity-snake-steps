using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Food : MonoBehaviour
{
    public Collider2D gridArea;


    private Game m_game;


    private void Start()
    {
        m_game = FindAnyObjectByType<Game>();
        RandomizePosition();
    }

    Vector2 GetFreePosition()
    {
        Bounds bounds = gridArea.bounds;
        // Pick a random position inside the bounds
        // Round the values to ensure it aligns with the grid
        int x = Mathf.RoundToInt(Random.Range(bounds.min.x + 1, bounds.max.x - 1));
        int y = Mathf.RoundToInt(Random.Range(bounds.min.y + 1, bounds.max.y - 1));

        // Prevent the food from spawning on the snake
        while (m_game.IsOccupies(x, y))
        {
            x++;

            if (x > bounds.max.x)
            {
                x = Mathf.RoundToInt(bounds.min.x);
                y++;

                if (y > bounds.max.y)
                {
                    y = Mathf.RoundToInt(bounds.min.y);
                }
            }
        }

        return new Vector2(x, y);

    }

    public void RandomizePosition()
    {



        // Debug.Log("Food:" + pos);
        transform.position = GetFreePosition();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        RandomizePosition();
    }

}
