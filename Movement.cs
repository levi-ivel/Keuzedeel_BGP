using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float MovementSpeed = 8f;
    public float MaxSpeed = 8f;
    public float JumpForce = 10f;
    public float Acceleration = 10f;
    public float Deceleration = 10f;
    public float AirControl = 0.5f;
    private new Rigidbody2D rigidbody;
    private bool isGrounded = false;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var movementInput = Input.GetAxis("Horizontal");
        var currentSpeed = rigidbody.velocity.x;

        float targetSpeed = movementInput * MovementSpeed;
        float speedDifference = targetSpeed - currentSpeed;
        float movement = (isGrounded ? Acceleration : AirControl) * speedDifference * Time.deltaTime;

        rigidbody.velocity = new Vector2(currentSpeed + movement, rigidbody.velocity.y);

        if (Mathf.Abs(rigidbody.velocity.x) > MaxSpeed)
        {
            rigidbody.velocity = new Vector2(Mathf.Sign(rigidbody.velocity.x) * MaxSpeed, rigidbody.velocity.y);
        }

        if (movementInput == 0 && isGrounded)
        {
            rigidbody.velocity = new Vector2(Mathf.MoveTowards(rigidbody.velocity.x, 0, Deceleration * Time.deltaTime), rigidbody.velocity.y);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            MovementSpeed = 10;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            MovementSpeed = 2;
        }
        else
        {
            MovementSpeed = 6;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        }

        CheckGrounded();
    }

    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, LayerMask.GetMask("Ground"));
        isGrounded = hit.collider != null;
    }
}
