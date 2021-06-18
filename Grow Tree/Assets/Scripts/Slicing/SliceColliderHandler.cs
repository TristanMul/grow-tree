using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceColliderHandler : MonoBehaviour
{
    [SerializeField] GameObject sliceTrailPrefab;
    [SerializeField] float minSlicingSpeed;

    GameObject sliceTrail;
    Rigidbody rb;
    Collider collider;
    Vector3 previousPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        CanSlice();
        
    }

    void CanSlice(){
        if(rb.velocity.magnitude > minSlicingSpeed){
            collider.enabled = true;
            //sliceTrail = Instantiate(sliceTrailPrefab,transform);
        }
        else{
            collider.enabled = false;
            if(sliceTrail){
                Destroy(sliceTrail, 1f);
            }
        }

    }

    
}
