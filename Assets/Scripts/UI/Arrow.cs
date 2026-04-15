using UnityEngine;

/*
 * <summary>
 * Este script controla el comportamiento de la flecha disparada por el enemigo.
 * </summary>
 * - La flecha se mueve en una dirección específica a una velocidad determinada.
 * - Tiene un tiempo de vida limitado, después del cual se destruye automáticamente.
 * - Al colisionar con el jugador, inflige daño y se destruye.
 */

public class Arrow : MonoBehaviour
{
    /*
     * <summary>
     * Variables para configurar la velocidad, tiempo de vida y daño de la flecha.
     * </summary>
     */

    [SerializeField] private float speed = 6f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private int damage = 1;

    private Vector2 direction;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    /*
     * <summary>
     * Establece la dirección de movimiento de la flecha y ajusta su rotación visual.
     * </summary>
     * <param name="dir">Dirección en la que se moverá la flecha.</param>
     */

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        // Rotación visual
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}