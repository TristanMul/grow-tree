using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slicer : MonoBehaviour
{
    [SerializeField] GameObject sliceObjectPrefab;
    [SerializeField] GameObject axeObjectPrefab;
    [SerializeField] GameObject lineObjectPrefab;
    [SerializeField] float sliceSpeed;  // Speed of sliceObject.
    [SerializeField] float minDistance; // Minimum distance betweeen touch point and sliceObject to move sliceObject.
    [SerializeField] float duration;
    [SerializeField] float minSliceDistance;
    [SerializeField] bool addMinDistance;
    Rigidbody2D rb;
    Camera cam;
    public static Slicer instance;
    bool isUpdateTrail = false;
    bool updateLine = false;
    public bool AltSlice;
    GameObject sliceObject;
    GameObject axeObject;
    GameObject sliceLine;
    Vector3 rayStart;
    Vector3 prevTouchPos;
    Transform startPoint, endPoint;
    Transform axe;
    bool canSlice = false;

    private void Awake()
    {
        instance = this;
    }

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
            if (canSlice)
            { 
                if (AltSlice)
                {
                    StartUpdateLine();
                }
                else
                {
                    StartUpdateTrail();
                }
            }
            startGame();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (canSlice)
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
        if (sliceLine)
        {
            Destroy(sliceLine);
        }
        updateLine = true;
        Generator.instance.isSlicing = true;
        sliceLine = Instantiate(lineObjectPrefab, transform);
        startPoint = sliceLine.transform.Find("StartPoint").transform;
        endPoint = sliceLine.transform.Find("EndPoint").transform;
        startPoint.transform.position = GetRayEndPoint();
        axeObject = Instantiate(axeObjectPrefab, startPoint);
        axe = axeObject.transform.Find("RotatingAxe").transform;
    }

    void StopUpdateTrail()
    {
        isUpdateTrail = false;
        Destroy(sliceObject);
    }

    void StopUpdateLine()
    {
        Generator.instance.isSlicing = false;
        updateLine = false;
        
        Vector3 wrongPosition = new Vector3(-6f, 1.8f, 0f);
        if (canSlice && endPoint)
        {
            canSlice = false;
            endPoint.transform.position = GetRayEndPoint();
            StartCoroutine(MoveSlicer());
        }
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
        /*Quaternion rotation*//*axe.rotation =  Quaternion.LookRotation(endPoint.position, Vector3.up);*/
        axe.transform.LookAt(endPoint.position, Vector3.up);
        //axe.eulerAngles = new Vector3(axe.eulerAngles.x, 90, axe.eulerAngles.z);
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
        yield return new WaitForSeconds(2f);
        canSlice = true;
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

    IEnumerator MoveSlicer()
    {
        Time.fixedDeltaTime = 0.01f;
        axeObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
        float elapsed = 0f;
        float distance = Vector3.Distance(startPoint.position, endPoint.position);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (axeObject && (distance > minSliceDistance || !addMinDistance))
            {
                axeObject.GetComponent<Collider>().enabled = true;
                axeObject.transform.position = Vector3.Lerp(axeObject.transform.position, endPoint.position, elapsed / (duration * distance));
            }
            yield return null;
        }
        Time.fixedDeltaTime = 0.02f;
        Destroy(axeObject);
        Destroy(sliceLine);
        canSlice = true;
    }
}
