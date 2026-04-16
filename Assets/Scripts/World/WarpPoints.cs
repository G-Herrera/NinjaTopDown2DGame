using UnityEngine;

public class WarpPoints : MonoBehaviour
{
    [Header("Warp Settings")]
    [SerializeField] private float warpCooldown = 2f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = Color.gray;
    public Transform destination;
    public float exitOffset = 0.5f;
    private bool canWarp = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

           UpdateColor();
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
                canWarp = false;
                destWarp.canWarp = false;

                UpdateColor();
                destWarp.UpdateColor();

                Vector3 offset = other.transform.up * exitOffset;
                other.transform.position = destination.position + offset;

                Invoke(nameof(ResetWarp), warpCooldown);
                destWarp.Invoke(nameof(ResetWarp), warpCooldown);
            }
        }
    }

    private void ResetWarp()
    {
        canWarp = true;
            UpdateColor();
    }

    private void UpdateColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = canWarp ? activeColor : inactiveColor;
        }
    }
}

