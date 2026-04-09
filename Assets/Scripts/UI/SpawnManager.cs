using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject m_Prefab;
    [SerializeField] private Transform[] m_SpawnPoints;

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

    private void SpawnEnemy()
    {
        GameObject skeletonEnemy=Instantiate(m_Prefab, SelecctSpawn(), Quaternion.identity);
        
        ChaserEnemy script = skeletonEnemy.GetComponent<ChaserEnemy>();
        if (script != null)
        {
            script.SetSpawnManager(this);
        }
    }

    private Vector2 SelecctSpawn()
    {
        if (m_SpawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned");
            return transform.position;
        }

        int index = Random.Range(0, m_SpawnPoints.Length);
        return m_SpawnPoints[index].position;
    }

    public void OnEnemyDeath()
    {
        currentEnemies--;
    }
}
