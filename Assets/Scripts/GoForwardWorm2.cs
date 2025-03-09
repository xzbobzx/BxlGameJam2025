using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoForwardWorm2 : MonoBehaviour
{
    public float speed;
    public Rigidbody body;
    public float sineAngle = 20;
    public float sineSpeed = 15;

    public float randomTicker = 3;
    public float randomTorque = 0;

    private void Start()
    {
        randomTorque = Random.Range( -7, 7 );
        randomTicker = Random.Range( 2, 5 );
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if( body == null )
        {
            if( transform.parent == null )
            {
                body = GetComponent<Rigidbody>();
            }

            if( body == null )
            {
                return;
            }
        }

        randomTicker -= Time.fixedDeltaTime;

        if( randomTicker < 0 )
        {
            randomTorque = Random.Range( -0.1f, 0.1f );
            randomTicker = Random.Range( 2, 5 );
        }

        Vector3 force = transform.up * speed * 0.1f;
        force.Scale( new Vector3( 1, 0, 1 ) );

        body.AddForce( force, ForceMode.VelocityChange );
        //body.AddForce( -Vector3.up, ForceMode.VelocityChange );

        body.AddTorque( ( Vector3.up * Mathf.Sin( Time.time * sineSpeed ) * sineAngle ) + ( Vector3.up * randomTorque ) ); 
    }
}
