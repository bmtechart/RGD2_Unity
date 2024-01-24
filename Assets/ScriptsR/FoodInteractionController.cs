using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public enum HandState
{
    Empty,
    LiftingLight,
    LiftingHeavy,
    HoldingLight,
    HoldingHeavy
}

public class FoodInteractionController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _leftHandInputAction;
    private InputAction _rightHandInputAction;

    private HandState _leftHandState;
    private HandState _rightHandState;

    private FoodObject _leftHandFoodObject;
    private FoodObject _rightHandFoodObject;
    private FoodObject _dualHandFoodObject;

    private float _leftHandThrowCharge;
    private float _rightHandThrowCharge;
    private float _dualHandThrowCharge;

    [Header("Pick-Up")]
    [SerializeField, Range(0.0f, 10.0f)] public float PickUpDistance = 2.0f;
    [SerializeField, Range(0.0f, 500.0f)] public float SingleHandMassLimit = 5.0f;

    [Header("Throw")]
    [SerializeField, Range(0.0f, 90.0f)] public float ThrowAngle = 30.0f;
    [SerializeField, Range(0.001f, 30.0f)] public float TimeToFullCharge = 5.0f;
    [SerializeField, Range(0.0f, 100000.0f)] public float MaxSingleHandForce = 5000.0f;
    [SerializeField, Range(0.0f, 100000.0f)] public float MaxDualHandForce = 10000.0f;

    [Header("Held Food Positions")]
    [SerializeField] public Vector3 LeftHandObjectPosition = new Vector3(-0.5f, -0.5f, 1.0f);
    [SerializeField] public Vector3 RightHandObjectPosition = new Vector3(0.0f, -0.5f, 1.0f);
    [SerializeField] public Vector3 DualHandObjectPosition = new Vector3(0.5f, -0.5f, 1.0f);

    private void Start()
    {
        //get playr input
        _playerInput = GetComponent<PlayerInput>();

        //get button actions for held input
        _leftHandInputAction = _playerInput.actions.FindAction("LeftHand");
        _rightHandInputAction = _playerInput.actions.FindAction("RightHand");

        //enable food interaction input
        _playerInput.actions.FindActionMap("FoodInteraction").Enable();

        //init hand states
        _leftHandState = HandState.Empty;
        _rightHandState = HandState.Empty;

        //init throw charges
        _leftHandThrowCharge = 0.0f;
        _rightHandThrowCharge = 0.0f;
        _dualHandThrowCharge = 0.0f;
    }

    private void FixedUpdate()
    {
        //throw held food objects
        ThrowFoodObjects();
    }

    private void ThrowFoodObjects()
    {
        //left hand charge throw
        if (_leftHandState == HandState.HoldingLight)
        {
            bool isCharging = false;

            //check input for charge throw
            foreach (ButtonControl buttonControl in _leftHandInputAction.controls)
            {
                if (buttonControl.isPressed)
                {
                    isCharging = true;
                }
            }

            if (isCharging)
            {
                //charge throw
                _leftHandThrowCharge += Time.fixedDeltaTime / TimeToFullCharge;

                //clamp charge at max
                if (_leftHandThrowCharge > 1.0f)
                {
                    _leftHandThrowCharge = 1.0f;
                }
            }
            else if (_leftHandThrowCharge > 0.0f)
            {
                //throw
                LeftHandThrow();
            }
            //else just hold
        }
        else if (_leftHandState == HandState.LiftingLight)
        {
            //don't allow throw until finished lifting (button release)
            foreach (ButtonControl buttonControl in _leftHandInputAction.controls)
            {
                if (!buttonControl.isPressed)
                {
                    _leftHandState = HandState.HoldingLight;
                }
            }
        }

        //right hand charge throw
        if (_rightHandState == HandState.HoldingLight)
        {
            bool isCharging = false;

            //check input for charge throw
            foreach (ButtonControl buttonControl in _rightHandInputAction.controls)
            {
                if (buttonControl.isPressed)
                {
                    isCharging = true;
                }
            }

            if (isCharging)
            {
                //charge throw
                _rightHandThrowCharge += Time.fixedDeltaTime / TimeToFullCharge;

                //clamp charge at max
                if (_rightHandThrowCharge > 1.0f)
                {
                    _rightHandThrowCharge = 1.0f;
                }
            }
            else if (_rightHandThrowCharge > 0.0f)
            {
                //throw
                RightHandThrow();
            }
            //else just hold
        }
        else if (_rightHandState == HandState.LiftingLight)
        {
            //don't allow throw until finished lifting (button release)
            foreach (ButtonControl buttonControl in _rightHandInputAction.controls)
            {
                if (!buttonControl.isPressed)
                {
                    _rightHandState = HandState.HoldingLight;
                }
            }
        }

        //dual hand charge throw
        if (_leftHandState == HandState.HoldingHeavy && _rightHandState == HandState.HoldingHeavy)
        {
            bool isCharging = false;

            //check input for charge throw
            foreach (ButtonControl buttonControl in _leftHandInputAction.controls)
            {
                if (buttonControl.isPressed)
                {
                    isCharging = true;
                }
            }
            foreach (ButtonControl buttonControl in _rightHandInputAction.controls)
            {
                if (buttonControl.isPressed)
                {
                    isCharging = true;
                }
            }

            if (isCharging)
            {
                //charge throw
                _dualHandThrowCharge += Time.fixedDeltaTime / TimeToFullCharge;

                //clamp charge at max
                if (_dualHandThrowCharge > 1.0f)
                {
                    _dualHandThrowCharge = 1.0f;
                }
            }
            else if (_dualHandThrowCharge > 0.0f)
            {
                //throw
                DualHandThrow();
            }
            //else just hold
        }
        else
        {
            //don't allow throw until finished lifting (buttons release)
            if (_leftHandState == HandState.LiftingHeavy)
            {
                foreach (ButtonControl buttonControl in _leftHandInputAction.controls)
                {
                    if (!buttonControl.isPressed)
                    {
                        _leftHandState = HandState.HoldingHeavy;
                    }
                }
            }
            if (_rightHandState == HandState.LiftingHeavy)
            {
                foreach (ButtonControl buttonControl in _rightHandInputAction.controls)
                {
                    if (!buttonControl.isPressed)
                    {
                        _rightHandState = HandState.HoldingHeavy;
                    }
                }
            }
        }
    }

    private void LeftHandPickUp(FoodObject food)
    {
        Debug.Log("food object picked up with left hand");
        //assign
        _leftHandFoodObject = food;
        
        //disable physics
        food.HoldObject();

        //position
        food.transform.SetParent(Camera.main.transform);
        food.transform.localPosition = LeftHandObjectPosition;

        //change hand state
        _leftHandState = HandState.LiftingLight;
    }

    private void RightHandPickUp(FoodObject food)
    {
        Debug.Log("food object picked up with right hand");
        //assign
        _rightHandFoodObject = food;

        //disable physics
        food.HoldObject();

        //position
        food.transform.SetParent(Camera.main.transform);
        food.transform.localPosition = RightHandObjectPosition;

        //change hand state
        _rightHandState = HandState.LiftingLight;
    }

    private void DualHandPickUp(FoodObject food)
    {
        Debug.Log("food object picked up with both hands");
        //assign
        _dualHandFoodObject = food;

        //disable physics
        food.HoldObject();

        //position
        food.transform.SetParent(Camera.main.transform);
        food.transform.localPosition = DualHandObjectPosition;

        //change hand state
        _leftHandState = HandState.LiftingHeavy;
        _rightHandState = HandState.LiftingHeavy;
    }

    private void LeftHandThrow()
    {
        if (_leftHandFoodObject != null)
        {
            Debug.Log("food object thrown with left hand");
            //unparent
            _leftHandFoodObject.transform.SetParent(null);

            //enable physics
            _leftHandFoodObject.DropObject();

            //apply force with random torque
            _leftHandFoodObject.GetComponent<Rigidbody>().AddForceAtPosition(Quaternion.AngleAxis(-ThrowAngle, Camera.main.transform.right) * Camera.main.transform.forward * _leftHandThrowCharge * MaxSingleHandForce, _leftHandFoodObject.transform.position + Random.insideUnitSphere * _leftHandFoodObject.ObjectCoreRadius);

            //reset charge
            _leftHandThrowCharge = 0.0f;

            //change hand state
            _leftHandState = HandState.Empty;

            //remove reference
            _leftHandFoodObject = null;
        }
    }

    private void RightHandThrow()
    {
        if (_rightHandFoodObject != null)
        {
            Debug.Log("food object thrown with right hand");
            //unparent
            _rightHandFoodObject.transform.SetParent(null);

            //enable physics
            _rightHandFoodObject.DropObject();

            //apply force with random torque
            _rightHandFoodObject.GetComponent<Rigidbody>().AddForceAtPosition(Quaternion.AngleAxis(-ThrowAngle, Camera.main.transform.right) * Camera.main.transform.forward * _rightHandThrowCharge * MaxSingleHandForce, _rightHandFoodObject.transform.position + Random.insideUnitSphere * _rightHandFoodObject.ObjectCoreRadius);

            //reset charge
            _rightHandThrowCharge = 0.0f;

            //change hand state
            _rightHandState = HandState.Empty;

            //remove reference
            _rightHandFoodObject = null;
        }
    }

    private void DualHandThrow()
    {
        if (_dualHandFoodObject != null)
        {
            Debug.Log("food object thrown with both hands");
            //unparent
            _dualHandFoodObject.transform.SetParent(null);

            //enable physics
            _dualHandFoodObject.DropObject();

            //apply force with random torque
            _dualHandFoodObject.GetComponent<Rigidbody>().AddForceAtPosition(Quaternion.AngleAxis(-ThrowAngle, Camera.main.transform.right) * Camera.main.transform.forward * _dualHandThrowCharge * MaxSingleHandForce, _dualHandFoodObject.transform.position + Random.insideUnitSphere * _dualHandFoodObject.ObjectCoreRadius);

            //reset charge
            _dualHandThrowCharge = 0.0f;

            //change hand state
            _leftHandState = HandState.Empty;
            _rightHandState = HandState.Empty;

            //remove reference
            _dualHandFoodObject = null;
        }
    }

    public void EnableFoodInteraction()
    {
        _playerInput.actions.FindActionMap("FoodInteraction").Enable();
    }

    public void DisableFoodInteraction()
    {
        _playerInput.actions.FindActionMap("FoodInteraction").Disable();
    }

    //pick up food object with left hand
    private void OnLeftHand()
    {
        if (_leftHandState == HandState.Empty)
        {
            //raycast to try pick up
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, PickUpDistance))
            {
                //if food
                FoodObject food = hit.collider.GetComponent<FoodObject>();
                if (food != null)
                {
                    //light or heavy food?
                    if (food.GetComponent<Rigidbody>().mass <= SingleHandMassLimit)
                    {
                        //pick up light food
                        LeftHandPickUp(food);
                    }
                    else
                    {
                        //try picking up heavy food
                        foreach (ButtonControl buttonControl in _rightHandInputAction.controls)
                        {
                            if (buttonControl.isPressed && _rightHandState == HandState.Empty)
                            {
                                //pick up heavy food
                                DualHandPickUp(food);
                            }
                        }

                        //if can't pick up heavy, jiggle it
                        if (_leftHandState == HandState.Empty)
                        {
                            //jiggle heavy food
                            //TODO: jiggle heavy food
                        }
                    }
                }
            }
        }
    }

    //pick up food object with right hand
    private void OnRightHand()
    {
        if (_rightHandState == HandState.Empty)
        {
            //raycast to try pick up
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, PickUpDistance))
            {
                //if food
                FoodObject food = hit.collider.GetComponent<FoodObject>();
                if (food != null)
                {
                    //light or heavy food?
                    if (food.GetComponent<Rigidbody>().mass <= SingleHandMassLimit)
                    {
                        //pick up light food
                        RightHandPickUp(food);
                    }
                    else
                    {
                        //try picking up heavy food
                        foreach (ButtonControl buttonControl in _leftHandInputAction.controls)
                        {
                            if (buttonControl.isPressed && _leftHandState == HandState.Empty)
                            {
                                //pick up heavy food
                                DualHandPickUp(food);
                            }
                        }

                        //if can't pick up heavy, jiggle it
                        if (_rightHandState == HandState.Empty)
                        {
                            //jiggle heavy food
                            //TODO: jiggle heavy food
                        }
                    }
                }
            }
        }
    }
}
