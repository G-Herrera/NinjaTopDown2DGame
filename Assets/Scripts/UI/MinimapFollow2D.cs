using UnityEngine;

public class MinimapFollow2D : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Movement")]
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Bounds (Optional)")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    [Header("Start")]
    [SerializeField] private bool snapOnStart = true;

    private void Start()
    {
        if (snapOnStart && target != null)
        {
            transform.position = GetDesiredPosition();
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = GetDesiredPosition();

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }

    private Vector3 GetDesiredPosition()
    {
        Vector3 desired = target.position + offset;

        if (useBounds)
        {
            desired.x = Mathf.Clamp(desired.x, minBounds.x, maxBounds.x);
            desired.y = Mathf.Clamp(desired.y, minBounds.y, maxBounds.y);
        }

        desired.z = offset.z;

        return desired;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}

