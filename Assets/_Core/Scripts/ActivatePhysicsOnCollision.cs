using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePhysicsOnCollision : MonoBehaviour
{
    // Rigidbody component
    private Rigidbody rb;

    // Tag of the object that will trigger the physics activation
    public string triggerTag = "ActivatePhysicsTag";

    void Start()
    {
        // Get the Rigidbody component attached to this object
        rb = GetComponent<Rigidbody>();

        // Initially set isKinematic to true to disable physics
        rb.isKinematic = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the specified tag
        if (collision.gameObject.CompareTag(triggerTag))
        {
            // Activate physics by setting isKinematic to false
            rb.isKinematic = false;
        }
    }
}
