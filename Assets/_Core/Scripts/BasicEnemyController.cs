using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : AIController
{

    public GameObject PatrolPointA;
    public GameObject PatrolPointB;

    // Start is called before the first frame update
    public override void Start()
    {
        if(!aiMovement) aiMovement = GetComponent<AIMovement>();
        if(!aiVision) aiVision = GetComponentInChildren<AIVision>(); 

        Sequence patrolNode = new Sequence("patrol");

        Leaf LookForPlayer = new Leaf("LookForPlayer", aiVision.LookForTarget);
        Leaf MoveToPlayer = new Leaf("Move to Player", FollowPlayer);
        Leaf AttackPlayer = new Leaf("Attack Player", AttackPlayer);
        patrolNode.AddChild(LookForPlayer);
        patrolNode.AddChild(MoveToPlayer);
        patrolNode.AddChild(AttackPlayer);


        Tree.AddChild(patrolNode);
    }

    public Node.Status FollowPlayer()
    {
        return aiMovement.FollowTarget(aiVision.target.gameObject);
    }

    public Node.Status PatrolA()
    {
        return aiMovement.GoToLocation(PatrolPointA.transform.position);
    }

    public Node.Status PatrolB()
    {
        return aiMovement.GoToLocation(PatrolPointB.transform.position);
    }

    public Node.Status AttackPlayer()
    {
        return Node.Status.SUCCESS;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
