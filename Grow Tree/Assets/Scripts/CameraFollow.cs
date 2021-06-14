using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private VectorList activeAttractors;
    [SerializeField] private List<Vector3> _attractorsList;
    public Vector3 cameraOffset;
    Vector3 newPos;
    public float cameraSmoothing;
    private Vector3 velocity;
    private List<Vector3> generator;
    private float interval = 1f;
    private float timeSinceIteration = 0;
    void Start()
    {

    }

    void LateUpdate()
    {
        timeSinceIteration += Time.deltaTime;
        if ( timeSinceIteration> Generator.instance._timeBetweenIterations)
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
            Vector3 centerPos = GetCenterPoint();
            newPos = centerPos + cameraOffset;
            timeSinceIteration = 0;
        }
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

        return new Vector3(bounds.center.x, bounds.center.y + 3f, bounds.center.z);
    }
}
//Dit is een comment

