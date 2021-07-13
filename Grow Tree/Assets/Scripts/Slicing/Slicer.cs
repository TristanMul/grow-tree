using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slicer : MonoBehaviour
{
    [SerializeField] GameObject sliceObjectPrefab;
    [SerializeField] float sliceSpeed;  // Speed of sliceObject.
    [SerializeField] float minDistance; // Minimum distance betweeen touch point and sliceObject to move sliceObject.
    [SerializeField] GameObject lineObjectPrefab;
    [SerializeField] float duration;
    Rigidbody2D rb;
    Camera cam;
    bool isUpdateTrail = false;
    bool updateLine = false;
    [SerializeField] bool AltSlice;
    GameObject sliceObject;
    GameObject sliceLine;
    Vector3 rayStart;
    Vector3 prevTouchPos;
    Transform startPoint, endPoint;
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
        if (Input.GetMouseButtonDown(0))
        {
            if (AltSlice)
            {
                StartUpdateLine();
            }
            else
            {
                StartUpdateTrail();
            }
            startGame();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (AltSlice)
            {
                StopUpdateLine();
            }
            else
            {
                StopUpdateTrail();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isUpdateTrail)
        {
            MoveSliceObject();
        }
        if (updateLine)
        {
            MoveLine();
        }
    }

    void StartUpdateTrail()
    {
        isUpdateTrail = true;
        sliceObject = Instantiate(sliceObjectPrefab, transform);
        sliceObject.transform.position = GetRayEndPoint();
    }
    void StartUpdateLine()
    {
        if (sliceObject)
        {
            Destroy(sliceObject);
        }
        updateLine = true;
        sliceLine = Instantiate(lineObjectPrefab, transform);
        startPoint = sliceLine.transform.Find("StartPoint").transform;
        endPoint = sliceLine.transform.Find("EndPoint").transform;
        startPoint.position = GetRayEndPoint();
    }
    void StopUpdateTrail()
    {
        isUpdateTrail = false;
        Destroy(sliceObject);
    }
    void StopUpdateLine()
    {
        updateLine = false;
        StartCoroutine(SliceObjects());

    }
    void MoveSliceObject()
    {
        if (!sliceObject) return;

        float swipeDistance = Vector3.Distance(GetRayEndPoint(), sliceObject.transform.position);
        if (swipeDistance > minDistance)
        {
            Time.fixedDeltaTime = 0.01f;
            Vector3 direction = (GetRayEndPoint() - sliceObject.transform.position).normalized;
            sliceObject.GetComponent<Rigidbody>().velocity = direction * sliceSpeed;
        }
        else
        {
            Time.fixedDeltaTime = 0.02f;
            sliceObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    void MoveLine()
    {
        if (!sliceLine) return;
        endPoint.position = GetRayEndPoint();
    }
    void startGame()
    {
        Generator.instance.movingCamera = true;
        StartCoroutine(Startgrowth());
    }

    IEnumerator Startgrowth()
    {
        yield return new WaitForSeconds(1f);
        Generator.instance.started = true;
    }

    public void DisableObject()
    {
        if (sliceObject.gameObject)
            sliceObject.gameObject.SetActive(false);
    }

    Vector3 GetRayEndPoint()
    {
        rayStart = cam.transform.position;

        Vector3 planePos = new Vector3(0, cam.transform.position.y, 0);
        Plane plane = new Plane(Vector3.forward, planePos);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        float distance; // Distance between the start of the ray and the point where it hits the plane.
        plane.Raycast(ray, out distance);
        Vector3 rayEnd = ray.GetPoint(distance);
        Debug.DrawLine(rayStart, rayEnd);
        return rayEnd;
    }

    IEnumerator SliceObjects()
    {
        yield return null;
        sliceObject = Instantiate(sliceObjectPrefab, startPoint);
        StartCoroutine(MoveSlicer());
    }
    IEnumerator MoveSlicer()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            sliceObject.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, elapsed / duration);
            yield return null;
        }
        Destroy(sliceObject);
        Destroy(sliceLine);

    }
}
