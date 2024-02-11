using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : AIBehaviour
{
    public float AttackRange;
    public float Damage;

    public bool isInRange()
    {

    }

    public Node.Status AttackPlayer()
    {
        return Node.Status.SUCCESS;
    }
}
