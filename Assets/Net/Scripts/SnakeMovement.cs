using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
namespace Mirror.MyGame
{
public class SnakeMovement : NetworkBehaviour
{
    [SerializeField] float rotationSpeed = 180f, speedChange = 0.5f;

    [SerializeField]
    [SyncVar]
    float speed = 3f;
    public float Speed
    {
        get { return speed; }
        private set { speed = value; }
    }

    public override void OnStartServer()
    {
        Food.ServerOnFoodEaten += ServerHandleFoodEaten;
    }

    public override void OnStopServer()
    {
        Food.ServerOnFoodEaten -= ServerHandleFoodEaten;
    }

    void ServerHandleFoodEaten(GameObject playerWhoAte)
    {
        if (gameObject == playerWhoAte)
            Speed += speedChange;
    }

    [ClientCallback]
    void Update()
    {
       
        
        float steerDirection = 0f;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.x < Screen.width / 2f)
            {
                steerDirection = -1f;
            }
            else
            {
                steerDirection = 1f;
            }
        }
        else
        {
            steerDirection = Input.GetAxis("Horizontal");
        }
         transform.Translate(Vector3.right *steerDirection* (Speed * Time.deltaTime));
       // transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime * steerDirection));
    }
}
}

// snekPos.position += snekPos.forward * (moveSpeed * Time.deltaTime);
// snekPos.Rotate(Vector3.up * (steerDirection * steerSpeed * Time.deltaTime));
