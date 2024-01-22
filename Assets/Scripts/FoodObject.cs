using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Collider _collider;

    private void Start()
    {
        //get rigidbody
        _rigidbody = GetComponent<Rigidbody>();

        //get collider
        _collider = GetComponent<Collider>();
    }

    //pick up object
    public void HoldObject()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _collider.enabled = false;
    }

    //drop object
    public void DropObject()
    {
        _rigidbody.constraints = RigidbodyConstraints.None;
        _collider.enabled = true;
    }
}
