using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIMovement : AIBehaviour
{

    enum MovingState { MOVING, IDLE };
    private MovingState movingState;

    [Tooltip("The min distance from the target position. If the nav mesh path ends further than this distance, then the pathing fails.")]
    [SerializeField]
    private float pathFailureThreshold = 2.0f;

    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        movingState = MovingState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //movement function to use in our behaviour tree
    public Node.Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, transform.position);

        //only set destination if we aren't already moving
        if(movingState == MovingState.IDLE)
        {
            agent.SetDestination(destination);
            movingState = MovingState.MOVING;
        }
        //if we cannot path to our destination
        else if(Vector3.Distance(agent.pathEndPosition, destination) >= pathFailureThreshold)
        {
            movingState = MovingState.IDLE;
            Debug.Log("Could not path find to destination!");
            return Node.Status.FAILURE;
        }
        //return success when we reach the destination
        else if(distanceToTarget < pathFailureThreshold)
        {
            Debug.Log("successfully reached destination!");
            movingState = MovingState.IDLE;
            return Node.Status.SUCCESS;
        }

        return Node.Status.RUNNING;

    }
}
