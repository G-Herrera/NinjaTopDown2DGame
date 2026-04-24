using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections;

public class EnemyStateManager2D : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private TextMeshProUGUI txtStateDebug;
    

    [Header("Ranges")]
    [SerializeField] private float detectionRange = 6f;
    [SerializeField] private float attackRange = 1.5f;

    [Header("Movement")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 3.5f;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 1.2f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Freeze")]
    [SerializeField] private float freezeDurationOnHit = 2f;
    [SerializeField] private int scoreValueOnFreeze = 50;

    private bool isFrozen;
    private Coroutine freezeRoutine;

    private EnemyState currentState;
    private int currentWaypointIndex;
    private float attackTimer;
    private PlayerHealth playerHealth;
    private ScoreManager scoreManager;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(animator == null) animator = GetComponent<Animator>();
        if(spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        playerHealth = player.GetComponent<PlayerHealth>();
        scoreManager = FindFirstObjectByType<ScoreManager>();

        ChangeState(EnemyState.Patrol);    
    }

    // Update is called once per frame
    private void Update()
    {
        if (isFrozen) return;

        switch (currentState) 
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }

        if (txtStateDebug != null)
        {
            txtStateDebug.text = $"{currentState}";
        }

    }

    private void ChangeState(EnemyState nextState)
    {
        currentState = nextState;

        animator.SetInteger("State", (int)currentState);

        Debug.Log($"[Enemy FSM] State changed to {currentState}");
    }

    private void Patrol()
    {
        MoveTo(waypoints[currentWaypointIndex], patrolSpeed);

        if (Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            ChangeState(EnemyState.Chase);
            return;
        }

        if(Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.2f)
        {
            int nextIndex = currentWaypointIndex;

            while (nextIndex == currentWaypointIndex && waypoints.Length > 1) 
            { 
                nextIndex = Random.Range(0, waypoints.Length);
            }

            currentWaypointIndex = nextIndex;
        }

    }

    private void Chase()
    {
        MoveTo(player, chaseSpeed);

        float distance = Vector2.Distance(transform.position, player.position);

        if(distance <= attackRange)
        {
            ChangeState(EnemyState.Attack);
            return;
        }

        if(distance > detectionRange)
        {
            ChangeState(EnemyState.Patrol);
        }
    }

    private void Attack()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            ChangeState(EnemyState.Chase);
            return;
        }

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f) 
        {
            Debug.Log("[Enemy FSM] Attack");

            if(playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }

            attackTimer = attackCooldown;
        }
    }

    private void MoveTo( Transform target, float speed)
    {
        Vector2 direction= (target.position - transform.position).normalized;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        if (direction.x > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if(direction.x < 0f)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void Freeze()
    {
        if(isFrozen) return;

        if (scoreManager != null)
        {
            scoreManager.AddScore(50);
        }

        if (freezeRoutine != null)
        {
            StopCoroutine(freezeRoutine);
        }
    
        freezeRoutine = StartCoroutine(FreezeCoroutine());
    }

    private IEnumerator FreezeCoroutine()
    {
        isFrozen = true;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.blue; // Change color to indicate freeze
        }

        animator.speed = 0f;

        yield return new WaitForSeconds(freezeDurationOnHit);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white; // Revert color
        }
        animator.speed = 1f;
        isFrozen = false;
    }
}
