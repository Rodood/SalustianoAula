using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase
    }

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

    Animator anim;
    bool facingRight = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
        currentState = EnemyState.Idle;
        idleTime = waitTime;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                anim.SetBool("Walk", false);

                currentIdleTime += Time.deltaTime;
                if(currentIdleTime > idleTime)
                {
                    currentState = EnemyState.Patrol;
                    currentIdleTime = 0f;
                }
                break;
            case EnemyState.Patrol:
                anim.SetBool("Walk", true);
                Patrol();
                break;
            case EnemyState.Chase:
                break;
        }
    }

    private void Patrol()
    {
        Transform target = patrolPoints[currentPatrolIndex];

        if(Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            anim.SetBool("Walk", false);

            currentWaitTime += Time.deltaTime;

            if(currentWaitTime >= waitTime)
            {
                currentWaitTime = 0f;
                currentPatrolIndex++;

                if(currentPatrolIndex >= patrolPoints.Length)
                {
                    currentPatrolIndex = 0;
                }
            }
        }
        else
        {
            Vector3 direction = (target.position - transform.position).normalized;

            anim.SetBool("Walk", true);
            anim.SetFloat("Horizontal", direction.x);
            anim.SetFloat("Vertical", direction.y);

            if((direction.x < -0.1f && facingRight) || 
                (direction.x > 0.1f && !facingRight))
            {
                Flip();
            }

            transform.position = Vector2.MoveTowards(transform.position, target.position,
                speed * Time.deltaTime);
        }
    }

    private void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }
}
