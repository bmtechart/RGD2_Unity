using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : MonoBehaviour
{
    protected Rigidbody _rigidbody;
    protected Collider _collider;
    protected Vector3 _meshExtents;

    [SerializeField, Range(10.0f, 50.0f)] public float MaxVelocity = 20.0f;
    internal float ObjectCoreRadius; //for random torque

    protected virtual void Start()
    {
        //get rigidbody
        _rigidbody = GetComponent<Rigidbody>();

        //get collider
        _collider = GetComponent<Collider>();

        //get mesh extents
        _meshExtents = GetComponent<MeshFilter>().mesh.bounds.extents;

        //set max velocity
        _rigidbody.maxLinearVelocity = MaxVelocity;

        //set object core radius
        ObjectCoreRadius = (_meshExtents.x + _meshExtents.y + _meshExtents.z) / 3.0f;
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
