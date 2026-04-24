using UnityEngine;

/*
 * <summary>
 * Este script controla el comportamiento de un enemigo que persigue al jugador.
 * </summary>
 * - El enemigo tiene tres estados: Perseguir, Atacar y Muerto.
 * - En el estado de Perseguir, el enemigo se mueve hacia el jugador.
 * - En el estado de Atacar, el enemigo inflige daño al jugador si está dentro del rango de ataque.
 * - En el estado de Muerto, el enemigo deja de moverse y se destruye después de una animación.
 */

public class ChaserEnemy : MonoBehaviour
{

    /* <summary>
     * Definición de los estados del enemigo para controlar su comportamiento.
     * Chase: El enemigo persigue al jugador.
     * Attack: El enemigo ataca al jugador si está dentro del rango.
     * Dead: El enemigo ha sido derrotado y no realiza ninguna acción.
     * </summary>
     */
    public enum EnemyState
    {
        Chase,
        Attack,
        Dead
    }

    /* <summary>
     * Variables para configurar las referencias a los objetos necesarios y las estadísticas del enemigo.
     * speed: Velocidad de movimiento del enemigo.
     * attackRange: Distancia a la que el enemigo puede atacar al jugador.
     * attackCooldown: Tiempo de espera entre ataques.
     * health: Puntos de vida del enemigo.
     * playerHealth: Referencia al componente de salud del jugador para infligir daño.
     * spawnManager: Referencia al SpawnManager para notificar cuando el enemigo muere.
     * animator: Referencia al Animator para controlar las animaciones del enemigo.
     * spriteRenderer: Referencia al SpriteRenderer para controlar la orientación del sprite.
     * </summary>
     */
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Stats")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int health = 3;
    [SerializeField] private int scoreValue = 100;

    private EnemyState currentState;
    private float attackTimer;
    private PlayerHealth playerHealth;
    private SpawnManager spawnManager;
    private ScoreManager scoreManager;

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

        scoreManager = FindFirstObjectByType<ScoreManager>();
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

    /* <summary>
     * Método para establecer la referencia al SpawnManager, permitiendo que el enemigo notifique su muerte.
     * </summary>
     * <param name="manager">Instancia del SpawnManager que controla la generación de enemigos.</param>
     */
    public void SetSpawnManager(SpawnManager manager)
    {
        spawnManager = manager;
    }

    /* <summary>
     * Método para cambiar el estado actual del enemigo y actualizar la animación correspondiente.
     * </summary>
     * <param name="newState">El nuevo estado al que se cambiará el enemigo.</param>
     */
    private void ChangeState(EnemyState newState)
    {
        currentState = newState;
        animator.SetInteger("State", (int)currentState);
    }

    /* <summary>
     * Método para manejar el comportamiento de persecución del enemigo hacia el jugador.
     * El enemigo se mueve hacia la posición del jugador y cambia al estado de ataque si está dentro del rango.
     * </summary>
     */
    private void Chase()
    {
        MoveToPlayer();

        float distance = Vector2.Distance(transform.position, player.position);

        if(distance <= attackRange)
        {
            ChangeState(EnemyState.Attack);
        }
    }

    /* <summary>
     * Método para manejar el comportamiento de ataque del enemigo hacia el jugador.
     * El enemigo inflige daño al jugador si está dentro del rango de ataque y espera un tiempo de enfriamiento entre ataques.
     * Si el jugador se aleja del rango, el enemigo vuelve al estado de persecución.
     * </summary>
     */
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

    /* <summary>
     * Método para manejar la recepción de daño por parte del enemigo.
     * El enemigo pierde puntos de vida y cambia al estado de muerto si su salud llega a cero o menos.
     * Al morir, notifica al SpawnManager y se destruye después de una animación.
     * </summary>
     * <param name="damage">Cantidad de daño que el enemigo recibirá.</param>
     */
    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            ChangeState(EnemyState.Dead);

            if (scoreManager != null)
            {
                scoreManager.AddScore(scoreValue);
            }

            if (spawnManager != null)
            {
                spawnManager.OnEnemyDeath();
            }

            Destroy(gameObject, 1.5f);
        }
    }

    /* <summary>
     * Método para mover al enemigo hacia la posición del jugador.
     * El enemigo se mueve en la dirección del jugador a una velocidad determinada y ajusta la orientación del sprite según la dirección.
     * </summary>
     */
    private void MoveToPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        if (direction.x > 0) spriteRenderer.flipX = false;
        else if (direction.x < 0) spriteRenderer.flipX = true;
    }
}
