using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleParentController : MonoBehaviour
{
    public CharacterController controller;

    public Transform spouse;

    public Transform moveTarget;
    public MoveMomDadTarget mover;
    public Transform headHolder;
    public Transform arm1;
    public Transform arm2;
    public Transform leg1;
    public Transform leg2;

    public float speed = 3;


    // Update is called once per frame
    void Update()
    {
        Vector3 directionToTarget = ( moveTarget.position - transform.position ).normalized;

        Vector3 movement = (directionToTarget * speed) * Time.deltaTime + (Vector3.down * 9.81f * Time.deltaTime);

        if( mover.waitForKiddo )
        {
            controller.transform.LookAt( new Vector3( mover.kiddo.transform.position.x, transform.position.y, mover.kiddo.transform.position.z ) );

            LookHeadAt( mover.kiddo );
            StaticLimbs();
        }
        else if( Vector3Helpers.GetHorizontalDistance( moveTarget.transform.position, transform.position ) < 1 )
        {
            controller.transform.LookAt( new Vector3( spouse.position.x, transform.position.y, spouse.position.z ) );

            StaticLimbs();
            StaticHead();          
        }
        else
        {
            controller.Move( movement );
            controller.transform.LookAt( new Vector3( moveTarget.transform.position.x, transform.position.y, moveTarget.transform.position.z ) );

            MoveLimbs();
            StaticHead();
        }
    }

    void StaticHead()
    {
        headHolder.localEulerAngles = new Vector3( 0, 0, 0 );
    }

    void LookHeadAt( Transform lookatTarget )
    {
        headHolder.LookAt( lookatTarget );
    }

    void StaticLimbs()
    {
        arm1.localEulerAngles = new Vector3( -90, 0, 0 );
        arm2.localEulerAngles = new Vector3( -90, 0, 0 );
        leg1.localEulerAngles = new Vector3( -90, 0, 0 );
        leg2.localEulerAngles = new Vector3( -90, 0, 0 );
    }

    void MoveLimbs()
    {
        arm1.localEulerAngles = new Vector3( -90 + Mathf.Sin( Time.time * 5 ) * 30, 0, 0 );
        arm2.localEulerAngles = new Vector3( -90 - Mathf.Sin( Time.time * 5 ) * 30, 0, 0 );
        leg1.localEulerAngles = new Vector3( -90 - Mathf.Sin( Time.time * 5 ) * 30, 0, 0 );
        leg2.localEulerAngles = new Vector3( -90 + Mathf.Sin( Time.time * 5 ) * 30, 0, 0 );
    }
}
