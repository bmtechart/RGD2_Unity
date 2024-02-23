using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        Leaf LookAtPlayer = new Leaf("Face Player", FacePlayer);
        Leaf AttackPlayer = new Leaf("Attack Player", Attack);
        patrolNode.AddChild(LookForPlayer);
        patrolNode.AddChild(MoveToPlayer);
        patrolNode.AddChild(LookAtPlayer);
        patrolNode.AddChild(AttackPlayer);
    }

    protected override void DrawDebug()
    {
        base.DrawDebug();
        CapsuleCollider cc = GetComponent<CapsuleCollider>();
        if (!cc) return;

        Vector3 lineStart = transform.position + cc.center;
        Vector3 lineEnd = lineStart + transform.forward * 3.0f;
        Handles.DrawLine(lineStart, lineEnd, 10.0f);
        Gizmos.DrawLine(lineStart, lineEnd);
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

    public Node.Status FacePlayer()
    {
        if (!aiMovement)
        {
            Debug.Log("No AI Movement component assigned to prefab!");
            return Node.Status.FAILURE;
        }

        if (target) return aiMovement.FaceTarget(target.gameObject);

        return Node.Status.FAILURE;
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
