using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTestBall : MonoBehaviour
{
    [SerializeField] GameObject TestBall;
    [SerializeField] Transform BallSpawn;
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
        GameObject ball = Instantiate(TestBall, new Vector3(0, 0, -1), Quaternion.identity);
        ball.transform.position = BallSpawn.position;
        ball.GetComponentInChildren<Rigidbody>().AddForce(Camera.main.transform.forward*ThrowForce, ForceMode.Impulse);
    }
}
