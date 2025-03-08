using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint nextWaypoint;
    public GameObject waypointDebug;

    public void Start()
    {
        waypointDebug.SetActive( false );
    }

    public bool AreWeCloseEnoughForNextWaypoint( Transform me )
    {
        if( Vector3Helpers.GetHorizontalDistance( me.transform.position, transform.position ) < 3 )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Waypoint GetNextWaypoint()
    {
        return nextWaypoint;
    }

    private void OnDrawGizmos()
    {
        if( nextWaypoint != null )
        {
            Gizmos.DrawLine( transform.position, nextWaypoint.transform.position );
        }
    }
}
