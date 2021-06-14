using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private VectorList activeAttractors;
    [SerializeField] private List<Vector3> _attractorsList;
    public Vector3 cameraOffset;
    public float cameraSmoothing;
    private Vector3 velocity;
    private List<Vector3> generator;
    private float interval = 1f;

    void Start()
    {
    }
    
    void LateUpdate()
    {
        _attractorsList = activeAttractors.CurrentObjectList;

        if (_attractorsList.Count == 0)
            return;
        Vector3 centerPos = GetCenterPoint();
        Vector3 newPos = centerPos + cameraOffset;
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, cameraSmoothing);
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

        return bounds.center;
    }
}
//Dit is een comment

