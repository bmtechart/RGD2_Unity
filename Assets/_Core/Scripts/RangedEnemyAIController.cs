using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAIController : AIController
{
    private Transform target;
    private AIVision aiVision;
    private AIMovement aiMovement;
    private AIAttack aiAttack;
    public override void Start()
    {
        base.Start();

        aiVision = GetAIBehaviour<AIVision>("Vision");
        aiMovement = GetAIBehaviour<AIMovement>("Movement");
        aiAttack = GetAIBehaviour<AIAttack>("RangedAttack");

        Sequence attackSequence = new Sequence("Attack Sequence");

        Tree.AddChild(attackSequence);

        Leaf lookForPlayer = new Leaf("Look for Player", CanSeePlayer);
        Leaf moveInRange = new Leaf("Move In Range", MoveInRangeOfPlayer);
        Leaf rangedAttack = new Leaf("Ranged Attack", AttackPlayer);

        attackSequence.AddChild(lookForPlayer);
        attackSequence.AddChild(moveInRange);
        attackSequence.AddChild(rangedAttack);
    }

    public override void Update()
    {
        base.Update();
        Tree.Process();
    }

    public Node.Status CanSeePlayer()
    {
        if (!aiVision)
        {
            Debug.Log("No AI Vision component assigned to prefab!");
            return Node.Status.FAILURE;
        }

        Node.Status returnStatus = aiVision.LookForTarget();

        if (returnStatus == Node.Status.SUCCESS)
        {
            target = aiVision.Target;

            Animator.SetLayerWeight(Animator.GetLayerIndex("Aggro"), 1.0f);
        }

        return aiVision.LookForTarget();
    }

    public Node.Status MoveInRangeOfPlayer()
    {
        if(!target) return Node.Status.FAILURE;
        if (!aiMovement) return Node.Status.FAILURE;
        return aiMovement.FollowTarget(target.gameObject);
    }

    public Node.Status AttackPlayer()
    {
        if (!target) return Node.Status.FAILURE;
        RangedAttackBehaviour rangedAttack = GetComponent<RangedAttackBehaviour>();
        if (!rangedAttack) return Node.Status.FAILURE;
        rangedAttack.Target = target;
        return aiAttack.Attack(target);
    }
}
