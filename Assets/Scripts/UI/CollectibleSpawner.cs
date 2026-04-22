using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Collectible Settings")]
    [SerializeField] private GameObject collectiblePrefab;
    [SerializeField] private Transform[] spawnPoints;

    private GameObject currentCollectible;
    private int lastSpawnIndex = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      SpawnCollectible();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnCollectible()
    {
        // Si ya hay un collectible en escena, lo destruimos antes de generar uno nuevo
        if (currentCollectible != null) Destroy(currentCollectible);

        // Verificamos que haya puntos de spawn y un prefab asignado antes de intentar generar el collectible
        if (spawnPoints.Length == 0 || collectiblePrefab == null)
        {
            Debug.LogWarning("No spawn points or collectible prefab assigned.");
            return;
        }

        
        int randomIndex = Random.Range(0, spawnPoints.Length);
        while (randomIndex == lastSpawnIndex && spawnPoints.Length > 1) 
        {
            randomIndex = Random.Range(0, spawnPoints.Length);
        }
        lastSpawnIndex = randomIndex;
        Transform spawnPoint = spawnPoints[randomIndex];
        currentCollectible = Instantiate(collectiblePrefab, spawnPoint.position, Quaternion.identity);
    }
}
