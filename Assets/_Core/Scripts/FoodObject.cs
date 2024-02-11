using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : ThrowableObject
{
    protected Vector3 _meshExtents;

    [SerializeField, Range(10.0f, 50.0f)] public float MaxVelocity = 20.0f;
    internal float ObjectCoreRadius; //for random torque

    protected override void Start()
    {
        base.Start();

        //get mesh extents
        _meshExtents = GetComponent<MeshFilter>().mesh.bounds.extents;

        //set max velocity
        _rigidbody.maxLinearVelocity = MaxVelocity;

        //set object core radius
        ObjectCoreRadius = (_meshExtents.x + _meshExtents.y + _meshExtents.z) / 3.0f;
    }
}
