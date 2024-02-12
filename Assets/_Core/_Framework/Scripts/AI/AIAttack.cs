using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : AIBehaviour
{
    public float AttackRange;
    public float Damage;
    public Transform target;

    public enum AttackState { IDLE, ATTACKING };
    public AttackState attackState;

    public bool isInRange()
    {
        if (Vector3.Distance(target.position, transform.position) > AttackRange)
        {
            return false;
        }

        return true;
    }

    public Node.Status AttackPlayer()
    {
        //start attack
            //still in range?
            //trigger attack animation
        //if target is still in range and within attack arc
            //damage target
        if(isInRange()) return Node.Status.SUCCESS;
        return Node.Status.FAILURE;

    }
}
