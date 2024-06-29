using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
public GameObject gameOverUI;
private bool isGameFrozen = false;

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
            MovementSpeed = 7;
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

private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Death") && !isGameFrozen)
    {
        FreezeGame();
        ActivateGameOverUI();
    }
}

private void FreezeGame()
    {
        isGameFrozen = true;
        Time.timeScale = 0f; 
    }

private void ActivateGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        isGameFrozen = false;
        gameOverUI.SetActive(false);
    }

public void QuitGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        isGameFrozen = false;
        gameOverUI.SetActive(false);
    }
}
