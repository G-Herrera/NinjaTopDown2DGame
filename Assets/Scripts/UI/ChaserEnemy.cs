using UnityEngine;

public class ChaserEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        Chase,
        Attack,
        Dead
    }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Stats")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int health = 3;

    private EnemyState currentState;
    private float attackTimer;
    private PlayerHealth playerHealth;
    private SpawnManager spawnManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            ChangeState(EnemyState.Chase);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Chase:
                Chase(); 
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Dead:
                break;
        }
    }

    public void SetSpawnManager(SpawnManager manager)
    {
        spawnManager = manager;
    }

    private void ChangeState(EnemyState newState)
    {
        currentState = newState;
        animator.SetInteger("State", (int)currentState);
    }

    private void Chase()
    {
        MoveToPlayer();

        float distance = Vector2.Distance(transform.position, player.position);

        if(distance <= attackRange)
        {
            ChangeState(EnemyState.Attack);
        }
    }

    private void Attack()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if(distance > attackRange)
        {
            ChangeState(EnemyState.Chase);
            return;
        }

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }

            attackTimer = attackCooldown;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            ChangeState(EnemyState.Dead);

            if(spawnManager != null)
            {
                spawnManager.OnEnemyDeath();
            }

            Destroy(gameObject, 1.5f);
        }
    }

    private void MoveToPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        if (direction.x > 0) spriteRenderer.flipX = false;
        else if (direction.x < 0) spriteRenderer.flipX = true;
    }
}
