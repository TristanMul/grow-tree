using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    [SerializeField] private float zLevel;
    [SerializeField] private Transform startPosition, endPosition;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
    }
    private void Update()
    {
        lr.SetPosition(0, new Vector3(startPosition.position.x,startPosition.position.y, startPosition.position.z - zLevel));
        lr.SetPosition(1, new Vector3(endPosition.position.x,endPosition.position.y,endPosition.position.z - zLevel));
    }
}
