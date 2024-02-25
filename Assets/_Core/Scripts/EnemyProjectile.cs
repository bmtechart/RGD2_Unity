using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class EnemyProjectile : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody>();
        
    }

    public void Throw(Vector3 target, float force)
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!rb) return;
        rb.AddForce(Vector3.Normalize(target - transform.position) * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Invoke("DestroySelf", 3.0f);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
