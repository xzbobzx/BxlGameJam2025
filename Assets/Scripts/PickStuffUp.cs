using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class PickStuffUp : MonoBehaviour
{
    public MoveMomDadTarget mover;
    public FirstPersonController firstPersonScript;
    public SelectionOutlineController outliner;
    public Transform cameraTransform;
    public LayerMask pickupObjectsMask;
    public LayerMask targetParentMask;
    public TextMeshProUGUI pickupText;
    public TextMeshProUGUI throwText;

    public Transform pickupHoldLocation;

    public bool carryingSomething = false;
    PickupAbleObject currentPickobject;

    public NotebookScript notebook;

    bool hittingSomething = false;

    // Start is called before the first frame update
    void Start()
    {
        pickupText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown( KeyCode.Escape ) && !Application.isEditor )
        {
            Application.Quit();
        }

        Ray supercoolray = new Ray( cameraTransform.position, cameraTransform.forward );
        RaycastHit hit;

        hittingSomething = false;
        bool dontallowOpenBookRandomly = false;
        bool dontallowPutInBook = false;

        if( mover.endOfTheLine && ( Vector3Helpers.GetHorizontalDistance( transform.position, mover.mom.position ) < 3 || Vector3Helpers.GetHorizontalDistance( transform.position, mover.dad.position ) < 3 ) )
        {
            OpenBookEnd();

            return;
        }

        if( !carryingSomething && Physics.Raycast( supercoolray, out hit, PickUpRaycast.GetDistance(), pickupObjectsMask ) )
        {
            if( PickupDatabase.db.ContainsKey( hit.collider.gameObject.GetInstanceID() ) )
            {
                hittingSomething = true;
                
                currentPickobject = PickupDatabase.db[hit.collider.gameObject.GetInstanceID()];

                if( currentPickobject.pickupBehavior == PickupBehaviorEnum.Push )
                {
                    pickupText.text = "Push";
                }
                else if( currentPickobject.pickupBehavior == PickupBehaviorEnum.Pickup )
                {
                    pickupText.text = "Pick up";
                }

                if( Input.GetMouseButtonDown( 0 ) )
                {
                    if( currentPickobject.pickupBehavior == PickupBehaviorEnum.Push )
                    {
                        if( hit.rigidbody != null )
                        {
                            Vector3 forwardVector = cameraTransform.forward;
                            forwardVector.Scale( new Vector3( 1, 0, 1 ) );

                            hit.rigidbody.AddForce( forwardVector * 500, ForceMode.Acceleration );
                        }
                    }
                    else if( currentPickobject.pickupBehavior == PickupBehaviorEnum.Pickup )
                    {
                        carryingSomething = true;
                        currentPickobject.PickUp( pickupHoldLocation );
                    }

                    outliner.enabled = false;
                    outliner.ClearTarget();
                }

                dontallowOpenBookRandomly = true;
            }
            else
            {
                currentPickobject = null;

                pickupText.text = "";
            }

            throwText.gameObject.SetActive( false );
        }
        else
        {
            bool hoveringOverParent = false;

            if( Physics.Raycast( supercoolray, out hit, PickUpRaycast.GetDistance(), targetParentMask ) )
            {
                if( hit.collider.gameObject.CompareTag( "Parent" ) )
                {
                    hoveringOverParent = true;

                    hittingSomething = true;

                    dontallowPutInBook = true;
                }
            }

            if( carryingSomething && hoveringOverParent && pickupText.text == "" )
            {
                pickupText.text = "Give";
                throwText.gameObject.SetActive( false );
            }
            else if( !hoveringOverParent )
            {
                pickupText.text = "";

                if( carryingSomething )
                {
                    throwText.gameObject.SetActive( true );
                }
                else
                {
                    throwText.gameObject.SetActive( false );
                }                
            }

            if( Input.GetMouseButtonDown( 0 ) && carryingSomething )
            {
                if( hoveringOverParent )
                {
                    currentPickobject.DestroyBecauseGive();

                    SingleParentController parent = hit.collider.gameObject.GetComponent<SingleParentController>();

                    if( parent != null )
                    {
                        parent.TriggerThankYou();
                    }
                }
                else
                {
                    currentPickobject.Throw( cameraTransform.forward, true );
                }

                carryingSomething = false;
            }

            if( !dontallowPutInBook )
            {
                if( carryingSomething && Input.GetKeyDown( KeyCode.F ) )
                {
                    currentPickobject.Throw( cameraTransform.forward, false );

                    carryingSomething = false;

                    outliner.enabled = true;
                    outliner.SetTarget();
                }
                else if( carryingSomething && Input.GetKeyDown( KeyCode.E ) )
                {
                    OpenBook( currentPickobject );
                    dontallowOpenBookRandomly = true;

                    currentPickobject = null;
                    carryingSomething = false;
                }
            }         
        }

        if( !carryingSomething && !dontallowOpenBookRandomly && Input.GetKeyDown( KeyCode.E ) )
        {
            OpenBookWithoutAnything();
        }

        if( Input.GetMouseButtonUp( 0 ) && !carryingSomething )
        {
            outliner.enabled = true;
            outliner.SetTarget();
        }
    }

    void TakeScreenshot()
    {
        string path = System.Environment.GetFolderPath( System.Environment.SpecialFolder.Desktop );

        var now = System.DateTime.Now;

        path = Path.Combine( path, "SundayWalk " + Random.Range(1000, 10000) + ".png" );

        ScreenCapture.CaptureScreenshot( path );

        Debug.Log( "Take Screenshot!" );
    }

    public void OpenBookEnd()
    {
        firstPersonScript.DisableFirstPersonController();

        throwText.gameObject.SetActive( false );

        notebook.EnableNotebook( null, false, true );

        TakeScreenshot();

        enabled = false;
    }

    public void OpenBook( PickupAbleObject thing )
    {
        firstPersonScript.DisableFirstPersonController();

        throwText.gameObject.SetActive( false );

        notebook.EnableNotebook( thing, false );

        enabled = false;
    }

    public void OpenBookWithoutAnything()
    {
        firstPersonScript.DisableFirstPersonController();

        throwText.gameObject.SetActive( false );

        notebook.EnableNotebook( null, true );

        enabled = false;
    }

    public void PutBookAway()
    {
        firstPersonScript.enabled = true;
        firstPersonScript.Start();

        outliner.enabled = true;
        outliner.SetTarget();

        enabled = true;
    }
}

public static class PickUpRaycast
{
    public static float GetDistance()
    {
        return 5;
    }
}
