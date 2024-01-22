using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActiveRagdoll 
{
    public class ThrowTestBall : MonoBehaviour
    {
        [SerializeField] GameObject TestBall;
        [SerializeField] GameObject BallSpawn;
        [SerializeField] float ThrowForce;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnThrow()
        {
            GameObject ball = Instantiate(TestBall, BallSpawn.transform);
            ball.transform.SetParent(null);
            //ball.transform.position = BallSpawn.transform.position;

            ball.GetComponentInChildren<Rigidbody>().AddForce(Camera.main.transform.forward * ThrowForce, ForceMode.Impulse);
        }
    }
}

