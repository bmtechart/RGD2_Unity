using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public enum HandState
{
    Empty,
    HoldingLight,
    HoldingHeavy
}

public enum HandSide
{
    Left,
    Right
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
    [SerializeField][Range(0.0f, 10.0f)] public float PickUpDistance = 2.0f;
    [SerializeField][Range(0.0f, 500.0f)] public float SingleHandMassLimit = 5.0f;

    [Header("Throw")]
    [SerializeField][Range(0.0f, 90.0f)] public float ThrowAngle = 30.0f;
    [SerializeField][Range(0.001f, 30.0f)] public float TimeToFullCharge = 5.0f;
    [SerializeField][Range(0.0f, 1000.0f)] public float MaxSingleHandForce = 50.0f;
    [SerializeField][Range(0.0f, 1000.0f)] public float MaxDualHandForce = 100.0f;

    private float _leftThrowStartTime;
    private float _rightThrowStartTime;
    [SerializeField] private bool isThrowingLeft;
    [SerializeField] private bool isThrowingRight;

    [Header("Held Food Positions")]
    [SerializeField] public Vector3 LeftHandObjectPosition = new Vector3(-0.5f, -0.5f, 1.0f);
    [SerializeField] public Vector3 RightHandObjectPosition = new Vector3(0.0f, -0.5f, 1.0f);
    [SerializeField] public Vector3 DualHandObjectPosition = new Vector3(0.5f, -0.5f, 1.0f);



    

    #region Throwing

    /// <summary>
    /// Bind throw functions to input actions. 
    /// </summary>
    private void BindThrowInput(HandSide hand)
    {
        if(hand == HandSide.Left)
        {
            _leftHandInputAction.performed += StartThrow;
            _leftHandInputAction.canceled += EndThrow;
        }

        if(hand == HandSide.Right)
        {
            _rightHandInputAction.performed += StartThrow;
            _rightHandInputAction.canceled += EndThrow;
        }
    }

    private void UnbindThrowInput(HandSide hand)
    {
        if(hand == HandSide.Left)
        {
            _leftHandInputAction.performed -= StartThrow;
            _leftHandInputAction.canceled -= EndThrow;
        }

        if (hand == HandSide.Right)
        {
            _rightHandInputAction.performed -= StartThrow;
            _rightHandInputAction.canceled -= EndThrow;
        }
    }

