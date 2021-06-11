using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceColliderHandler : MonoBehaviour
{
    [SerializeField] float minSlicingSpeed;

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
        // CanSlice();
    }

    void CanSlice(){
        Vector3 currentPos = rb.position;

        Vector3 velocity = (currentPos - previousPos) / Time.deltaTime;
        Debug.Log(velocity.magnitude);
        if(velocity.magnitude > minSlicingSpeed){
            collider.enabled = true;
        }
        else{
            collider.enabled = false;
        }

        previousPos = currentPos;

    }

    
}
