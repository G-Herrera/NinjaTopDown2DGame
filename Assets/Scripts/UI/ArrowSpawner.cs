using UnityEngine;

/*
 * <summary>
 * Este script se encarga de generar flechas en puntos específicos del escenario, apuntando hacia el jugador.
 * </summary>
 * - Las flechas se generan a intervalos regulares definidos por spawnCooldown.
 * - Cada flecha se instancia en una posición aleatoria de un conjunto de puntos de generación.
 * - Las flechas se dirigen automáticamente hacia la posición del jugador al momento de su creación.
 */

public class ArrowSpawner : MonoBehaviour
{
    /*
     * <summary>
     * Variables para configurar las referencias a los objetos necesarios y los parámetros de generación.
     * </summary>
     */

    [Header("References")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform player;

    /*
     * <summary>
     * Variable para configurar el tiempo de espera entre cada generación de flechas.
     * </summary>
     */
    [Header("Spawn Settings")]
    [SerializeField] private float spawnCooldown = 2f;

    private float timer;

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnArrow();
            timer = spawnCooldown;
        }
    }

    /*
     * <summary>
     * Método para generar una flecha en un punto aleatorio y dirigirla hacia el jugador.
     * </summary>
     */

    private void SpawnArrow()
    {
        if (spawnPoints.Length == 0) return;

        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        GameObject arrow = Instantiate(arrowPrefab, spawnPoint.position, Quaternion.identity);

        Vector2 direction = (player.position - spawnPoint.position).normalized;

        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.SetDirection(direction);
        }
    }
}