using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActiveRagdoll;

public class RagdollLimb : MonoBehaviour
{
    ActiveRagdollController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<ActiveRagdollController>();
        if (!controller) Debug.Log("Failed to find ragdoll controller");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Food")) return;

        if (controller.ActiveRagdollEnabled) controller.ActiveRagdollEnabled = false;
        Debug.Log("food collided with target!");
        GetComponent<Rigidbody>().AddForce((collision.impulse*-1.0f) * 500.0f, ForceMode.Impulse);
    }
}
