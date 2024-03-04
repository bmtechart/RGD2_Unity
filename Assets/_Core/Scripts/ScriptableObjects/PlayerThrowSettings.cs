using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerThrowSettings", menuName = "Scriptable Objects/PlayerThrowSettings", order = 0)]
public class PlayerThrowSettings : ScriptableObject
{
    [Header("Grab")]
    [SerializeField, Range(0.0f, 10.0f), Tooltip("Distance (m) of the sphere cast used for determining if an object is grabbable.")] public float GrabDistance = 3.0f;
    [SerializeField, Range(0.001f, 0.5f), Tooltip("Radius (m) of the sphere cast used for determining if an object is grabbable.")] public float GrabSphereCastRadius = 0.15f;

    [Header("Throw")]
    [SerializeField, Range(0.0f, 45.0f), Tooltip("Angle (°) that objects are thown at (in relation to the camera angle).")] public float ThrowAngle = 15.0f;
    [SerializeField, Range(0.0f, 100.0f), Tooltip("Velocity (m/s) that objects travel at when thrown.")] public float ThrowVelocity = 25.0f;

    [Header("Animation")]
    [SerializeField, Range(0.001f, 1.0f), Tooltip("Time (s) for the grabbing hand to return to default position after grabbing.")] public float GrabAnimationTime = 0.25f;
    [SerializeField, Range(0.0f, 100.0f), Tooltip("Speed (hz) for the throwing hand extention when throwing.")] public float ThrowAnimationSpeed = 10.0f;
    [SerializeField, Range(0.001f, 1.0f), Tooltip("Time (s) for the throwing hand to extend when throwing.")] public float ThrowAnimationTime1 = 0.5f;
    [SerializeField, Range(0.001f, 1.0f), Tooltip("Time (s) for the throwing hand to return to default position after throwing.")] public float ThrowAnimationTime2 = 0.5f;

    [Header("Hand Positions")]
    [SerializeField, Tooltip("The position relative to the camera at which objects are held.")] public Vector3 HeldObjectPosition = new Vector3(0.5f, -0.375f, 1.25f);
    [SerializeField, Tooltip("The position relative to the camera at which objects are held while the throw button is being held.")] public Vector3 ChargedObjectPosition = new Vector3(0.75f, 0.5f, 0.5f);
    [SerializeField, Tooltip("The position relative to the camera at which the off hand is while the throw button is being held.")] public Vector3 ChargedOffHandPosition = new Vector3(-0.1f, -0.125f, 1.325f);
    [SerializeField, Tooltip("The position relative to the camera at which the throwing hand is after a throw.")] public Vector3 ThrownHandPosition = new Vector3(0.1f, -0.125f, 1.325f);
    [SerializeField, Tooltip("The position relative to the camera at which the off hand is after a throw.")] public Vector3 ThrownOffHandPosition = new Vector3(-0.75f, -0.625f, 0.0f);
    internal const float ArmLength = 1.12f;

    [Header("Aim Assist")]
    [SerializeField] public bool UseAimAssist = true;
    [SerializeField, Range(0.0f, 90.0f), Tooltip("Maximum horizontal arc (°) for detecting targets to aim at.")] public float FullAccuracyBonus = 15.0f;
    internal float AccuracyBonus; //accessable if UI needs it
    [SerializeField, Range(0.001f, 60.0f), Tooltip("Time (s) needed to hold the throw button to achive a full accuracy bonus.")] public float TimeToFullAccuracy = 0.5f;
    [SerializeField, Range(0.001f, 60.0f), Tooltip("Time (s) that it takes to fully decay the accuracy bonus after holding the throw button to full accuracy.")] public float TimeFromFullAccuracy = 5.0f;
}
