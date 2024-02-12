using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : AIBehaviour
{
    public float AttackRange;
    public float Damage;
    public Transform target;


    public enum AttackState { IDLE, ATTACKING, COMPLETE };
    public AttackState attackState;

    public bool isInRange()
    {
        if (Vector3.Distance(target.position, transform.position) > AttackRange) return false;

        return true;
    }

    /// <summary>
    /// Behaviour Tree Attack, not working yet.
    /// Change to Public once it is working
    /// </summary>
    /// <returns></returns>
    private Node.Status Attack()
    {
        switch(attackState)
        {
            //if attack has played to completion
            case AttackState.COMPLETE:
                attackState = AttackState.IDLE; //reset attack state
                return Node.Status.SUCCESS;
            //if attack has successfully started and is playing
            case AttackState.ATTACKING:
                return Node.Status.RUNNING;

            case AttackState.IDLE:
               
                //if we lose target, return failure
                if (!target) return Node.Status.FAILURE;

                //when starting an attack, if out of range, return failure
                if (!isInRange()) return Node.Status.FAILURE;

                //update attack state and set node to running
                attackState = AttackState.ATTACKING;
                return Node.Status.RUNNING;

            default: 
                return Node.Status.FAILURE;

        }
    }
}
