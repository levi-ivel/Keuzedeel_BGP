using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
public Transform target1;
public Transform target2; 
private Transform currentTarget; 
public float speed = 3f;
public float stopTime = 2f; 
private bool isWaiting = false;

void Start()
    {
        currentTarget = target1;
    }

void Update()
    {
        if (!isWaiting)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
            {
                StartCoroutine(WaitAtTarget());
            }

            if (transform.position.x < currentTarget.position.x)
            {
                transform.localScale = new Vector3(4f, 4f, 4f); 
            }
            else
            {
                transform.localScale = new Vector3(-4f, 4f, 4f);
            }
        }
    }

IEnumerator WaitAtTarget()
    {
        isWaiting = true;

        if (currentTarget == target1)
        {
            currentTarget = target2;
        }
        else if (currentTarget == target2)
        {
            currentTarget = target1;
        }

        yield return new WaitForSeconds(stopTime);

        isWaiting = false;
    }
}
