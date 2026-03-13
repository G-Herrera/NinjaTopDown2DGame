using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerStateManager : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("World Wrap")]
    [SerializeField] private WorldBounds2D world;
    [SerializeField] private bool autoFindWorld = true;
    [SerializeField] private float wrapPadding = 0.05f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Health")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Orientation")]
    [SerializeField] private float directionDeadzone = 0.01f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private static readonly int HashIsMoving = Animator.StringToHash("isMoving");
    private static readonly int HashMoveX = Animator.StringToHash("moveX");
    private static readonly int HashMoveY = Animator.StringToHash("moveY");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        if (animator == null) animator = GetComponentInChildren<Animator>(true);
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
        if (playerHealth == null) playerHealth = GetComponent<PlayerHealth>();
        if (autoFindWorld && world == null) world = WorldBounds2D.Instance;
    }

    private void Update()
    {
        // Si no estamos en gameplay o estamos muertos, no hay input
        if (!GameFlowManager.Instance.IsGameplay || (playerHealth != null && playerHealth.IsDead))
        {
            moveInput = Vector2.zero;
            UpdateAnimator(Vector2.zero);
            return;
        }

        if (Keyboard.current == null)
        {
            moveInput = Vector2.zero;
            UpdateAnimator(Vector2.zero);
            return;
        }

        // Captura input
        float x = 0f;
        float y = 0f;
        if (Keyboard.current.aKey.isPressed) x -= 1f;
        if (Keyboard.current.dKey.isPressed) x += 1f;
        if (Keyboard.current.wKey.isPressed) y += 1f;
        if (Keyboard.current.sKey.isPressed) y -= 1f;

        moveInput = new Vector2(x, y);
        if (moveInput.sqrMagnitude > 1f)
            moveInput = moveInput.normalized;

        UpdateAnimator(moveInput);
        UpdateOrientation(moveInput);
    }

    private void FixedUpdate()
    {
        // Si no es gameplay o estamos muertos, detener físico
        if (!GameFlowManager.Instance.IsGameplay || (playerHealth != null && playerHealth.IsDead))
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 nextPos = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;

        // World wrap
        if (world != null)
        {
            if (nextPos.x > world.maxX) nextPos.x = world.minX + wrapPadding;
            else if (nextPos.x < world.minX) nextPos.x = world.maxX - wrapPadding;

            if (nextPos.y > world.maxY) nextPos.y = world.minY + wrapPadding;
            else if (nextPos.y < world.minY) nextPos.y = world.maxY - wrapPadding;
        }

        rb.MovePosition(nextPos);
    }

    private void UpdateAnimator(Vector2 input)
    {
        if (animator == null) return;

        bool isMoving = input.sqrMagnitude > 0.0001f;
        animator.SetBool(HashIsMoving, isMoving);
        animator.SetFloat(HashMoveX, input.x);
        animator.SetFloat(HashMoveY, input.y);
    }

    private void UpdateOrientation(Vector2 dir)
    {
        if (spriteRenderer == null) return;
        if (dir.sqrMagnitude < directionDeadzone * directionDeadzone) return;

        bool verticalDominant = Mathf.Abs(dir.y) > Mathf.Abs(dir.x);

        if (verticalDominant)
        {
            spriteRenderer.flipX = false;
            spriteRenderer.transform.localEulerAngles = dir.y > 0 ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);
        }
        else
        {
            spriteRenderer.transform.localEulerAngles = Vector3.zero;
            spriteRenderer.flipX = dir.x < 0f;
        }
    }

    // Esto asegura que si desactivamos el script desde GameFlowManager, el jugador se detenga
    private void OnDisable()
    {
        moveInput = Vector2.zero;
        if (animator != null)
        {
            animator.SetBool(HashIsMoving, false);
            animator.SetFloat(HashMoveX, 0f);
            animator.SetFloat(HashMoveY, 0f);
        }

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false; // Bloquea física si script desactivado
        }
    }

    // Esto vuelve a activar la física si reactivamos el script
    private void OnEnable()
    {
        if (rb != null)
            rb.simulated = true;
    }
}