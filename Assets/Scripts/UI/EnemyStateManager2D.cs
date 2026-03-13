using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;

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

    private EnemyState currentState;
    private int currentWaypointIndex;
    private float attackTimer;
    private PlayerHealth playerHealth;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(animator == null) animator = GetComponent<Animator>();
        if(spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        playerHealth = player.GetComponent<PlayerHealth>();

        ChangeState(EnemyState.Patrol);    
    }

    // Update is called once per frame
    private void Update()
    {
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
            txtStateDebug.text = $"Enemy State: {currentState}";
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
}
