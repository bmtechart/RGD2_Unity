using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    public float hp;
    public GameObject destroyed;
   


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "HeavyThrowable")
        {
            hp--;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    // {
    //     if(collision.gameObject.tag == "ThrowableObject")
    //     {
    //         hp--;
    //     }
    // }

    private void Update()
    {


        if (hp <= 0)
        {
            Instantiate(destroyed, transform.position, transform.rotation);
            Destroy(gameObject);
        } 
        
    }

}
