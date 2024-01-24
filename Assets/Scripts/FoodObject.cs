using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class FoodObject : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;

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
