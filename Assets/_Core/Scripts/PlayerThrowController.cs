using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public enum HandState
{
    Empty,
    Holding,
    Throwing,
    LiftingLight, //remove when removing FoodInteractionController.cs
    LiftingHeavy, //remove when removing FoodInteractionController.cs
    HoldingLight, //remove when removing FoodInteractionController.cs
    HoldingHeavy //remove when removing FoodInteractionController.cs
}

public class PlayerThrowController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _handInputAction;

    private HandState _handState;

    internal FoodObject HeldObject;

    [Header("Grab")]
    [SerializeField, Range(0.0f, 10.0f), Tooltip("Distance (m) of the sphere cast used for determining if an object is grabbable.")] public float GrabDistance = 2.0f;
    [SerializeField, Range(0.001f, 0.5f), Tooltip("Radius (m) of the sphere cast used for determining if an object is grabbable.")] public float GrabSphereCastRadius = 0.1f;

    [Header("Throw")]
    [SerializeField, Range(0.0f, 45.0f), Tooltip("Angle (°) that objects are thown at (in relation to the camera angle).")] public float ThrowAngle = 15.0f;
    [SerializeField, Range(0.0f, 100.0f), Tooltip("Velocity (m/s) that objects travel at when thrown.")] public float ThrowVelocity = 20.0f;

    [Header("Aim Assist")]
    [SerializeField] public bool UseAimAssist = true;
    [SerializeField, Range(0.0f, 90.0f), Tooltip("Maximum horizontal arc (°) for detecting targets to aim at.")] public float FullAccuracyBonus = 15.0f;
    internal float AccuracyBonus;
    private float _maxTargetDistance;
    [SerializeField, Range(0.001f, 60.0f), Tooltip("Time (s) needed to hold the throw button to achive a full accuracy bonus.")] public float TimeToFullAccuracy = 1.0f;
    [SerializeField, Range(0.001f, 60.0f), Tooltip("Time (s) that it takes to fully decay the accuracy bonus after holding the throw button to full accuracy.")] public float TimeFromFullAccuracy = 1.0f;
    private float _accuracyBonusTimer;

    [Header("Held Object Position")]
    [SerializeField, Tooltip("The position relative to the camera at which objects are held.")] public Vector3 HeldObjectPosition = new Vector3(0.5f, -0.5f, 1.0f);

    private void Start()
    {
        //get playr input
        _playerInput = GetComponent<PlayerInput>();

        //enable food interaction input
        _playerInput.actions.FindActionMap("HandInteraction").Enable();

        //get button actions for held input
        _handInputAction = _playerInput.actions.FindAction("HandAction");

        //init hand states
        _handState = HandState.Empty;

        //init other variables
        AccuracyBonus = 0.0f;
        _accuracyBonusTimer = 0.0f;
        _maxTargetDistance = ThrowVelocity * ThrowVelocity / Mathf.Abs(Physics.gravity.y);
    }

    private void FixedUpdate()
    {
        if (_handState == HandState.Throwing)
        {
            //charge throw
            ChargeThrow();
        }
    }

    private void ChargeThrow()
    {
        bool isCharging = false;

        //check input for still charging throw
        foreach (ButtonControl buttonControl in _handInputAction.controls)
        {
            if (buttonControl.isPressed)
            {
                isCharging = true;
            }
        }

        if (isCharging)
        {
            //increment timer
            _accuracyBonusTimer += Time.fixedDeltaTime;

            //calculate accuracy bonus
            if (_accuracyBonusTimer < TimeToFullAccuracy)
            {
                AccuracyBonus = _accuracyBonusTimer / TimeToFullAccuracy;
            }
            else
            {
                AccuracyBonus = 1.0f - (_accuracyBonusTimer - TimeToFullAccuracy) / TimeFromFullAccuracy;
            }

            //clamp accuracy bonus
            if (AccuracyBonus < 0.0f)
            {
                AccuracyBonus = 0.0f;
            }
        }
        else
        {
            //throw if no longer charging
            ThrowObject();
        }
    }

    private void GrabObject(FoodObject food)
    {
        Debug.Log("object grabbed");
        //assign
        HeldObject = food;

        //disable physics
        food.HoldObject();

        //position
        food.transform.SetParent(Camera.main.transform);
        food.transform.localPosition = HeldObjectPosition;

        //change hand state
        _handState = HandState.Holding;
    }

    private void ThrowObject()
    {
        if (HeldObject != null)
        {
            //unparent
            HeldObject.transform.SetParent(null);

            //enable physics
            HeldObject.DropObject();

            //get target if using aim assist
            AimAssistTarget target = null;
            if (UseAimAssist)
            {
                target = GetAimAssistTarget();
            }

            //apply velocity
            if (target != null)
            {
                Debug.Log("object thrown with aim assist");
                //determine modified angles for aim assist
                float assistedAngleX = 0.0f;
                float assistedAngleY = 0.0f;

                //apply velocity at modified angle
                HeldObject.GetComponent<Rigidbody>().velocity = ThrowVelocity * (Quaternion.AngleAxis(-assistedAngleY, Vector3.up) * (Quaternion.AngleAxis(-assistedAngleX, Camera.main.transform.right) * Camera.main.transform.forward));
            }
            else
            {
                Debug.Log("object thrown without aim assist");
                //apply velocity at default angle
                HeldObject.GetComponent<Rigidbody>().velocity = ThrowVelocity * (Quaternion.AngleAxis(-ThrowAngle, Camera.main.transform.right) * Camera.main.transform.forward);
            }

            //reset accuracy bonus
            AccuracyBonus = 0.0f;

            //change hand state
            _handState = HandState.Empty;

            //remove reference
            HeldObject = null;
        }
    }

    private AimAssistTarget GetAimAssistTarget()
    {
        List<AimAssistTarget> aimAssistTargets = new List<AimAssistTarget>(FindObjectsByType<AimAssistTarget>(FindObjectsSortMode.InstanceID));

        //remove targets out of bonus arc
        for (int i = aimAssistTargets.Count - 1; i >= 0; i--)
        {
            Vector3 targetDirection = aimAssistTargets[i].transform.position - Camera.main.transform.position;
            if (Vector3.Angle(new Vector3(targetDirection.x, 0.0f, targetDirection.z), new Vector3(Camera.main.transform.forward.x, 0.0f, Camera.main.transform.forward.z)) > FullAccuracyBonus / 2.0f * AccuracyBonus)
            {
                aimAssistTargets.Remove(aimAssistTargets[i]);
            }
        }

        //remove targets out of max range
        for (int i = aimAssistTargets.Count - 1; i >= 0; i--)
        {
            if (Vector3.Distance(new Vector3(aimAssistTargets[i].transform.position.x, 0.0f, aimAssistTargets[i].transform.position.z), new Vector3(Camera.main.transform.position.x, 0.0f, Camera.main.transform.position.z)) > _maxTargetDistance)
            {
                aimAssistTargets.Remove(aimAssistTargets[i]);
            }
        }

        //determine nearest target
        AimAssistTarget selectedTarget = null;
        float selectedTargetDistance = _maxTargetDistance;
        foreach (AimAssistTarget target in aimAssistTargets)
        {
            float distance = Vector3.Distance(new Vector3(target.transform.position.x, 0.0f, target.transform.position.z), new Vector3(Camera.main.transform.position.x, 0.0f, Camera.main.transform.position.z));
            if (distance < selectedTargetDistance)
            {
                selectedTarget = target;
                selectedTargetDistance = distance;
            }
        }

        //return selected target
        return selectedTarget;
    }

    public void EnableInteraction()
    {
        _playerInput.actions.FindActionMap("HandInteraction").Enable();
    }

    public void DisableInteraction()
    {
        _playerInput.actions.FindActionMap("HandInteraction").Disable();
    }

    //grab or throw object with input event
    private void OnHandAction()
    {
        if (_handState == HandState.Empty)
        {
            //sphere cast to try grab
            if (Physics.SphereCast(Camera.main.transform.position, GrabSphereCastRadius, Camera.main.transform.forward, out RaycastHit hit, GrabDistance))
            {
                //if throwable object
                FoodObject food = hit.collider.GetComponent<FoodObject>();
                if (food != null)
                {
                    //grab object
                    GrabObject(food);
                }
            }
        }
        else if (_handState == HandState.Holding)
        {
            //start throwing object
            _handState = HandState.Throwing;
        }
    }
}