using UnityEngine;

/*
 * <summary>
 * Este script controla el comportamiento de la estrella ninja lanzada por el jugador.
 * </summary>
 * - La estrella ninja se mueve en una dirección específica a una velocidad determinada.
 * - Tiene un tiempo de vida limitado, después del cual se destruye automáticamente.
 * - Al colisionar con un enemigo, inflige daño y se destruye.
 */

public class NinjaStar : MonoBehaviour
{
    /* <summary>
     * Variables para configurar la velocidad, tiempo de vida y daño de la estrella ninja.
     * </summary>
     */
    [Header("Stats")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 1;

    private Vector2 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    /* <summary>
     * Establece la dirección de movimiento de la estrella ninja y ajusta su rotación visual.
     * </summary>
     * <param name="dir">Dirección en la que se moverá la estrella ninja.</param>
     */
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ChaserEnemy skeletonEnemy =collision.GetComponent<ChaserEnemy>();
        EnemyStateManager2D ninjaEnemy = collision.GetComponent<EnemyStateManager2D>();

        if (skeletonEnemy != null)
        {
            skeletonEnemy.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (ninjaEnemy != null)
        {
            ninjaEnemy.Freeze();
            Destroy(gameObject);
        }
    }
}
