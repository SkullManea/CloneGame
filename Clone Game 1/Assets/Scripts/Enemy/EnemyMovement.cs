using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEditor;
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
    public SpriteRenderer sprite;

    public Collider2D enemyCollider;
    public LayerMask wallLayer;


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
                sprite.flipX = true;
                patrolDestination = 1;
            }
            else
            {
                sprite.flipX = false;
                patrolDestination = 0;
            }
        }
    }

    private void ChasePlayer()
    {
        float leftLimit = Mathf.Min(patrolPoints[0].position.x, patrolPoints[1].position.x);
        float rightLimit = Mathf.Max(patrolPoints[0].position.x, patrolPoints[1].position.x);

        if (transform.position.x <= leftLimit || transform.position.x >= rightLimit)
        {
            isChasing = false;
            return;
        }

        if (transform.position.x > playerTransform.position.x)
        {
            sprite.flipX = false;

            Vector2 rayStart = new Vector2(enemyCollider.bounds.min.x, enemyCollider.bounds.center.y + 0.3f);

            Debug.DrawRay(rayStart, Vector2.left * 0.1f, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.left, 0.1f, wallLayer);

            if (hit.collider == null)
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }

        if (transform.position.x < playerTransform.position.x)
        {
            sprite.flipX = true;

            Vector2 rayStart = new Vector2(enemyCollider.bounds.max.x, enemyCollider.bounds.center.y + 0.3f);

            Debug.DrawRay(rayStart, Vector2.right * 0.1f, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.right, 0.1f, wallLayer);

            if (hit.collider == null)
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }
}
