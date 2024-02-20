using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIMovement : AIBehaviour
{

    enum MovingState { MOVING, IDLE, FOLLOWING };
    private MovingState movingState;

    [SerializeField] private Animator animator;

    private GameObject followTarget;
    [SerializeField]
    [Tooltip("When the follow target deviates from its initial position by this amount, recalculate the path to the target.")]
    private float followTargetDifferenceThreshold;

    [Tooltip("The min distance from the target position. If the nav mesh path ends further than this distance, then the pathing fails.")]
    [SerializeField]
    private float pathFailureThreshold = .5f;

    /*
    [Tooltip("The max angle difference between the front face of this game object and the path target.")]
    [SerializeField]
    private float MaxRotationDeviation = 0.0f;
    */
    private NavMeshAgent agent;
    // Start is called before the first frame update
    protected override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        movingState = MovingState.IDLE;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(animator) UpdateAnimator();
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
            return Node.Status.FAILURE;
        }
        //return success when we reach the destination
        else if(distanceToTarget < pathFailureThreshold)
        {
            movingState = MovingState.IDLE;
            return Node.Status.SUCCESS;
        }

        return Node.Status.RUNNING;
    }

    public Node.Status FollowTarget(GameObject target)
    {
        followTarget = target;
        if (!followTarget) return Node.Status.FAILURE; //return failure if nothing to follow

        float distanceToTarget = Vector3.Distance(followTarget.transform.position, transform.position);

        if (movingState == MovingState.IDLE)
        {
            agent.SetDestination(target.transform.position);
            movingState = MovingState.FOLLOWING;
        }

        if (movingState == MovingState.FOLLOWING)
        {
            //cannot path to target, return failure
            if(Vector3.Distance(agent.pathEndPosition, followTarget.transform.position) > pathFailureThreshold)
            {
                movingState = MovingState.IDLE;
                return Node.Status.FAILURE;
            }


            if (distanceToTarget < pathFailureThreshold)
            {
                movingState = MovingState.IDLE;
                return Node.Status.SUCCESS;
            }

            //if follow target has moved, reset destination
            if (Vector3.Distance(agent.pathEndPosition, followTarget.transform.position) > followTargetDifferenceThreshold)
            {
                agent.SetDestination(followTarget.transform.position);
            }
        }
        //default return running
        return Node.Status.RUNNING;
    }


    public void UpdateAnimator()
    {
        float normalizedMoveSpeed = Mathf.InverseLerp(0, agent.speed, agent.velocity.magnitude);
        
        animator.SetFloat("MoveSpeed", normalizedMoveSpeed);
    }
}
