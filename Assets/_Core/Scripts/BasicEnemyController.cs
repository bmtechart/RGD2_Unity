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
        //get references to ai behaviours on this game object
        aiVision = GetAIBehaviour<AIVision>("Vision");
        aiMovement = GetAIBehaviour<AIMovement>("Movement");
        aiAttack = GetAIBehaviour<AIAttack>("MeleeAttack");


        //assemble behaviour tree
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

    /// <summary>
    /// This function accesses the ai vision script to determine if the player is in range of this enemy.
    /// </summary>
    /// <returns>
    /// Returns a node state for the behaviour tree. 
    /// Success if a player is visible, failure if a player is not visible. 
    /// </returns>
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

    /// <summary>
    /// Accesses the ai movement script to rotate the enemy to face the player
    /// </summary>
    /// <returns>Returns success if facing the player, running if rotating, and failure if no player is available.</returns>
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


    /// <summary>
    /// Uses the ai movement script to set the destination of a navmesh agent. 
    /// For use with a behaviour tree. 
    /// </summary>
    /// <returns>Returns success if the player has been reached, 
    /// failure if the player cannot be reached, 
    /// and running if en route to player.</returns>
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

    /// <summary>
    /// Triggers the ai attack script.
    /// Runs whatever attack ability is assigned to the ai attack script.
    /// </summary>
    /// <returns>
    /// Success if the attack has a been completed.
    /// Running if the attack is currenlty being executed.
    /// Failure if no valid target to attack. 
    /// </returns>
    public Node.Status Attack()
    {
        return aiAttack.Attack(aiVision.Target);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        //continually run the behaviour tree. 
        Tree.Process();
    }
}
