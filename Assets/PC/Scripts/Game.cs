using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
// using UnityEngine.UI;
public class Game : MonoBehaviour
{
    public Collider2D gridArea;


    public bool moveThroughWalls = false;
    private GameObject[] m_walls;
    private Snake[] m_snakes;
    private BoundLimit m_limit;
    private void Awake()
    {

        m_limit = gridArea.GetComponent<BoundLimit>();
        m_limit.OnOutBound += OnTouchWall;
    }
    void Destroy()
    {
        m_limit.OnOutBound -= OnTouchWall;
       

    }
    private void Start()
    {
        m_walls = GameObject.FindGameObjectsWithTag("Wall");
        SetWallColor();
        m_snakes = FindObjectsOfType<Snake>();
      

    }
    public Vector2 GetFreePosition()
    {
        Bounds bounds = gridArea.bounds;
        // Pick a random position inside the bounds
        // Round the values to ensure it aligns with the grid
        int x = Mathf.RoundToInt(Random.Range(bounds.min.x + 1, bounds.max.x - 1));
        int y = Mathf.RoundToInt(Random.Range(bounds.min.y + 1, bounds.max.y - 1));

        // Prevent the food from spawning on the snake
        while (IsOccupies(x, y))
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

    public void OnTouchWall(Transform snake)
    {
        // Debug.Log(snake.name + " hit wall");
        Snake s = snake.GetComponent<Snake>();
        KeyInput km = snake.GetComponent<KeyInput>();
        if (moveThroughWalls)
        {
            s.controller.Traverse(snake.localPosition, km.direction);
        }
        else
        {
            s.controller.Reset();
        }
    }
   

    public bool IsOccupies(int x, int y)
    {
        foreach (var s in m_snakes)
            if (s.IsOccupies(x, y))
                return true;
        return false;
    }

    void SetWallColor()
    {
        foreach (GameObject wall in m_walls)
        {
            SpriteRenderer sp = wall.GetComponent<SpriteRenderer>();
            sp.color = moveThroughWalls ? Color.green : Color.gray;
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            moveThroughWalls = !moveThroughWalls;
            SetWallColor();
        }
    }
}
