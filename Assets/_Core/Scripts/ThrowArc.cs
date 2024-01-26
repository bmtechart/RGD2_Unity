using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowArc : MonoBehaviour
{
    public FoodInteractionController Player;

    private LineRenderer _lineRenderer;

    private Vector3[] _lineVertices;

    [SerializeField] public Hand ThrowingHand = Hand.Left;
    [SerializeField, Range(2, 500)] public int LineResolution = 50;
    [SerializeField, Range(1.0f, 50.0f)] public float TrajectoryLength = 5;

    private void Start()
    {
        //get line renderer
        _lineRenderer = GetComponent<LineRenderer>();

        //init line vertices
        _lineVertices = new Vector3[LineResolution];
        for (int i = 0; i < _lineVertices.Length; i++)
        {
            _lineVertices[i] = Vector3.zero;
        }
        _lineRenderer.positionCount = _lineVertices.Length;
        _lineRenderer.SetPositions(_lineVertices);
    }

    private void Update()
    {
        //draw arc if charging throw
        if (ThrowingHand == Hand.Left && Player.LeftHandThrowCharge > 0.0f)
        {
            //draw left handed arc
            DrawArc(Player.LeftHandObjectPosition, Player.LeftHandThrowCharge, Player.MaxSingleHandForce, Player.LeftHandFoodObject);
        }
        else if (ThrowingHand == Hand.Right && Player.RightHandThrowCharge > 0.0f)
        {
            //draw left handed arc
            DrawArc(Player.RightHandObjectPosition, Player.RightHandThrowCharge, Player.MaxSingleHandForce, Player.RightHandFoodObject);
        }
        else if (ThrowingHand == Hand.Dual && Player.DualHandThrowCharge > 0.0f)
        {
            //draw left handed arc
            DrawArc(Player.DualHandObjectPosition, Player.DualHandThrowCharge, Player.MaxDualHandForce, Player.DualHandFoodObject);
        }
        else
        {
            //hide arc
            _lineRenderer.enabled = false;
        }
    }

    private void DrawArc(Vector3 startPoint, float throwCharge, float maxForce, FoodObject food)
    {
        //calculate vertices
        for(int i = 0; i < _lineVertices.Length; i++)
        {
            if (i == 0)
            {
                _lineVertices[0] = startPoint;
            }
            else
            {
                //find x coordinate
                float xCoordinate = TrajectoryLength * i / (_lineVertices.Length - 1);

                //estimate initial launch velocity
                float estimatedVelocity = Time.fixedDeltaTime * throwCharge * maxForce / food.GetComponent<Rigidbody>().mass;

                //clamp estimated velocity
                if (estimatedVelocity > food.GetComponent<Rigidbody>().maxLinearVelocity)
                {
                    estimatedVelocity = food.GetComponent<Rigidbody>().maxLinearVelocity;
                }

                //find angle
                float radianAngle = Mathf.Deg2Rad * (Vector3.Angle(Player.transform.forward, Camera.main.transform.forward) - Player.ThrowAngle);
                Debug.Log(radianAngle);

                //find y coordinate
                float yCoordinate = -(xCoordinate * Mathf.Tan(radianAngle) - (Physics.gravity.y * xCoordinate * xCoordinate / (2 * estimatedVelocity * estimatedVelocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle))));

                //calculate vertex
                _lineVertices[i] = startPoint + new Vector3(0.0f, yCoordinate, xCoordinate);
            }
        }

        //set vertices
        _lineRenderer.SetPositions(_lineVertices);

        //show arc
        _lineRenderer.enabled = true;
    }
}
