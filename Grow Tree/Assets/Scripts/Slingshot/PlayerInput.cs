using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private FloatValue horizontalSpeed;
    [SerializeField] private float horizontalSpeedCap;
    [SerializeField] private float dragSpeed;

    private Vector3 previousMousePosition;
    private Vector3 currentMousePosition;
    
    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 mousePosition;
        float deltaMousePosition;
        float sign;
        float speed;

        if (Input.GetMouseButtonDown(0))
        {
            mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z + 2f);
            previousMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z + 2f);
            currentMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            deltaMousePosition = Mathf.Abs(previousMousePosition.x - currentMousePosition.x);
            sign = Mathf.Sign(previousMousePosition.x - currentMousePosition.x);

            speed = deltaMousePosition * dragSpeed * -sign;
            speed = Mathf.Clamp(speed, -horizontalSpeedCap, horizontalSpeedCap);
            horizontalSpeed.Value = speed;

            previousMousePosition = currentMousePosition;
        }
        else
        {
            horizontalSpeed.Value = 0f;
        }

    }
}
