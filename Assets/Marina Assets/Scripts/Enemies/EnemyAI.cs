using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Space(5)]
    [Header("——— ENEMY FOLLOW THE PLAYER COMPONENTS.")]
    [SerializeField] private float detectionRadius = 5.0f;
    [SerializeField] private float speed = 3.0f;

    [Space(5)]
    [Header("——— STUN COMPONENTS.")]
    [SerializeField] private float stunDuration = 2.0f;
    [SerializeField] private float stunRadius = 2.0f;

    [Space(5)]
    [Header("——— RETREAT COMPONENTS.")]
    [SerializeField] private float retreatDistance = 3.0f;
    [SerializeField] private float retreatSpeed = 2.0f;

    [Space(5)]
    [Header("——— OBSTACLE AVOIDANCE COMPONENTS.")]
    [SerializeField] private float obstacleDetectionDistance = 1.0f;
    [SerializeField] private float avoidanceForce = 5.0f;
    [SerializeField] private LayerMask obstacleLayer;

    [Space(5)]
    [Header("——— SEPARATION COMPONENTS.")]
    [SerializeField] private float separationDistance = 1.0f;
    [SerializeField] private float separationForce = 1.0f;

    private Transform playerTransform;
    public static bool isStunning = false;

    private EnemyHealth enemyHealth;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        if (enemyHealth.currentHealth >= 0.1f && !EnemyManager.Instance.IsPlayerStunned)
        {
            if (playerTransform != null && !isStunning)
            {
                DetectAndMoveTowardsPlayer();
            }
        }
    }

    private void DetectAndMoveTowardsPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRadius && distanceToPlayer > stunRadius)
        {
            MoveTowardsPlayer();
        }
        else if (distanceToPlayer <= stunRadius && !EnemyManager.Instance.IsPlayerStunned)
        {
            StartCoroutine(StunAndRetreat());
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        if (IsObstacleInPath(direction))
        {
            direction = AvoidObstacles(direction);
        }

        direction += GetSeparationForce();
        direction.Normalize();

        transform.position += direction * speed * Time.deltaTime;
    }

    private bool IsObstacleInPath(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, obstacleDetectionDistance, obstacleLayer);
        return hit.collider != null;
    }

    private Vector3 AvoidObstacles(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, obstacleDetectionDistance, obstacleLayer);
        if (hit.collider != null)
        {
            Vector3 hitNormal = hit.normal;
            Vector3 avoidDirection = Vector3.Cross(hitNormal, Vector3.forward).normalized;
            avoidDirection *= avoidanceForce;
            direction += avoidDirection;
            direction.Normalize();
        }
        return direction;
    }

    private Vector3 GetSeparationForce()
    {
        Vector3 separationForce = Vector3.zero;
        Collider2D[] neighbors = Physics2D.OverlapCircleAll(transform.position, separationDistance);

        foreach (Collider2D neighbor in neighbors)
        {
            if (neighbor.gameObject != gameObject && neighbor.CompareTag("Enemy"))
            {
                Vector3 diff = transform.position - neighbor.transform.position;
                diff.Normalize();
                diff /= Vector3.Distance(transform.position, neighbor.transform.position);
                separationForce += diff;
            }
        }

        return separationForce * this.separationForce;
    }

    private IEnumerator StunAndRetreat()
    {
        isStunning = true;
        SimpleMovement player = playerTransform.GetComponent<SimpleMovement>();
        if (player != null)
        {
            player.Stun(stunDuration);
            EnemyManager.Instance.StunPlayer(stunDuration);
        }

        yield return new WaitForSeconds(stunDuration);

        Vector3 retreatDirection = (transform.position - playerTransform.position).normalized;
        float retreatStartTime = Time.time;
        float retreatDuration = retreatDistance / retreatSpeed;

        while (Time.time < retreatStartTime + retreatDuration)
        {
            if (IsObstacleInPath(retreatDirection))
            {
                retreatDirection = AvoidObstacles(retreatDirection);
            }

            retreatDirection += GetSeparationForce();
            retreatDirection.Normalize();

            transform.position += retreatDirection * retreatSpeed * Time.deltaTime;
            yield return null;
        }

        isStunning = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stunRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, obstacleDetectionDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, separationDistance);
    }
}