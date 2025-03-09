using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPickupSpawner : MonoBehaviour
{
    [Header( "Spawn Settings - Add Objects To ListOfThings" )]
    public List<PickupAbleObject> listOfPossibleThingsToSpawn = new();
    public float spawnRadius = 10;
    public float amountOfThingsToSpawn = 3;

    [Header( "Dont Touch" )]
    public LayerMask mask;
    public GameObject debug;

    // Start is called before the first frame update
    void Start()
    {
        debug.SetActive( false );

        if( listOfPossibleThingsToSpawn.Count == 0 )
        {
            Debug.LogError( "listOfPossibleThingsToSpawn.Count = 0!!" );
            return;
        }

        for( int i = 0; i < amountOfThingsToSpawn; i++ )
        {
            int randomObject = Random.Range( 0, listOfPossibleThingsToSpawn.Count );

            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;

            Vector3 randomRaycastPosition = transform.position + new Vector3( randomCircle.x, 0, randomCircle.y );

            Ray newRay = new Ray( randomRaycastPosition, -Vector3.up );
            RaycastHit outHit;

            if( Physics.Raycast( newRay, out outHit, 1000f, mask  ) )
            {
                PickupAbleObject newObject = Instantiate( listOfPossibleThingsToSpawn[randomObject] );
                newObject.transform.position = outHit.point + Vector3.up;
                newObject.transform.Rotate( Vector3.up, Random.Range( 0, 360 ) );
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere( transform.position, spawnRadius );
        Gizmos.DrawLine( transform.position + new Vector3( spawnRadius, 0, 0 ), transform.position + new Vector3( -spawnRadius, 0, 0 ) );
        Gizmos.DrawLine( transform.position + new Vector3( 0, 0, spawnRadius ), transform.position + new Vector3( 0, 0, -spawnRadius ) );
    }
}
