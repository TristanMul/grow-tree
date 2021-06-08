using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private FloatValue horizontalSpeed;
    [SerializeField] private FloatValue verticalSpeed;
    [SerializeField] private float horizontalSpeedCap;
    [SerializeField] private float verticalSpeedCap;
    [SerializeField] private float dragSpeed;

    private Vector3 previousMousePosition;
    private Vector3 currentMousePosition;
    private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }

    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 mousePosition;
        Vector2 _speed;
        Vector2 _deltaMousePosition;
        Vector2 sign;

        if (Input.GetMouseButtonDown(0))
        {
            mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.transform.position.z + 2f);
            previousMousePosition = camera.ScreenToWorldPoint(mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.transform.position.z + 2f);
            currentMousePosition = camera.ScreenToWorldPoint(mousePosition);

            _deltaMousePosition.x = Mathf.Abs(previousMousePosition.x - currentMousePosition.x);
            sign.x = Mathf.Sign(previousMousePosition.x - currentMousePosition.x);

            _speed.x = _deltaMousePosition.x * dragSpeed * -sign.x;
            _speed.x = Mathf.Clamp(_speed.x, -horizontalSpeedCap, horizontalSpeedCap);
            horizontalSpeed.Value = _speed.x;

            _deltaMousePosition.y = Mathf.Abs(previousMousePosition.y - currentMousePosition.y);
            sign.y = Mathf.Sign(previousMousePosition.y - currentMousePosition.y);

            _speed.y = _deltaMousePosition.y * dragSpeed * -sign.y;
            _speed.y = Mathf.Clamp(_speed.y, -verticalSpeedCap, verticalSpeedCap);
            verticalSpeed.Value = _speed.y;

            previousMousePosition = currentMousePosition;
        }
        else
        {
            horizontalSpeed.Value = 0f;
        }

    }
}