    /// <summary>
    /// Function bound to throw event. 
    /// </summary>
    /// <param name="ctx"> Input action callback provides information about player input. Necessary to bind to input action event. </param>
    public void StartThrow(InputAction.CallbackContext ctx)
    {
        switch(ctx.action.name)
        {
            case "LeftHand":
                isThrowingLeft = true;
                _leftThrowStartTime = (float)ctx.time;
                break;
            case "RightHand":
                isThrowingRight = true;
                _rightThrowStartTime = (float)ctx.time;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// This goes on Update to update arm animation. All logic for actually throwing is on the end throw callback.
    /// </summary>
    public void ChargeThrow()
    {

    }

    /// <summary>
    /// Bind to release of throw input. Gets time since input was pressed to calculate charge for throw.
    /// When throw input is released, rebind input to grab functions
    /// </summary>
    /// <param name="ctx">Input action callback provides information about player input. Necessary to bind to input action event.</param>
    public void EndThrow(InputAction.CallbackContext ctx)
    {
        HandSide inputHand = HandSide.Left;
        float ThrowCharge = 0.0f;

        switch(ctx.action.name)
        {
            case "LeftHand":
                ThrowCharge = Mathf.InverseLerp(0.0f, TimeToFullCharge, Time.realtimeSinceStartup - _leftThrowStartTime);
                inputHand = HandSide.Left;
                break;

            case "RightHand":
                ThrowCharge = Mathf.InverseLerp(0.0f, TimeToFullCharge, Time.realtimeSinceStartup - _rightThrowStartTime);
                inputHand = HandSide.Right;
                break;

            default:
                break;
        }

        ThrowItem(inputHand, ThrowCharge);
        UnbindThrowInput(inputHand);
        BindGrabInputs(inputHand);
        Debug.Log("Grab Mode");
    }

    /// <summary>
    /// Throws item held in player's hand.
    /// </summary>
    /// <param name="hand">Which hand is throwing the object.</param>
    /// <param name="throwPower">Normalized value for throw power.</param>
    private void ThrowItem(HandSide hand, float throwPower) 
    {
        FoodObject foodToThrow = null;
        Debug.Log("Throw item from " + hand.ToString() + " with power " + throwPower.ToString());
        switch(hand)
        {
            case HandSide.Left:
                if (!_leftHandFoodObject) return;
                foodToThrow = _leftHandFoodObject;
                break;


            case HandSide.Right:
                if (!_rightHandFoodObject) return;
                foodToThrow = _rightHandFoodObject;
                break;

            default:
                break;
        }

        if (!foodToThrow) return;

        foodToThrow.DropObject();

        Rigidbody foodRigidBody = foodToThrow.GetComponent<Rigidbody>();
        if (!foodRigidBody) return;

        foodRigidBody.transform.SetParent(null);

        foodRigidBody.AddForce(Camera.main.transform.forward * (throwPower*MaxSingleHandForce), ForceMode.Impulse);

        switch (hand)
        {
            case HandSide.Left:
                _leftHandFoodObject = null;
                break;


            case HandSide.Right:
                _rightHandFoodObject = null;
                break;

            default:
                break;
        }
    }

    #endregion

    #region Grabbing

    private void BindGrabInputs(HandSide hand)
    {
        if (hand == HandSide.Left)
        {
            _leftHandInputAction.performed += StartGrab;
            _leftHandInputAction.canceled += EndGrab;
        }
        
        else if (hand == HandSide.Right)
        {
            _rightHandInputAction.performed += StartGrab;
            _rightHandInputAction.canceled += EndGrab;
        }
    }

    private void UnbindGrabInputs(HandSide hand)
    {
        if (hand == HandSide.Left) 
        { 
            _leftHandInputAction.performed -= StartGrab;
            _leftHandInputAction.canceled -= EndGrab;

        }
        
        if(hand == HandSide.Right)
        {
            _rightHandInputAction.performed -= StartGrab;
            _rightHandInputAction.canceled -= EndGrab;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctx"></param>
    public void StartGrab(InputAction.CallbackContext ctx)
    {
        if (ctx.action.name == "LeftHand")
        {
            if (_leftHandFoodObject) return;
            Grab(HandSide.Left);
            Debug.Log("Grab left");
        }

        if(ctx.action.name == "RightHand")
        {
            if (_rightHandFoodObject) return;
            Grab(HandSide.Right);
            Debug.Log("Grab Right");
        }

    }


    /// <summary>
    /// When grab input is released, rebind input to throw fucntions
    /// </summary>
    /// <param name="ctx"></param>
    public void EndGrab(InputAction.CallbackContext ctx)
    {
        if (ctx.action.name == "LeftHand")
        {
            if (!_leftHandFoodObject) return;
            UnbindGrabInputs(HandSide.Left);
            BindThrowInput(HandSide.Left);
        }

        if (ctx.action.name == "RightHand")
        {
            if (!_rightHandFoodObject) return;
            UnbindGrabInputs(HandSide.Right);
            BindThrowInput(HandSide.Right);
        }
    }


    private void Grab(HandSide hand)
    {
        FoodObject food = null;

        //ray trace for food object
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, PickUpDistance))
        {
            //if food
            food = hit.collider.GetComponent<FoodObject>();
        }
        //if we aren't grabbing food, exit
        if (!food) return;


        //if light food, grab it
        if (food.GetComponent<Rigidbody>().mass <= SingleHandMassLimit)
        {
            //disable physics
            food.HoldObject();
            //position
            food.transform.SetParent(Camera.main.transform);
        }

        //set food position and state based on which hand is grabbing
        if(hand == HandSide.Left)
        {
            _leftHandFoodObject = food;
            food.transform.localPosition = LeftHandObjectPosition;

            //change hand state
            _leftHandState = HandState.HoldingLight;
        }
        
        if(hand == HandSide.Right)
        {
            _rightHandFoodObject = food;
            food.transform.localPosition = RightHandObjectPosition;

            //change hand state
            _rightHandState = HandState.HoldingLight;
        }
    }

    #endregion

    #region Callbacks

    private void Start()
    {
        //get playr input
        _playerInput = GetComponent<PlayerInput>();

        //get button actions for held input
        _leftHandInputAction = _playerInput.actions.FindAction("LeftHand");
        _rightHandInputAction = _playerInput.actions.FindAction("RightHand");

        BindGrabInputs(HandSide.Left);
        BindGrabInputs(HandSide.Right);

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

    private void Update()
    {
        ChargeThrow();
    }

    private void FixedUpdate()
    {
        //throw held food objects
        //ThrowFoodObjects(); 
    }

    #endregion
    
    

    private void LeftHandThrow()
    {
        if (_leftHandFoodObject != null)
        {
            Debug.Log("food object thrown with left hand");
            //unparent
            _leftHandFoodObject.transform.SetParent(null);

            //enable physics
            _leftHandFoodObject.DropObject();

            //apply force
            _leftHandFoodObject.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * _leftHandThrowCharge * MaxSingleHandForce);

            //reset charge
            _leftHandThrowCharge = 0.0f;

            //change hand state
            _leftHandState = HandState.HoldingLight;

            //remove reference
            _leftHandFoodObject = null;
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

}
