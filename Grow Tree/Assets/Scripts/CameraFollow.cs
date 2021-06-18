﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private VectorList activeAttractors;
    [SerializeField] private List<Vector3> _attractorsList;
    [SerializeField] private Transform finishPosition;
    public Vector3 cameraOffset;
    Vector3 newPos;
    public float cameraSmoothing;
    private Vector3 velocity;
    private List<Vector3> generator;
    private float timeSinceIteration = 1;
    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;
    private Camera cam;
    private Highlighter radar;
    private bool finished;

    void Awake()
    {
        cam = GetComponent<Camera>();
        radar = GameObject.Find("Canvas").GetComponent<Highlighter>();
        if(finishPosition == null)
        {
            finishPosition = GameObject.Find("FinishCamera").GetComponent<Transform>();
        }
    }

    void LateUpdate()
    {
        if (radar.highlights.Count == 0)
        {
            _attractorsList.Clear();
            foreach (Generator.Branch extremedy in Generator.instance._extremities)
            {
                if (extremedy._canGrow)
                {
                    _attractorsList.Add(extremedy._start);
                }
            }

            if (_attractorsList.Count == 0)
                return;
            MoveCamera();
            ZoomCamera();
            timeSinceIteration = 0;
        }else
        {
            velocity = Vector3.zero;
        }

        if(cam.velocity == Vector3.zero && !Generator.instance.BranchHitBarrier())
        {
            print("dead");
        }

        if (finished)
        {
            //transform.position = Vector3.SmoothDamp(transform.position, finishPosition.position, ref velocity, cameraSmoothing);
            transform.position = Vector3.Lerp(transform.position, finishPosition.position, Time.deltaTime);
        }
    }
    //}

    void ZoomCamera()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime); ;
    }

    void MoveCamera()
    {
        Vector3 centerPos = GetCenterPoint();
        newPos = centerPos + cameraOffset;

        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, cameraSmoothing);
    }

    public void MoveToFinish()
    {
        finished = true;
    }

    Vector3 GetCenterPoint()
    {
        if (_attractorsList.Count == 1)
        {
            return _attractorsList[0];
        }

        var bounds = new Bounds(_attractorsList[0], Vector3.zero);
        for (int i = 0; i < _attractorsList.Count; i++)
        {
            bounds.Encapsulate(_attractorsList[i]);
        }

        return new Vector3(bounds.center.x, bounds.center.y + 3f, bounds.center.z);
    }

    float GreatestDistance()
    {
        var bounds = new Bounds(_attractorsList[0], Vector3.zero);
        for (int i = 0; i < _attractorsList.Count; i++)
        {
            bounds.Encapsulate(_attractorsList[i]);
        }

        return bounds.size.x;
    }
}
