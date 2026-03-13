using UnityEngine;

public class WarpPoints : MonoBehaviour
{
    public Transform destination;
    public float exitOffset = 0.5f;
    private bool canWarp = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canWarp) return;
        
        if (other.CompareTag("Player"))
        {
            WarpPoints destWarp = destination.GetComponent<WarpPoints>();

            if (destWarp != null)
            {
                destWarp.canWarp = false;
                Vector3 offset = other.transform.up * exitOffset;
                other.transform.position = destination.position + offset;

                Invoke(nameof(ResetWarp), 0.2f);
                destWarp.Invoke(nameof(ResetWarp), 0.2f);
            }
        }
    }

    private void ResetWarp()
    {
        canWarp = true;
    }
}
