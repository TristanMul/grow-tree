using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slicer : MonoBehaviour
{
    [SerializeField] GameObject sliceTrailPrefab;

    Rigidbody2D rb;
    Camera cam;
    bool isUpdateTrail = false;
    GameObject sliceTrail;
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
            StartUpdateTrail();
        }
        else if(Input.GetMouseButtonUp(0)){
            StopUpdateTrail();
        }

        if(isUpdateTrail){
            UpdateTrail();
            ShootRay();
        }
    }

    void UpdateTrail(){
        sliceTrail.transform.position = GetRayEndPoint();
    }

    void StartUpdateTrail(){
        isUpdateTrail = true;
        sliceTrail = Instantiate(sliceTrailPrefab, transform);
    }

    void StopUpdateTrail(){
        isUpdateTrail = false;
        Destroy(sliceTrail, 2f);
    }

    Vector3 GetRayEndPoint(){
        rayStart = cam.transform.position;

        Vector3 planePos = new Vector3 (0, cam.transform.position.y, 0);
        Plane plane = new Plane(Vector3.forward, planePos);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        float distance; // Distance between the start of the ray and the point where it hits the plane.
        plane.Raycast(ray, out distance);
        Vector3 rayEnd = ray.GetPoint(distance);
        // Debug.DrawLine(rayStart, rayEnd);
        return rayEnd;
    }

    void ShootRay(){
        rayStart = cam.transform.position;

        Vector3 planePos = new Vector3 (0, cam.transform.position.y, 0);
        Plane plane = new Plane(Vector3.forward, planePos);

        float range = 0 - cam.transform.position.z;

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, range)){
            if(hit.collider.CompareTag("Branch")){
                hit.collider.GetComponent<CheckBranch>().CutBranch();
            }
        }

    }
}
