using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    protected Rigidbody _rigidbody;
    protected Collider _collider;

    protected virtual void Start()
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

    //on collision enter,
        //if collision object is damageable
        //damage it
}