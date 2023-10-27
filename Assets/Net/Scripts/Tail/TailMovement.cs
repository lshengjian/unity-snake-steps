using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Mirror.MyGame
{
public class TailMovement : NetworkBehaviour
{
  //  [SerializeField] NavMeshAgent agent;
    [SerializeField] TailNetwork tail;

    void Start()
    {
        transform.position = tail.Target.transform.position;
        //transform.LookAt(this.transform);
    }

    void Update()
    {
        //agent.speed = tail.Owner.Speed;
       // agent.SetDestination(tail.Target.transform.position);
       transform.position=Vector3.MoveTowards(transform.position,tail.Target.transform.position,Time.deltaTime);
    }
}
}