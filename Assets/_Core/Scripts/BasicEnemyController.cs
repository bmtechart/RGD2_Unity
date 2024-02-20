using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : AIController
{
    private Transform target;

    AIVision aiVision;
    AIMovement aiMovement;
    AIAttack aiAttack;

    // Start is called before the first frame update
    public override void Start()
    {

        aiVision = GetAIBehaviour<AIVision>("Vision");
        aiMovement = GetAIBehaviour<AIMovement>("Movement");
        aiAttack = GetAIBehaviour<AIAttack>("MeleeAttack");

        Sequence patrolNode = new Sequence("patrol");

        Tree.AddChild(patrolNode);

        Leaf LookForPlayer = new Leaf("LookForPlayer", LookForTarget);
        Leaf MoveToPlayer = new Leaf("Move to Player", FollowPlayer);
        Leaf AttackPlayer = new Leaf("Attack Player", Attack);
        patrolNode.AddChild(LookForPlayer);
        patrolNode.AddChild(MoveToPlayer);
        patrolNode.AddChild(AttackPlayer);
    }

    public Node.Status LookForTarget() 
    {
        
        if(!aiVision)
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

    public Node.Status FollowPlayer()
    {
        if(!aiMovement)
        {
            Debug.Log("No AI Movement component assigned to prefab!");
            return Node.Status.FAILURE;
        }

        if(target) return aiMovement.FollowTarget(target.gameObject);

        return Node.Status.FAILURE;
    }


    public Node.Status Attack()
    {
        return aiAttack.Attack(aiVision.Target);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        //process behaviour tree
        Tree.Process();
    }
}
