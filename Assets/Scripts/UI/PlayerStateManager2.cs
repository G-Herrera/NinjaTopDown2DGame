using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerStateManager2 : MonoBehaviour
{
    // =========================================================
    // Inspector: Movement
    // =========================================================
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    // [SerializeField] private bool rotateForVertical = true;

    // =========================================================
    // Inspector: Animation
    // =========================================================
    // Assign these in the Inspector if you want explicit wiring.
    // If left empty, the script will auto-find them on this object or its children.
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // =========================================================
    // Inspector: Debug
    // =========================================================
    [Header("Debug")]
    [SerializeField] private bool logWarnings = true;
    [SerializeField] private bool logMovement = false;

    // =========================================================
    // Private runtime fields
    // =========================================================
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;

    // Cache parameter hashes (faster + avoids GC, and helps keep names consistent)
    private static readonly int HashIsMoving = Animator.StringToHash("isMoving");
    private static readonly int HashMoveX    = Animator.StringToHash("moveX");
    private static readonly int HashMoveY    = Animator.StringToHash("moveY");
    private static readonly int HashIsGrounded = Animator.StringToHash("isGrounded");

    private void Awake()
    {
        // --- Rigidbody2D setup ---
        rb = GetComponent<Rigidbody2D>();

        // Typical 2D top-down setup (no gravity).
        rb.gravityScale = 3f;

        // Prevent rotation caused by physics.
        rb.freezeRotation = true;

        // --- Auto-find visual components if not assigned ---
        // (true) includes inactive children in case your PlayerVisual is disabled in menus.
        if (animator == null) animator = GetComponentInChildren<Animator>(true);
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);

        // --- Basic validation logs (helps students wiring in Inspector) ---
        if (logWarnings)
        {
            if (animator == null)
                Debug.LogWarning("[PlayerStateManager] Animator NOT found. Add an Animator (usually on PlayerVisual) or assign it in the Inspector.");

            if (spriteRenderer == null)
                Debug.LogWarning("[PlayerStateManager] SpriteRenderer NOT found. Add a SpriteRenderer (usually on PlayerVisual) or assign it in the Inspector.");
        }
    }

    private void Update()
    {
        CheckGround();

        // If no keyboard is available, do not move.
        if (Keyboard.current == null)
        {
            moveInput = Vector2.zero;
            UpdateAnimator(moveInput);
            return;
        }

        float x = 0f;
        float y = 0f;

        // A/D = left/right
        if (Keyboard.current.aKey.isPressed) x -= 1f;
        if (Keyboard.current.dKey.isPressed) x += 1f;

        // W/S = up/down
        //if (Keyboard.current.wKey.isPressed) y += 1f;
        //if (Keyboard.current.sKey.isPressed) y -= 1f;

        // Normalize so diagonal movement is not faster.
        moveInput = new Vector2(x, 0f);

        if (logMovement)
        {
            Debug.Log($"[PlayerStateManager] moveInput={moveInput}  timeScale={Time.timeScale}");
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            Jump();
        }

        UpdateAnimator(moveInput);
    }

    private void FixedUpdate()
    {
            rb.linearVelocity = new Vector2(
                moveInput.x * moveSpeed,
                rb.linearVelocity.y
            );
    }

    private void UpdateAnimator(Vector2 input)
{
    // Animator is optional; movement still works without it.
    if (animator == null) return;

    // True if the stick/input is not near zero.
    bool isMoving = input.sqrMagnitude > 0.0001f;

    // Animator parameters (must exist in the Animator Controller)
    animator.SetBool(HashIsMoving, isMoving);
    animator.SetFloat(HashMoveX, input.x);
    animator.SetFloat(HashMoveY, input.y);
    animator.SetBool(HashIsGrounded, isGrounded);


        // If we don't have a SpriteRenderer, we can't change visual orientation.
        if (spriteRenderer == null) return;

    // If not moving, keep last orientation (idle keeps facing last direction).
    if (!isMoving) return;

    // Choose a "dominant axis" so diagonals don't jitter between horizontal and vertical.
    bool verticalDominant = Mathf.Abs(input.y) > Mathf.Abs(input.x);

    if (verticalDominant)
    {
        // Face up/down by rotating the visual
        // (works well for prototypes when you only have right-facing animations)
        spriteRenderer.flipX = false; // no left/right flip when rotated

        if (input.y > 0.01f)
            spriteRenderer.transform.localEulerAngles = new Vector3(0f, 0f, 90f);   // Up (+Y)
        else if (input.y < -0.01f)
            spriteRenderer.transform.localEulerAngles = new Vector3(0f, 0f, -90f); // Down (-Y)
    }
    else
    {
        // Face left/right: reset rotation and use flipX for left
        spriteRenderer.transform.localEulerAngles = Vector3.zero;

        if (Mathf.Abs(input.x) > 0.01f)
            spriteRenderer.flipX = input.x < 0f;
    }
}

    private void CheckGround()
    {
        if (groundCheck == null) return;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position,groundRadius,groundLayer);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // reset Y
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

}
