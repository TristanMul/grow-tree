using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slicer : MonoBehaviour
{
    [SerializeField] GameObject sliceTrailPrefab;

    Rigidbody2D rb;
    Camera cam;
    bool isSlicing = false;
    GameObject sliceTrail;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            StartSlicing();
        }
        else if(Input.GetMouseButtonUp(0)){
            StopSlicing();
        }

        if(isSlicing){
            Slicing();
        }
    }

    void Slicing(){
        Debug.Log("slice");
        rb.position = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void StartSlicing(){
        Debug.Log("start");
        isSlicing = true;
        sliceTrail = Instantiate(sliceTrailPrefab, transform);
    }

    void StopSlicing(){
        Debug.Log("stop");
        isSlicing = false;
        sliceTrail.transform.parent = null;
        Destroy(sliceTrail, 1f);
    }
}
