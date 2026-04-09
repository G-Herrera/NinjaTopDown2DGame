using UnityEngine;

public class NinjaStar : MonoBehaviour
{
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

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ChaserEnemy skeletonEnemy =collision.GetComponent<ChaserEnemy>();

        if (skeletonEnemy != null)
        {
            skeletonEnemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
