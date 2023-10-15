using System.Collections;
using System.Collections.Generic;

using UnityEngine;
// using UnityEngine.UI;
public class Game : MonoBehaviour
{
     public bool moveThroughWalls = false;
    private GameObject[] m_walls;
    private Snake[] m_snakes;
    private void Awake()
    {
        m_snakes = FindObjectsOfType<Snake>();
    }
     private void Start()
    {


        m_walls = GameObject.FindGameObjectsWithTag("Wall");
        SetWallColor();

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
            sp.material.color = moveThroughWalls ? Color.green : Color.gray;
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
