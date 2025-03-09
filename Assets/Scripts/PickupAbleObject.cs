using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupBehaviorEnum { Push, Pickup };


public class PickupAbleObject : MonoBehaviour
{
    public PickupBehaviorEnum pickupBehavior;

    private float mass;
    private Vector3 originalSize;

    public Rigidbody rb;
    public Collider col;

    private void Awake()
    {
        mass = rb.mass;
        originalSize = transform.localScale;

        int instanceID = gameObject.GetInstanceID();

        if( !PickupDatabase.db.ContainsKey( instanceID ) )
        {
            PickupDatabase.db.Add( instanceID, this );
        }
        else
        {
            PickupDatabase.db[instanceID] = this;
        }
    }

    public void PickUp( Transform newParent )
    {
        Destroy( rb );
        col.enabled = false;

        transform.parent = newParent;
        transform.localScale = transform.localScale * 0.5f;
        transform.localPosition = Vector3.zero;
    }

    public void Throw( Vector3 lookat, bool drop )
    {       
        transform.parent = null;
        transform.localScale = originalSize;

        transform.position += lookat.normalized;

        rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = mass;
        col.enabled = true;

        if( !drop )
        {
            rb.AddForce( lookat * 15, ForceMode.VelocityChange );
        }
    }

    public void MoveToBookPlacement( Transform parent )
    {
        transform.gameObject.layer = 10;
        foreach( Transform child in transform )
        {
            child.gameObject.layer = 10;
        }

        transform.parent = parent;

        transform.localScale = transform.localScale * 0.1f;
        transform.localPosition = Vector3.zero;
        //transform.localRotation = Quaternion.identity;
    }

    public void DestroyBecauseGive()
    {
        Destroy( gameObject );
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public static class PickupDatabase
{
    public static Dictionary<int, PickupAbleObject> db = new();
}