using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoForward : MonoBehaviour
{
    public int Speed;
    public Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = transform.up * Speed * Time.deltaTime;
        body.AddForce(Vector3.down * Speed * 2);
    }
}
