using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    public int patrolDestination;

    public Transform playerTransform;
    public float chaseDistance;
    public float safeDistance;
    private bool isChasing;


    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (!isChasing && distanceToPlayer <= chaseDistance)
            isChasing = true;

        if (isChasing && distanceToPlayer >= safeDistance)
            isChasing = false;

        if (isChasing)
            ChasePlayer();
        else
            Patrol();

    }

    private void Patrol()
    {
        Transform targetPoint = patrolPoints[patrolDestination];

        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < .2f)
        {
            if (patrolDestination == 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                patrolDestination = 1;
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
                patrolDestination = 0;
            }
        }
    }

    private void ChasePlayer()
    {
        if (transform.position.x > playerTransform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }

        if (transform.position.x < playerTransform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }
}
