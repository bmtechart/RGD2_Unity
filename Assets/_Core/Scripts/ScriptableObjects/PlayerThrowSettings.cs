using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerThrowSettings", menuName = "Scriptable Objects/PlayerThrowSettings", order = 0)]
public class PlayerThrowSettings : ScriptableObject
{
    [Header("Grab")]
    [SerializeField, Range(0.0f, 10.0f), Tooltip("Distance (m) of the sphere cast used for determining if an object is grabbable.")] public float GrabDistance = 2.0f;
    [SerializeField, Range(0.001f, 0.5f), Tooltip("Radius (m) of the sphere cast used for determining if an object is grabbable.")] public float GrabSphereCastRadius = 0.1f;

    [Header("Throw")]
    [SerializeField, Range(0.0f, 45.0f), Tooltip("Angle (°) that objects are thown at (in relation to the camera angle).")] public float ThrowAngle = 15.0f;
    [SerializeField, Range(0.0f, 100.0f), Tooltip("Velocity (m/s) that objects travel at when thrown.")] public float ThrowVelocity = 25.0f;

    [Header("Held Object Position")]
    [SerializeField, Tooltip("The position relative to the camera at which objects are held.")] public Vector3 HeldObjectPosition = new Vector3(0.5f, -0.5f, 1.0f);

    [Header("Aim Assist")]
    [SerializeField] public bool UseAimAssist = true;
    [SerializeField, Range(0.0f, 90.0f), Tooltip("Maximum horizontal arc (°) for detecting targets to aim at.")] public float FullAccuracyBonus = 15.0f;
    internal float AccuracyBonus; //accessable if UI needs it
    [SerializeField, Range(0.001f, 60.0f), Tooltip("Time (s) needed to hold the throw button to achive a full accuracy bonus.")] public float TimeToFullAccuracy = 1.0f;
    [SerializeField, Range(0.001f, 60.0f), Tooltip("Time (s) that it takes to fully decay the accuracy bonus after holding the throw button to full accuracy.")] public float TimeFromFullAccuracy = 1.0f;
}
