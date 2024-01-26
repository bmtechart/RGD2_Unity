using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Collider _collider;

    [Header("Physical Properties")]
    [SerializeField, Range(0.001f, 10.0f)] public float StandardMass = 1.0f;
    [SerializeField, Range(0.1f, 5.0f)] public float DragCoefficient = 0.47f;
    [SerializeField, Range(10.0f, 50.0f)] public float MaxVelocity = 20.0f;
    [SerializeField, Range(0.0f, 10.0f)] public float ObjectCoreRadius = 0.25f; //for random torque

    private void Start()
    {
        //get rigidbody
        _rigidbody = GetComponent<Rigidbody>();

        //set physical properties
        _rigidbody.mass = StandardMass * transform.localScale.x * transform.localScale.y * transform.localScale.z;
        _rigidbody.drag = DragCoefficient;
        _rigidbody.maxLinearVelocity = MaxVelocity;

        //get collider
        _collider = GetComponent<Collider>();
    }

    public void ScaleFood(Vector3 scale)
    {
        //set scale
        transform.localScale = scale;

        //set physical properties
        _rigidbody.mass = StandardMass * transform.localScale.x * transform.localScale.y * transform.localScale.z;
    }

    //pick up object
    public void HoldObject()
    {
        _rigidbody.isKinematic = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _collider.enabled = false;
    }

    //drop object
    public void DropObject()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.constraints = RigidbodyConstraints.None;
        _collider.enabled = true;
    }
}
