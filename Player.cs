using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Data.Common;

public class Movement : MonoBehaviour
{
//START --Variables Movement--
public float MovementSpeed = 8f;
public float MaxSpeed = 8f;
public float JumpForce = 10f;
public float Acceleration = 10f;
public float Deceleration = 10f;
public float AirControl = 0.5f;
private new Rigidbody2D rigidbody;
private bool isGrounded = false;
//END --Variables Movement--

//START --Variables Death--
public GameObject gameOverUI;
public GameObject MainUI;
private bool isGameFrozen = false;
public int maxLives = 3;
private int lives;
//END --Variables Death--

//START --Variables Consumables--
public Slider foodSlider;
public Slider drinkSlider;
public float decreaseRate = 1f; 
public float increaseAmount = 10f;
private float foodLevel = 100f;
private float drinkLevel = 100f;
//END --Variables Consumables--

//START --Variables Timer--
public float gameDuration = 300f; 
public Text timerText;
private float timer;
//END --Variables Timer--

//START --Variables Lives--
public Text livesText;
private bool lifeLost = false;
//END --Variables Lives--

//START --Variables Power-up--
public GameObject Spear;
private bool hasSpear = false;
private float spearTimer = 0f;
private float spearDuration = 5f; 
//END --Variables Power-up--

//START --Variables NextStage--
public GameObject ClearUI;
//END --Variables NextStage--

private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;

        rigidbody = GetComponent<Rigidbody2D>();

        foodSlider.maxValue = 100f;
        foodSlider.value = foodLevel;
        drinkSlider.maxValue = 100f;
        drinkSlider.value = drinkLevel;

        timer = gameDuration;
        UpdateTimerText();

        lives = PlayerPrefs.GetInt("Lives", maxLives);
        lives = PlayerPrefs.GetInt("Lives", maxLives);
        UpdateLivesText();
    }

private void Update()
    {
        Moving();
        CheckGrounded();
    
        DecreaseSliders();
        UpdateTimerText();

    if (!lifeLost && (timer <= 0 || foodLevel <= 0 || drinkLevel <= 0))
    {
        LoseLife();
    }
    
    if (hasSpear)
    {
        spearTimer -= Time.deltaTime;
        if (spearTimer <= 0)
        {
            hasSpear = false;
            Spear.SetActive(false);
        }
    }
    }

//START --Movement--
private void Moving()
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

        if (movementInput > 0)
        {
            transform.localScale = new Vector3(4f, 4f, 4f); 
        }
        else if (movementInput < 0)
        {
            transform.localScale = new Vector3(-4f, 4f, 4f); 
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
    }


private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, LayerMask.GetMask("Ground"));
        isGrounded = hit.collider != null;
    }
//END --Movement--

//START --Death--
private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Death") && !isGameFrozen)
        {
            LoseLife();
        }
//END --Death--

//START --Next Level--
        if (other.tag == "Next")
        {
            FreezeGame();
            MainUI.SetActive(false);
            ClearUI.SetActive(true);
        }

//END --Next Level--

//START --Consumables--
        if (other.tag == "Food")
        {
            IncreaseFoodLevel();
            Destroy(other.gameObject); 
        }

        if (other.tag == "Drink")
        {
            IncreaseDrinkLevel();
            Destroy(other.gameObject);
        }
    
        if (other.tag == "Power")
        {
            Spear.SetActive(true);
            hasSpear = true;
            spearTimer = spearDuration;
            Destroy(other.gameObject);
        }
//END --Consumables--

//START --Enemy--
        if (other.tag == "Enemy")
        {
            if (hasSpear)
            {
                Destroy(other.gameObject); 
            }
            else
            {
                LoseLife(); 
            }
        }
//END --Enemy--
}

//START --Game Over UI--
private void FreezeGame()
    {
        isGameFrozen = true;
        Time.timeScale = 0f; 
    }

private void ActivateGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

private void DeactivateMainUI()
    {
        MainUI.SetActive(false);
    }

public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        isGameFrozen = false;
        gameOverUI.SetActive(false);
        lifeLost = false;
    }

public void QuitGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        isGameFrozen = false;
        gameOverUI.SetActive(false);
    }
//END --Game Over UI--

//START --Level Clear UI--
public void BackLevelClear()
    {
        Time.timeScale = 1f;
        isGameFrozen = false;
        SceneManager.LoadScene(0);
        
    }

public void Continue()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1f;
            isGameFrozen = false;
            SceneManager.LoadScene(nextSceneIndex);
        }

        else
        {
            Time.timeScale = 1f;
            isGameFrozen = false;
            SceneManager.LoadScene(0);
        }
    }

//END --Level Clear UI--

//START --Consumable Sliders--
void DecreaseSliders()
    {
        foodLevel -= decreaseRate * Time.deltaTime;
        drinkLevel -= decreaseRate * Time.deltaTime;

        foodLevel = Mathf.Clamp(foodLevel, 0f, 100f);
        drinkLevel = Mathf.Clamp(drinkLevel, 0f, 100f);

        foodSlider.value = foodLevel;
        drinkSlider.value = drinkLevel;
    }

void IncreaseFoodLevel()
    {
        foodLevel += increaseAmount;
        foodLevel = Mathf.Clamp(foodLevel, 0f, 100f); 
        foodSlider.value = foodLevel;
    }

void IncreaseDrinkLevel()
    {
        drinkLevel += increaseAmount;
        drinkLevel = Mathf.Clamp(drinkLevel, 0f, 100f); 
        drinkSlider.value = drinkLevel;
    }
//END --Consumable Sliders--

//START --Timer--
void UpdateTimerText()
    {
        timer -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
//END --Timer--

//START --Lives--
private void LoseLife()
    {
        if (!lifeLost)
        {
            lives--;
            PlayerPrefs.SetInt("Lives", lives);
            UpdateLivesText();
            lifeLost = true;

            if (lives <= 0)
            {
                GoToGameOver();
            }
            else
            {
                FreezeGame();
                DeactivateMainUI();
                ActivateGameOverUI();
            }
        }
    }
private void UpdateLivesText()
    {
        livesText.text = "Lives: " + lives;
    }

private void GoToGameOver()
    {
        PlayerPrefs.SetInt("Lives", maxLives);
        SceneManager.LoadScene(0);
    }
//END --Lives--
}
