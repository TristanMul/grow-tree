using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slicer : MonoBehaviour
{
    [SerializeField] GameObject sliceObjectPrefab;

    Rigidbody2D rb;
    Camera cam;
    bool isSlicing = false;
    GameObject sliceObject;
    Vector3 rayStart;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        rayStart = cam.transform.position;
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
        sliceObject.transform.position = GetRayEndPoint();
    }

    void StartSlicing(){
        isSlicing = true;
        sliceObject = Instantiate(sliceObjectPrefab, transform);
    }

    void StopSlicing(){
        isSlicing = false;
        Destroy(sliceObject, 2f);
    }

    void DisableThis()
    {
        this.enabled = false;
    }

    Vector3 GetRayEndPoint(){
        Vector3 planePos = new Vector3 (0, cam.transform.position.y, 0);
        Plane plane = new Plane(Vector3.forward, planePos);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        float distance; // Distance between the start of the ray and the point where it hits the plane.
        plane.Raycast(ray, out distance);
        Vector3 rayEnd = ray.GetPoint(distance);
        Debug.DrawLine(rayStart, rayEnd);
        return rayEnd;
    }
}
