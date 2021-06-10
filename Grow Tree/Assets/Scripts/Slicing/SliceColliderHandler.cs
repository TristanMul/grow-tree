using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceColliderHandler : MonoBehaviour
{
    [SerializeField] float minSlicingSpeed;

    Rigidbody2D rb;
    Collider2D collider;
    Vector2 previousPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CanSlice();
    }

    void CanSlice(){
        Vector2 currentPos = rb.position;

        float velocity = (currentPos - previousPos).magnitude * Time.deltaTime;
        if(velocity > minSlicingSpeed){
            collider.enabled = true;
        }
        else{
            collider.enabled = false;
        }

        previousPos = currentPos;

    }

    
}
