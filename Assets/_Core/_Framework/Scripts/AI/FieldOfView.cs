using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class FieldOfView : MonoBehaviour
{
    #region Parameters
    [Tooltip("The transform that is the origin of the point of view. This could be the unit's eyes or a security camera. If not set, this will default to the game object transform.")]
    public Transform viewOrigin;

    [Tooltip("Max distance where a visible target will be seen")]
    public float viewRadius;

    [Tooltip("Angle in degrees from the forward vector of the view origin that that targets will be detected.")]
    [Range(0, 360)]
    public float viewAngle;

    [Tooltip("Layers that contain valid view targets")]
    public LayerMask targetMask;

    [Tooltip("Layers containing objects that will block line of sight.")]
    public LayerMask obstacleMask;

    #endregion

    #region Variables
    private List<Transform> visibleTargets = new List<Transform>();

    public List<Transform> VisibleTargets { get { return visibleTargets; } }
    #endregion

    #region Functions
    public List<Transform> FindVisibleTargets()
    {
        Transform lookFrom = transform;
        if (viewOrigin) lookFrom = viewOrigin;

        visibleTargets.Clear();
        //get all colliders in radius
        Collider[] targetsInViewRadius = Physics.OverlapSphere(lookFrom.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - lookFrom.position).normalized;

            //if target is in view angle
            if (Vector3.Angle(lookFrom.forward, dirToTarget) < viewAngle/2)
            {
                float distanceToTarget = Vector3.Distance(lookFrom.position, target.position);

                if(!Physics.Raycast(lookFrom.position,dirToTarget, distanceToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }

        return visibleTargets;
    }

    public Vector3 DirFromAngle(float angle, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angle += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    #endregion
    public void Update()
    {

    }
}
