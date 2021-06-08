using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairMovement : MonoBehaviour
{
    [SerializeField] private FloatValue verticalValue;
    [SerializeField] private Vector3Value playerPosition;
    [SerializeField] private bool canMove;
    [SerializeField] private float sensitivity;
    private PlayerInput playerInput;
    private Rigidbody rigidBody;
    private Vector3 myPos;
    private SpriteRenderer _renderer;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        _renderer = GetComponent<SpriteRenderer>();
        myPos = GetComponent<Transform>().position;
        rigidBody = GetComponent<Rigidbody>();
        playerPosition.Value = transform.position;
    }

    private void Start()
    {
    }

    private void Update()
    {
        transform.position = new Vector3(transform.parent.position.x, transform.position.y, transform.position.z);
        Move();
    }

    private void Move()
    {
        if (!canMove)
        {
            _renderer.enabled = false;
            rigidBody.velocity = Vector3.zero;
            return;
        }

        if (playerInput.isClicking)
        {
            _renderer.enabled = true;
            float clampYPosition;
            rigidBody.velocity = new Vector3(0f, verticalValue.Value * sensitivity, 0f);
            clampYPosition = Mathf.Clamp(transform.position.y, 0.5f, 6.9f);
            transform.position = new Vector3(transform.position.x, clampYPosition, transform.position.z);
        }
        else
        {
            _renderer.enabled = false;
            transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
        }
    }

    public void PlayerCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
}
