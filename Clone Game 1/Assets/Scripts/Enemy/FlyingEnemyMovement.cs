using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class FlyingEnemyMovement : MonoBehaviour
{
    private enum EnemyState
    {
        Patrol,
        Chase,
        Return
    }

    [Header("Patrol")]
    public Transform patrolCenter;
    public float patrolRadius = 5f;
    public float moveSpeed = 2f;
    public float waitTime = 1f;

    [Header("Detection")]
    public Transform playerTransform;
    public float chaseDistance;
    public float loseDistance;

    private EnemyState currentState = EnemyState.Patrol;

    private Vector2 patrolTarget;
    private bool waiting;

    [SerializeField] private SpriteRenderer sprite;

    void Start()
    {
        PickNewPatrolPoint();
    }

    void Update()
    {
        float distanceFromCenter = Vector2.Distance(playerTransform.position, patrolCenter.position);
        float distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

        switch (currentState)
        {
            case EnemyState.Patrol:

                if (distanceFromCenter <= chaseDistance)
                    currentState = EnemyState.Chase;
                else
                    Patrol();

                break;

            case EnemyState.Chase:

                if (distanceToPlayer >= loseDistance)
                    currentState = EnemyState.Return;
                else
                    ChasePlayer();

                break;

            case EnemyState.Return:

                ReturnToPatrol();

                break;
        }
    }

    private void Patrol()
    {
        if (waiting)
            return;

        transform.position = Vector2.MoveTowards(transform.position, patrolTarget, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, patrolTarget) < 0.1f && !waiting)
        {
            StartCoroutine(WaitThenChooseNewPoint());
        }
    }

    private void ReturnToPatrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, patrolCenter.position, moveSpeed * Time.deltaTime);

        FaceDirection(patrolTarget);

        if (Vector2.Distance(transform.position, patrolCenter.position) < 0.1f)
        {
            PickNewPatrolPoint();
            currentState = EnemyState.Patrol;
        }
    }

    private void PickNewPatrolPoint()
    {
        patrolTarget = (Vector2)patrolCenter.position + Random.insideUnitCircle * patrolRadius;
    }

    private IEnumerator WaitThenChooseNewPoint()
    {
        waiting = true;

        yield return new WaitForSeconds(waitTime);

        PickNewPatrolPoint();

        waiting = false;
    }

    private void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

    }

    private void FaceDirection(Vector2 target)
    {
        if (target.x < transform.position.x)
            sprite.flipX = false;
        else
            sprite.flipX = true;
    }

    private void OnDrawGizmosSelected()
    {

        if (patrolCenter == null)
            return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(patrolCenter.position, patrolRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(patrolCenter.position, chaseDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(patrolCenter.position, loseDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(patrolTarget, 0.15f);
    }

}
