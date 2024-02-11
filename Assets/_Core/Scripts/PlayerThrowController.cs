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

    internal ThrowableObject HeldObject;

    [SerializeField] public PlayerThrowSettings playerThrowSettings;

    private float _maxTargetDistance;
    private float _accuracyBonusTimer;

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
        playerThrowSettings.AccuracyBonus = 0.0f;
        _accuracyBonusTimer = 0.0f;
        _maxTargetDistance = playerThrowSettings.ThrowVelocity * playerThrowSettings.ThrowVelocity / Mathf.Abs(Physics.gravity.y);
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
            if (_accuracyBonusTimer < playerThrowSettings.TimeToFullAccuracy)
            {
                playerThrowSettings.AccuracyBonus = _accuracyBonusTimer / playerThrowSettings.TimeToFullAccuracy;
            }
            else
            {
                playerThrowSettings.AccuracyBonus = 1.0f - (_accuracyBonusTimer - playerThrowSettings.TimeToFullAccuracy) / playerThrowSettings.TimeFromFullAccuracy;
            }

            //clamp accuracy bonus
            if (playerThrowSettings.AccuracyBonus < 0.0f)
            {
                playerThrowSettings.AccuracyBonus = 0.0f;
            }
        }
        else
        {
            //throw if no longer charging
            ThrowObject();

            //reset timer
            _accuracyBonusTimer = 0.0f;
        }
    }

    private void GrabObject(ThrowableObject throwableObject)
    {
        Debug.Log("object grabbed");
        //assign
        HeldObject = throwableObject;

        //disable physics
        throwableObject.HoldObject();

        //position
        throwableObject.transform.SetParent(Camera.main.transform);
        throwableObject.transform.localPosition = playerThrowSettings.HeldObjectPosition;

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
            if (playerThrowSettings.UseAimAssist)
            {
                target = GetAimAssistTarget();
            }

            //apply velocity
            if (target != null)
            {
                Debug.Log("object thrown with aim assist");

                //determine modified angles for aim assist
                float assistedAngleX = Mathf.Rad2Deg * -(Mathf.Asin(Vector3.Distance(new Vector3(target.transform.position.x, 0.0f, target.transform.position.z), new Vector3(HeldObject.transform.position.x, 0.0f, HeldObject.transform.position.z)) * Physics.gravity.y / playerThrowSettings.ThrowVelocity / playerThrowSettings.ThrowVelocity) / 2.0f);
                float assistedAngleY = Vector3.SignedAngle(new Vector3(target.transform.position.x, 0.0f, target.transform.position.z) - new Vector3(HeldObject.transform.position.x, 0.0f, HeldObject.transform.position.z), new Vector3(Camera.main.transform.forward.x, 0.0f, Camera.main.transform.forward.z), Vector3.up);
                
                //find camera pitch
                float cameraPitch = Vector3.SignedAngle(new Vector3(Camera.main.transform.forward.x, 0.0f, Camera.main.transform.forward.z), Camera.main.transform.forward, Camera.main.transform.right);

                //apply velocity at modified angle
                HeldObject.GetComponent<Rigidbody>().velocity = playerThrowSettings.ThrowVelocity * (Quaternion.AngleAxis(-assistedAngleY, Vector3.up) * (Quaternion.AngleAxis(-assistedAngleX - cameraPitch, Camera.main.transform.right) * Camera.main.transform.forward));
            }
            else
            {
                Debug.Log("object thrown");

                //apply velocity at default angle
                HeldObject.GetComponent<Rigidbody>().velocity = playerThrowSettings.ThrowVelocity * (Quaternion.AngleAxis(-playerThrowSettings.ThrowAngle, Camera.main.transform.right) * Camera.main.transform.forward);
            }

            //reset accuracy bonus
            playerThrowSettings.AccuracyBonus = 0.0f;

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
            if (Vector3.Angle(new Vector3(targetDirection.x, 0.0f, targetDirection.z), new Vector3(Camera.main.transform.forward.x, 0.0f, Camera.main.transform.forward.z)) > playerThrowSettings.FullAccuracyBonus / 2.0f * playerThrowSettings.AccuracyBonus)
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
            if (Physics.SphereCast(Camera.main.transform.position, playerThrowSettings.GrabSphereCastRadius, Camera.main.transform.forward, out RaycastHit hit, playerThrowSettings.GrabDistance))
            {
                //if throwable object
                ThrowableObject throwableObject = hit.collider.GetComponent<ThrowableObject>();
                if (throwableObject != null)
                {
                    //grab object
                    GrabObject(throwableObject);
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