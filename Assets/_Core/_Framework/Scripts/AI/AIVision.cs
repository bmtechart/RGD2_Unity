using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FieldOfView))]
public class AIVision : AIBehaviour
{
    private FieldOfView fieldOfView;

    [Tooltip("Toggle debug display.")]
    [SerializeField] bool drawDebug;
    
    private Transform _target;

    public Transform Target { get { return _target; } }

    protected override void Start()
    {
        fieldOfView = GetComponent<FieldOfView>();
        if (!fieldOfView) Debug.Log("Warning! Vision component has no field of view component!");
    }

    public Node.Status LookForTarget()
    {
        List <Transform> visibleTargets = fieldOfView.FindVisibleTargets();
        if (visibleTargets.Count > 0) _target = visibleTargets[0];

        return CanSeeTarget();
    }

    public Node.Status CanSeeTarget()
    {
        if (!Target) return Node.Status.FAILURE;
        return Node.Status.SUCCESS;
    }
}
