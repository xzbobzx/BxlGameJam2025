using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotebookScript : MonoBehaviour
{
    public FirstPersonController daController;
    public TextMeshProUGUI notebookTutorialText;
    public PickStuffUp pickStuffer;
    public Transform notebook;

    public Transform placementHolder;

    private bool allowClose = false;
    private bool dontPlaceAnything = false;
    private bool gameIsOver = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive( false );
    }

    // Update is called once per frame
    void Update()
    {
        placementHolder.localPosition += new Vector3( Input.GetAxis( "Mouse X" ) * daController.mouseSensitivity * 0.01f, Input.GetAxis( "Mouse Y" ) * daController.mouseSensitivity * 0.01f, Input.mouseScrollDelta.y * 0.05f );

        placementHolder.localPosition = new Vector3(
            Mathf.Clamp( placementHolder.localPosition.x, -1, 1.2f ),
            Mathf.Clamp( placementHolder.localPosition.y, -0.7f, 0.7f ),
            Mathf.Clamp( placementHolder.localPosition.z, 3, 3.8f ) );

        if( Input.GetMouseButtonDown( 0 ) && !dontPlaceAnything )
        {
            placementHolder.GetChild( 0 ).transform.parent = notebook;

            notebookTutorialText.text = "Press E to close!";

            allowClose = true;
        }

        if( dontPlaceAnything && !gameIsOver && Input.GetKeyUp( KeyCode.E ) )
        {
            allowClose = true;
        }

        if( Input.GetKeyDown( KeyCode.E ) && allowClose && !gameIsOver )
        {
            DisableNotebook();
        }
    }

    public void EnableNotebook( PickupAbleObject thingToPlace, bool allowCloseAnytime, bool gameEnd = false )
    {
        if( gameEnd )
        {
            notebookTutorialText.text = "Thank you for playing Sunday Walk :)";
        }
        else if( thingToPlace != null )
        {
            notebookTutorialText.text = "Put the thing anywhere :)";

            placementHolder.localPosition = new Vector3( 0, 0, 3.7f );
            thingToPlace.MoveToBookPlacement( placementHolder );
        }
        else
        {
            notebookTutorialText.text = "Press E to close!";
        }

        gameObject.SetActive( true );        
        notebookTutorialText.gameObject.SetActive( true );        

        if( allowCloseAnytime || gameEnd )
        {
            dontPlaceAnything = true;
        }
        else
        {
            dontPlaceAnything = false;
        }

        gameIsOver = gameEnd;

        allowClose = false;
    }

    public void DisableNotebook()
    {
        pickStuffer.PutBookAway();

        gameObject.SetActive( false );
        notebookTutorialText.gameObject.SetActive( false );
    }
}
