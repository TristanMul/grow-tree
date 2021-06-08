using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairMovement : MonoBehaviour
{
    [SerializeField] private FloatValue verticalValue;
    [SerializeField] private Vector3Value playerPosition;
    [SerializeField] private bool canMove;
    [SerializeField] private float sensitivity;
    private Rigidbody rigidBody;
    private Vector3 myPos;

    // Start is called before the first frame update
    void Start()
    {
        myPos = GetComponent<Transform>().position;
        rigidBody = GetComponent<Rigidbody>();
        playerPosition.Value = transform.position;
    }

    private void Update()
    {
        //playerPosition.Value = transform.position;
        transform.position = new Vector3(transform.parent.position.x, transform.position.y, transform.position.z);
        //myPos.x = transform.parent.position.x;
        Move();
    }

    private void Move()
    {
        if (!canMove)
        {
            rigidBody.velocity = Vector3.zero;
            return;
        }

        float clampYPosition;
        rigidBody.velocity = new Vector3(0f ,verticalValue.Value * sensitivity , 0f);
        clampYPosition = Mathf.Clamp(transform.position.y, 0.5f, 6.9f);
        transform.position = new Vector3(transform.position.x, clampYPosition, transform.position.z);
    }

    public void PlayerCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
}
