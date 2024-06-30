using UnityEngine;

public class Enemy : MonoBehaviour
{
public Transform target1;
public Transform target2; 
private Transform currentTarget; 
private float speed = 3f;

void Start()
    {
        currentTarget = target1;
    }

void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            if (currentTarget == target1)
            {
                currentTarget = target2;
            }
            else if (currentTarget == target2)
            {
                currentTarget = target1;
            }
        }

        if (transform.position.x < currentTarget.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f); 
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
