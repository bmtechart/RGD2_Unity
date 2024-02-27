using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(FieldOfView))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;

        Transform lookFrom = fov.transform;

        if(fov.viewOrigin) lookFrom = fov.viewOrigin;

        Handles.DrawWireArc(lookFrom.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);

        Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle/2, false);
        Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);
        Handles.DrawLine(lookFrom.position, lookFrom.position + viewAngleA * fov.viewRadius);
        Handles.DrawLine(lookFrom.position, lookFrom.position + viewAngleB * fov.viewRadius);

        Handles.color = Color.red;
        foreach(Transform t in fov.VisibleTargets)
        {
            Handles.DrawLine(lookFrom.position, t.position);
        }

    }
}
