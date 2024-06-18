using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Movement : MonoBehaviour
{
public float MovementSpeed = 1f;
public float JumpForce = 1f;
private Rigidbody2D rigidbody;


private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

private void Update()
{
var movement = Input.GetAxis("Horizontal");
transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;

    if (Input.GetKey("left ctrl"))
    {
        MovementSpeed = 2;
    }

    else if (Input.GetKey("left shift"))
    {
        MovementSpeed = 10;
    }

    else
    {
        MovementSpeed = 5;
    }

    if (Input.GetButtonDown("Jump") && Mathf.Abs(rigidbody.velocity.y) < 0.001f)
    {
        rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
    }
}

}
