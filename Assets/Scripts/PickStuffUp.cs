using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickStuffUp : MonoBehaviour
{
    public Transform cameraTransform;
    public LayerMask pickupObjectsMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray supercoolray = new Ray( cameraTransform.position, cameraTransform.forward );
        RaycastHit hit;

        if( Physics.Raycast( supercoolray, out hit, 5, pickupObjectsMask ) )
        {
            Debug.Log( "hit " + Time.time );

            if( Input.GetMouseButtonDown( 0 ) )
            {
                if( hit.rigidbody != null )
                {
                    hit.rigidbody.AddForce( cameraTransform.forward * 10, ForceMode.Acceleration );
                }
            }
        }
    }

    private void FixedUpdate()
    {

    }
}
