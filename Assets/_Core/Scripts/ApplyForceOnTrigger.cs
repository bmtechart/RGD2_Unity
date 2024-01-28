using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForceOnCollisionOrTrigger : MonoBehaviour
{
    // Force magnitude to be applied
    public float forceMagnitude = 10f;

    // Tag of the object that will trigger the force application
    public string triggerTag = "ApplyForceTag";

    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the specified tag and a Rigidbody
        if (other.gameObject.CompareTag(triggerTag) && other.attachedRigidbody != null)
        {
            ApplyForce(other.attachedRigidbody);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the specified tag and a Rigidbody
        if (collision.gameObject.CompareTag(triggerTag) && collision.rigidbody != null)
        {
            ApplyForce(collision.rigidbody);
        }
    }

    private void ApplyForce(Rigidbody rb)
    {
        // Apply a force in the forward direction of this GameObject
        rb.AddForce(transform.forward * forceMagnitude, ForceMode.Impulse);
    }
}
