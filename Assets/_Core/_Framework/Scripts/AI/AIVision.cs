using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FieldOfView))]
public class AIVision : AIBehaviour
{
    private FieldOfView fieldOfView;
    [SerializeField] bool drawDebug;
    public Transform target;

    private void Start()
    {
        fieldOfView = GetComponent<FieldOfView>();
        if (!fieldOfView) Debug.Log("Warning! Vision component has no field of view component!");
    }

    public Node.Status LookForTarget()
    {
        fieldOfView.FindVisibleTargets();
        target = fieldOfView.visibleTargets[0]; 

        if(!drawDebug) return CanSeeTarget();

        return CanSeeTarget();
    }

    public Node.Status CanSeeTarget()
    {
        if (fieldOfView.visibleTargets.Count <= 0) return Node.Status.FAILURE;
        return Node.Status.SUCCESS;
    }
}
