using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : AIController
{
    // Start is called before the first frame update
    public override void Start()
    {
        if(!aiMovement) aiMovement = GetComponent<AIMovement>();
        if(!aiVision) aiVision = GetComponentInChildren<AIVision>(); 

        Sequence patrolNode = new Sequence("patrol");

        Tree.AddChild(patrolNode);

        Leaf LookForPlayer = new Leaf("LookForPlayer", aiVision.LookForTarget);
        Leaf MoveToPlayer = new Leaf("Move to Player", FollowPlayer);
        Leaf AttackPlayer = new Leaf("Attack Player", Attack);
        patrolNode.AddChild(LookForPlayer);
        patrolNode.AddChild(MoveToPlayer);
        patrolNode.AddChild(AttackPlayer);



    }

    public Node.Status FollowPlayer()
    {
        return aiMovement.FollowTarget(aiVision.target.gameObject);
    }


    public Node.Status Attack()
    {
        return Node.Status.SUCCESS;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
