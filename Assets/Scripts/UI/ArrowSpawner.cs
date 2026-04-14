using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform player;

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