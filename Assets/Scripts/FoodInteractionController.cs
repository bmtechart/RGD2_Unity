using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public enum HandState
{
    Empty,
    HoldingLight,
    HoldingHeavy,
    ThrowingLight,
    ThrowingHeavy
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

    [Header("Pick-Up")]
    [SerializeField] public float PickUpDistance = 2.0f;
    [SerializeField] public float SingleHandMassLimit = 5.0f;

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
    }

    private void Update()
    {
        //throw held food objects
        //TODO: throw held food objects
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
                        food.HoldObject();
                        food.transform.SetParent(Camera.main.transform);
                        food.transform.localPosition = LeftHandObjectPosition;
                        Debug.Log("food object picked up with left hand");

                        //change hand state
                        _leftHandState = HandState.HoldingLight;
                    }
                    else
                    {
                        //try picking up heavy food
                        foreach (ButtonControl buttonControl in _rightHandInputAction.controls)
                        {
                            if (buttonControl.isPressed && _rightHandState == HandState.Empty)
                            {
                                //pick up heavy food
                                //TODO: pick up heavy food

                                //change hand states
                                _leftHandState = HandState.HoldingHeavy;
                                _rightHandState = HandState.HoldingHeavy;
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
                        food.HoldObject();
                        food.transform.SetParent(Camera.main.transform);
                        food.transform.localPosition = RightHandObjectPosition;
                        Debug.Log("food object picked up with right hand");

                        //change hand state
                        _rightHandState = HandState.HoldingLight;
                    }
                    else
                    {
                        //try picking up heavy food
                        foreach (ButtonControl buttonControl in _leftHandInputAction.controls)
                        {
                            if (buttonControl.isPressed && _leftHandState == HandState.Empty)
                            {
                                //pick up heavy food
                                //TODO: pick up heavy food

                                //change hand states
                                _rightHandState = HandState.HoldingHeavy;
                                _leftHandState = HandState.HoldingHeavy;
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
