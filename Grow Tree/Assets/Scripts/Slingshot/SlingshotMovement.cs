using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotMovement : MonoBehaviour
{
    [SerializeField] private FloatValue horizontalSpeed;
    [SerializeField] private Vector3Value playerPosition;
    [SerializeField] private bool canMove;
    [SerializeField] private float sensitivity;
    private Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerPosition.Value = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        playerPosition.Value = transform.position;
        Move();
    }

    private void Move()
    {
        if (!canMove)
        {
            rigidBody.velocity = Vector3.zero;
            return;
        }

        float clampXPosition;
        rigidBody.velocity = new Vector3(horizontalSpeed.Value * sensitivity, 0f, 0f);
        clampXPosition = Mathf.Clamp(transform.position.x, 0.5f, 6.9f);
        transform.position = new Vector3(clampXPosition, transform.position.y, transform.position.z);
    }

    public void PlayerCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
}
