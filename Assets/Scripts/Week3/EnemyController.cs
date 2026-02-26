using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase
    }

    [Header("Identidade")]
    public string nameID;

    [Header("Enemy AI")]
    public EnemyState currentState;

    [Header("Movement")]
    public float speed = 2.0f;
    public Transform[] patrolPoints;
    int currentPatrolIndex = 0;

    [Header("Wait")]
    public float waitTime = 2.0f;
    private float currentWaitTime = 0f;
    private float idleTime = 0f;
    private float currentIdleTime = 0f;

    [Header("Sensors")]
    public float visionRadius = 5.0f;
    public float chaseRadius = 8.0f;
    public float attackDistance = 1.0f;

    [Header("Combat & ID")]
    public string uniqueID;
    public GameObject arenaPrefab;

    Animator anim;
    bool facingRight = true;
    Transform playerPos;

    // --- Movement ---
    Rigidbody2D rb;
    Vector2 movementDirection;
    float currentSpeedMultiplier = 1f;

    int enemyLayer;
    int obstacleLayer;

    private void Start()
    {
        if (GlobalData.defeatedEnemies.Contains(uniqueID))
        {
            gameObject.SetActive(false);
            return;
        }

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        rb.freezeRotation = true;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerPos = player.transform;
        }

        currentState = EnemyState.Idle;
        idleTime = waitTime;

        // Layer setup
        enemyLayer = LayerMask.NameToLayer("Enemy");
        obstacleLayer = LayerMask.NameToLayer("Obstacle");

        // Ignore obstacle collision by default (Patrol mode)
        Physics2D.IgnoreLayerCollision(enemyLayer, obstacleLayer, false);
    }

    private void Update()
    {
        if (playerPos == null) return;

        float distance = Vector2.Distance(transform.position,
            playerPos.position);

        switch (currentState)
        {
            case EnemyState.Idle:
                anim.SetBool("Walk", false);
                movementDirection = Vector2.zero;

                currentIdleTime += Time.deltaTime;
                if (currentIdleTime > idleTime)
                {
                    currentState = EnemyState.Patrol;
                    currentIdleTime = 0f;
                }
                break;

            case EnemyState.Patrol:

                if (distance <= visionRadius)
                {
                    currentState = EnemyState.Chase;
                    Physics2D.IgnoreLayerCollision(enemyLayer, obstacleLayer, false);
                }

                Patrol();
                break;

            case EnemyState.Chase:

                if (distance > chaseRadius)
                {
                    currentState = EnemyState.Patrol;
                    Physics2D.IgnoreLayerCollision(enemyLayer, obstacleLayer, true);
                }

                if (distance <= attackDistance)
                {
                    StartCombat();
                    movementDirection = Vector2.zero;
                }
                else
                {
                    Chase();
                }

                break;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movementDirection * speed * currentSpeedMultiplier;
    }

    private void StartCombat()
    {
        BattleStarter starter = GetComponentInParent<BattleStarter>();

        List<GameObject> _enemyList = new List<GameObject>();
        _enemyList.Add(arenaPrefab);

        if (starter != null)
        {
            starter.StartBattle(playerPos.gameObject, uniqueID, _enemyList);
        }
    }

    private void Chase()
    {
        Vector2 direction = (playerPos.position - transform.position).normalized;

        anim.SetBool("Walk", true);
        anim.SetFloat("Horizontal", direction.x);
        anim.SetFloat("Vertical", direction.y);

        if ((direction.x < -0.1f && facingRight) ||
            (direction.x > 0.1f && !facingRight))
        {
            Flip();
        }

        movementDirection = direction;
        currentSpeedMultiplier = 1.5f;
    }

    private void Patrol()
    {
        Transform target = patrolPoints[currentPatrolIndex];

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            anim.SetBool("Walk", false);
            movementDirection = Vector2.zero;

            currentWaitTime += Time.deltaTime;

            if (currentWaitTime >= waitTime)
            {
                currentWaitTime = 0f;
                currentPatrolIndex++;

                if (currentPatrolIndex >= patrolPoints.Length)
                {
                    currentPatrolIndex = 0;
                }
            }
        }
        else
        {
            Vector2 direction = (target.position - transform.position).normalized;

            anim.SetBool("Walk", true);
            anim.SetFloat("Horizontal", direction.x);
            anim.SetFloat("Vertical", direction.y);

            if ((direction.x < -0.1f && facingRight) ||
                (direction.x > 0.1f && !facingRight))
            {
                Flip();
            }

            movementDirection = direction;
            currentSpeedMultiplier = 1f;
        }
    }

    private void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
