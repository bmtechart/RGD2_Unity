using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIAttack : AIBehaviour
{

    [SerializeField] private bool showDebug;


    public float AttackRange;
    public float Damage;
    public Transform target;

    public AbilityBase attack;

    public enum AttackState { IDLE, ATTACKING, COMPLETE };
    private AttackState attackState;

    public bool isInRange()
    {
        if (Vector3.Distance(target.position, transform.position) > AttackRange) return false;

        return true;
    }

    public Node.Status Attack(Transform attackTarget)
    {
        if (!attack) return Node.Status.FAILURE;
        if (!target) target = attackTarget;
        switch(attackState)
        {
            //if attack has played to completion
            case AttackState.COMPLETE:

                //if still in range, keep attacking
                if(isInRange())
                {
                    //update attack state and set node to running
                    attackState = AttackState.ATTACKING;
                    attack.TriggerAbility();
                    return Node.Status.RUNNING;
                }

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
                attack.TriggerAbility();
                return Node.Status.RUNNING;

            default: 
                return Node.Status.FAILURE;

        }
    }

    protected override void Start()
    {
        base.Start();

        if (attack) attack.abilityComplete += AttackComplete;
    }

    public void AttackComplete()
    {
        attackState = AttackState.COMPLETE;
    }


}
