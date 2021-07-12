using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    [SerializeField] private Transform startPosition, endPosition;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
    }
    private void Update()
    {
        lr.SetPosition(0, startPosition.position);
        lr.SetPosition(1, endPosition.position);
    }
}
