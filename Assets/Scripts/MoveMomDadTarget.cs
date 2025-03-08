using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMomDadTarget : MonoBehaviour
{
    public Waypoint targetWaypoint;

    public Transform kidTeleporter;

    public Transform mom;
    public Transform dad;
    public Transform momTarget;
    public Transform dadTarget;

    public Transform kiddo;

    public float distanceBeforeWalk;
    public float maxKiddoDistance = 8;
    public float kiddoResetDistance = 2;

    public bool drawDebug;
    public GameObject drawDebugGO;

    public bool waitForKiddo = false;
    public bool superWaitForKiddo = false;

    private void Start()
    {
        drawDebugGO.SetActive( drawDebug );
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localKiddoPosition = transform.InverseTransformPoint( kiddo.position );

        if( targetWaypoint != null )
        {
            transform.LookAt( targetWaypoint.transform );

            if( targetWaypoint.AreWeCloseEnoughForNextWaypoint( transform ) )
            {
                targetWaypoint = targetWaypoint.GetNextWaypoint();
            }
        }

        if( Vector3Helpers.GetHorizontalDistance( mom.position, momTarget.position ) < distanceBeforeWalk && Vector3Helpers.GetHorizontalDistance( dad.position, dadTarget.position ) < distanceBeforeWalk && localKiddoPosition.z > -maxKiddoDistance )
        {
            transform.position += transform.forward * 10 * Time.deltaTime;
        }

        if( !superWaitForKiddo && Vector3Helpers.GetHorizontalDistance( kiddo.position, mom.position ) < 2 )
        {
            superWaitForKiddo = true;
        }
        else if( !superWaitForKiddo && Vector3Helpers.GetHorizontalDistance( kiddo.position, dad.position ) < 2 )
        {
            superWaitForKiddo = true;
        }
        else if( superWaitForKiddo && Vector3Helpers.GetHorizontalDistance( kiddo.position, mom.position ) > 3 && Vector3Helpers.GetHorizontalDistance( kiddo.position, dad.position ) > 3 )
        {
            superWaitForKiddo = false;
        }



        if( !waitForKiddo && ( localKiddoPosition.z <= -maxKiddoDistance || superWaitForKiddo ) )
        {
            waitForKiddo = true;
        }
        else if( waitForKiddo && localKiddoPosition.z > -kiddoResetDistance && !superWaitForKiddo )
        {
            waitForKiddo = false;
        }
    }
}

public static class Vector3Helpers
{
    public static float GetHorizontalDistance( Vector3 pos1, Vector3 pos2 )
    {
        return Vector3.Distance( new Vector3( pos1.x, 0, pos1.z ), new Vector3( pos2.x, 0, pos2.z ) );
    }
}
