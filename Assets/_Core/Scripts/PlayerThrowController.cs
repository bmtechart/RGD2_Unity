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

    [SerializeField] public PlayerThrowSettings PlayerThrowSettings;

    [SerializeField] public Transform LeftShoulder;
    [SerializeField] public Transform RightShoulder;
    [SerializeField] public Transform LeftHandIKRigTarget;
    [SerializeField] public Transform RightHandIKRigTarget;
    private Transform LeftHandIKCameraTarget;
    private Transform RightHandIKCameraTarget;
    private Vector3 _leftHandPosition;
    private Vector3 _rightHandPosition;
    private Vector3 _leftHandChargePosition;
    private Vector3 _rightHandChargePosition;
    private Vector3 _leftHandThrowPosition;
    private Vector3 _rightHandThrowPosition;

    private float _animationTimer;

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

        //init hand positions and rotations
        _rightHandPosition = PlayerThrowSettings.HeldObjectPosition;
        _leftHandPosition = new Vector3(-_rightHandPosition.x, _rightHandPosition.y, _rightHandPosition.z);
        _rightHandChargePosition = PlayerThrowSettings.ChargedObjectPosition;
        _leftHandChargePosition = PlayerThrowSettings.ChargedOffHandPosition;
        _rightHandThrowPosition = PlayerThrowSettings.ThrownHandPosition;
        _leftHandThrowPosition = PlayerThrowSettings.ThrownOffHandPosition;
        RightHandIKCameraTarget = Camera.main.GetComponent<CameraHandTargetReference>().RightHandIKTarget;
        LeftHandIKCameraTarget = Camera.main.GetComponent<CameraHandTargetReference>().LeftHandIKTarget;
        RightHandIKCameraTarget.localPosition = _rightHandPosition;
        LeftHandIKCameraTarget.localPosition = _leftHandPosition;
        RightHandIKRigTarget.position = RightHandIKCameraTarget.position;
        LeftHandIKRigTarget.position = LeftHandIKCameraTarget.position;
        RightHandIKCameraTarget.rotation = RightHandIKRigTarget.rotation;
        LeftHandIKCameraTarget.rotation = LeftHandIKRigTarget.rotation;

        //init other variables
        _animationTimer = 0.0f;
        PlayerThrowSettings.AccuracyBonus = 0.0f;
        _accuracyBonusTimer = 0.0f;
        _maxTargetDistance = PlayerThrowSettings.ThrowVelocity * PlayerThrowSettings.ThrowVelocity / Mathf.Abs(Physics.gravity.y);
    }

    private void Update()
    {
        //position and rotate hands
        LeftHandIKRigTarget.position = LeftHandIKCameraTarget.position;
        RightHandIKRigTarget.position = RightHandIKCameraTarget.position;
        LeftHandIKRigTarget.rotation = LeftHandIKCameraTarget.rotation;
        RightHandIKRigTarget.rotation = RightHandIKCameraTarget.rotation;

        //unbreak elbows
        if (Vector3.Distance(LeftHandIKRigTarget.position, LeftShoulder.position) > PlayerThrowSettings.ArmLength)
        {
            LeftHandIKRigTarget.position = LeftShoulder.position + (LeftHandIKRigTarget.position - LeftShoulder.position).normalized * PlayerThrowSettings.ArmLength;
        }
        if (Vector3.Distance(RightHandIKRigTarget.position, RightShoulder.position) > PlayerThrowSettings.ArmLength)
        {
            RightHandIKRigTarget.position = RightShoulder.position + (RightHandIKRigTarget.position - RightShoulder.position).normalized * PlayerThrowSettings.ArmLength;
        }

        //position object
        if (HeldObject != null)
        {
            HeldObject.transform.position = RightHandIKRigTarget.position;
        }
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
            if (_accuracyBonusTimer < PlayerThrowSettings.TimeToFullAccuracy)
            {
                PlayerThrowSettings.AccuracyBonus = _accuracyBonusTimer / PlayerThrowSettings.TimeToFullAccuracy;
            }
            else
            {
                PlayerThrowSettings.AccuracyBonus = 1.0f - (_accuracyBonusTimer - PlayerThrowSettings.TimeToFullAccuracy) / PlayerThrowSettings.TimeFromFullAccuracy;
            }

            //clamp accuracy bonus
            if (PlayerThrowSettings.AccuracyBonus < 0.0f)
            {
                PlayerThrowSettings.AccuracyBonus = 0.0f;
            }

            //set object & hand position based on accuracy bonus
            LeftHandIKCameraTarget.localPosition = Vector3.Lerp(_leftHandPosition, _leftHandChargePosition, PlayerThrowSettings.AccuracyBonus);
            RightHandIKCameraTarget.localPosition = Vector3.Lerp(_rightHandPosition, _rightHandChargePosition, PlayerThrowSettings.AccuracyBonus);
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
        //stop animation coroutines
        StopAllCoroutines();

        //assign
        HeldObject = throwableObject;

        //disable physics
        HeldObject.HoldObject();

        //reparent
        HeldObject.transform.SetParent(Camera.main.transform);

        //change hand state
        _handState = HandState.Holding;

        //animate hand
        StartCoroutine(GrabAnimationCoroutine());
    }

    private IEnumerator GrabAnimationCoroutine()
    {
        _animationTimer = PlayerThrowSettings.GrabAnimationTime;

        RightHandIKCameraTarget.position = HeldObject.transform.position;

        Vector3 LeftHandIKCameraTargetStartingPosition = LeftHandIKCameraTarget.localPosition;
        Vector3 RightHandIKCameraTargetGrabPosition = RightHandIKCameraTarget.localPosition;

        while (_animationTimer > 0.0f)
        {
            _animationTimer -= Time.deltaTime;

            LeftHandIKCameraTarget.localPosition = Vector3.Lerp(_leftHandPosition, LeftHandIKCameraTargetStartingPosition, _animationTimer / PlayerThrowSettings.GrabAnimationTime);
            RightHandIKCameraTarget.localPosition = Vector3.Lerp(_rightHandPosition, RightHandIKCameraTargetGrabPosition, _animationTimer / PlayerThrowSettings.GrabAnimationTime);

            yield return null;
        }

        LeftHandIKCameraTarget.localPosition = _leftHandPosition;
        RightHandIKCameraTarget.localPosition = _rightHandPosition;
    }

    private void ThrowObject()
    {
        if (HeldObject != null)
        {
            //stop animation coroutines
            StopAllCoroutines();

            //unparent
            HeldObject.transform.SetParent(null);

            //enable physics
            HeldObject.DropObject();

            //get target if using aim assist
            AimAssistTarget target = null;
            if (PlayerThrowSettings.UseAimAssist)
            {
                target = GetAimAssistTarget();
            }

            //apply velocity
            if (target != null)
            {

                //determine modified angles for aim assist
                float assistedAngleX = Mathf.Rad2Deg * -(Mathf.Asin(Vector3.Distance(new Vector3(target.transform.position.x, 0.0f, target.transform.position.z), new Vector3(HeldObject.transform.position.x, 0.0f, HeldObject.transform.position.z)) * Physics.gravity.y / PlayerThrowSettings.ThrowVelocity / PlayerThrowSettings.ThrowVelocity) / 2.0f);
                float assistedAngleY = Vector3.SignedAngle(new Vector3(target.transform.position.x, 0.0f, target.transform.position.z) - new Vector3(HeldObject.transform.position.x, 0.0f, HeldObject.transform.position.z), new Vector3(Camera.main.transform.forward.x, 0.0f, Camera.main.transform.forward.z), Vector3.up);
                
                //find camera pitch
                float cameraPitch = Vector3.SignedAngle(new Vector3(Camera.main.transform.forward.x, 0.0f, Camera.main.transform.forward.z), Camera.main.transform.forward, Camera.main.transform.right);

                //apply velocity at modified angle
                HeldObject.GetComponent<Rigidbody>().velocity = PlayerThrowSettings.ThrowVelocity * (Quaternion.AngleAxis(-assistedAngleY, Vector3.up) * (Quaternion.AngleAxis(-assistedAngleX - cameraPitch, Camera.main.transform.right) * Camera.main.transform.forward));
            }
            else
            {

                //apply velocity at default angle
                HeldObject.GetComponent<Rigidbody>().velocity = PlayerThrowSettings.ThrowVelocity * (Quaternion.AngleAxis(-PlayerThrowSettings.ThrowAngle, Camera.main.transform.right) * Camera.main.transform.forward);
            }
            HeldObject.isThrown = true;
            //reset accuracy bonus
            PlayerThrowSettings.AccuracyBonus = 0.0f;

            //change hand state
            _handState = HandState.Empty;

            //remove reference
            HeldObject = null;

            //animate hand
            StartCoroutine(ThrowAnimationCoroutine());
        }
    }

    private IEnumerator ThrowAnimationCoroutine()
    {
        _animationTimer = PlayerThrowSettings.ThrowAnimationTime1;

        while(_animationTimer > 0.0f)
        {
            _animationTimer -= Time.deltaTime;

            LeftHandIKCameraTarget.localPosition = Vector3.Lerp(LeftHandIKCameraTarget.localPosition, _leftHandThrowPosition, Time.deltaTime * PlayerThrowSettings.ThrowAnimationSpeed);
            RightHandIKCameraTarget.localPosition = Vector3.Lerp(RightHandIKCameraTarget.localPosition, _rightHandThrowPosition, Time.deltaTime * PlayerThrowSettings.ThrowAnimationSpeed);

            yield return null;
        }

        Vector3 LeftHandIKCameraTargetExtendedPosition = LeftHandIKCameraTarget.localPosition;
        Vector3 RightHandIKCameraTargetExtendedPosition = RightHandIKCameraTarget.localPosition;

        _animationTimer = PlayerThrowSettings.ThrowAnimationTime2;

        while (_animationTimer > 0.0f)
        {
            _animationTimer -= Time.deltaTime;

            LeftHandIKCameraTarget.localPosition = Vector3.Lerp(_leftHandPosition, LeftHandIKCameraTargetExtendedPosition, _animationTimer / PlayerThrowSettings.ThrowAnimationTime2);
            RightHandIKCameraTarget.localPosition = Vector3.Lerp(_rightHandPosition, RightHandIKCameraTargetExtendedPosition, _animationTimer / PlayerThrowSettings.ThrowAnimationTime2);

            yield return null;
        }

        LeftHandIKCameraTarget.localPosition = _leftHandPosition;
        RightHandIKCameraTarget.localPosition = _rightHandPosition;
    }

    private AimAssistTarget GetAimAssistTarget()
    {
        List<AimAssistTarget> aimAssistTargets = new List<AimAssistTarget>(FindObjectsByType<AimAssistTarget>(FindObjectsSortMode.InstanceID));

        //remove targets out of bonus arc
        for (int i = aimAssistTargets.Count - 1; i >= 0; i--)
        {
            Vector3 targetDirection = aimAssistTargets[i].transform.position - Camera.main.transform.position;
            if (Vector3.Angle(new Vector3(targetDirection.x, 0.0f, targetDirection.z), new Vector3(Camera.main.transform.forward.x, 0.0f, Camera.main.transform.forward.z)) > PlayerThrowSettings.FullAccuracyBonus / 2.0f * PlayerThrowSettings.AccuracyBonus)
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
            if (Physics.SphereCast(Camera.main.transform.position, PlayerThrowSettings.GrabSphereCastRadius, Camera.main.transform.forward, out RaycastHit hit, PlayerThrowSettings.GrabDistance))
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