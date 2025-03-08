using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooFarTeleport : MonoBehaviour
{
    public MoveMomDadTarget mover;
    public float teleportDistance = 50;
    public float fadeTime = 3;

    bool isFadingTeleporting = false;

    public Image fader;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( Vector3.Distance( transform.position, mover.mom.transform.position ) > teleportDistance ||  Vector3.Distance( transform.position, mover.dad.transform.position ) > teleportDistance )
        {
            if( !isFadingTeleporting )
            {
                isFadingTeleporting = true;
                StartCoroutine( FadeTeleport() );
            }
        }
    }

    IEnumerator FadeTeleport()
    {
        for( float alpha = 1f; alpha >= 0; alpha -= Time.deltaTime / fadeTime )
        {
            fader.color = new Color( 0, 0, 0, 1 - alpha );

            yield return null;
        }

        transform.position = mover.kidTeleporter.position;

        for( float alpha = 0f; alpha <= 1; alpha += Time.deltaTime / fadeTime )
        {
            fader.color = new Color( 0, 0, 0, 1 - alpha );

            yield return null;
        }

        isFadingTeleporting = false;
    }
}
