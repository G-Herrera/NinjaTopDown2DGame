using UnityEngine;

/*
 * <summary>
 * Este script se encarga de generar enemigos en puntos específicos del escenario, controlando la cantidad máxima y el tiempo entre cada generación.
 * </summary>
 * - Los enemigos se generan a intervalos regulares definidos por spawnCooldown.
 * - Cada enemigo se instancia en una posición aleatoria de un conjunto de puntos de generación.
 * - El SpawnManager mantiene un conteo de los enemigos actuales para no exceder el límite establecido.
 * - Cuando un enemigo muere, el SpawnManager es notificado para actualizar el conteo y permitir nuevas generaciones.
 */

public class SpawnManager : MonoBehaviour
{
    /* <summary>
     * Variables para configurar las referencias a los objetos necesarios y los parámetros de generación.
     * m_Prefab: Prefab del enemigo que se va a generar.
     * m_SpawnPoints: Array de puntos en el escenario donde se pueden generar los enemigos.
     * maxEnemies: Cantidad máxima de enemigos que pueden estar activos al mismo tiempo.
     * spawnCooldown: Tiempo de espera entre cada generación de enemigos.
     * timer: Contador interno para controlar el tiempo entre generaciones.
     * currentEnemies: Contador de enemigos actualmente activos en el escenario.
     * </summary>
     */
    [Header("References")]
    [SerializeField] private GameObject m_Prefab;
    [SerializeField] private Transform[] m_SpawnPoints;
    [SerializeField] private Transform playerTransform;

    [Header("Spawn Control")]
    [SerializeField] private int maxEnemies = 2;
    [SerializeField] private float spawnCooldown = 5f;

    private float timer;
    private int currentEnemies;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentEnemies = 0;    
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEnemies < maxEnemies)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SpawnEnemy();
                currentEnemies++;
                timer = spawnCooldown;
            }
        }  
    }

    /* <summary>
     * Método para generar un enemigo en un punto aleatorio del escenario.
     * Se instancia el prefab del enemigo en la posición seleccionada y se establece una referencia al SpawnManager para que el enemigo pueda notificar su muerte.
     * </summary>
     */
    private void SpawnEnemy()
    {
        GameObject skeletonEnemy=Instantiate(m_Prefab, SelecctSpawn(), Quaternion.identity);
        
        ChaserEnemy script = skeletonEnemy.GetComponent<ChaserEnemy>();
        if (script != null)
        {
            script.SetSpawnManager(this);
        }
    }

    /* <summary>
     * Método para seleccionar un punto de generación aleatorio del array de spawn points.
     * Si no hay puntos asignados, se muestra una advertencia y se devuelve la posición del SpawnManager como fallback.
     * </summary>
     * <returns>Posición del punto de generación seleccionado.</returns>
     */
    private Vector2 SelecctSpawn()
    {
        if (m_SpawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned");
            return Vector2.zero;
        }

        Transform farthestSpawn = m_SpawnPoints[0];
        float maxDistance = 0f;

        foreach (Transform spawnPoint in m_SpawnPoints)
        {
            float distance = Vector2.Distance(spawnPoint.position, playerTransform.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestSpawn = spawnPoint;
            }
        }

        return farthestSpawn.position;
    }

    /* <summary>
     * Método para ser llamado por los enemigos cuando mueren, permitiendo al SpawnManager actualizar el conteo de enemigos activos.
     * Esto es crucial para permitir que el SpawnManager genere nuevos enemigos cuando el número actual es menor que el máximo permitido.
     * </summary>
     */
    public void OnEnemyDeath()
    {
        currentEnemies--;
    }
}
