using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private int maxHP = 3;
    [SerializeField] private int currentHP;

    [Header("UI (HUD)")]
    [SerializeField] private TextMeshProUGUI txtHP;

    [Header("Player Animation")]
    [SerializeField] private Animator animator;           // assign PlayerVisual animator (or auto-find)
    [SerializeField] private float hurtLockTime = 0.20f;  // small lock to avoid hurt spam

    [Header("Death Timing")]
    [Tooltip("Tiempo para que se vea la animación de muerte antes de disparar GameOver")]
    [SerializeField] private float gameOverDelayAfterDeath = 0.8f;

    [Header("Invulnerability")]
    [Tooltip("Tiempo de invulnerabilidad después de recibir daño (opcional)")]
    [SerializeField] private float invulnerabilityTime = 0.5f;
    [SerializeField] private float blinkInterval = 0.1f; // Intervalo de parpadeo durante la invulnerabilidad

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hurtClip;

    private bool isDead;
    private float hurtLockTimer;
    private Coroutine deathRoutine;
    private bool isInvulnerable;
    private Coroutine invulnerabilityRoutine;
    private SpriteRenderer spriteRenderer;

    private static readonly int HashHurt = Animator.StringToHash("hurt");
    private static readonly int HashIsDead = Animator.StringToHash("isDead");

    public int CurrentHP => currentHP;
    public bool IsDead => isDead;

    private void Awake()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        // Auto-find animator in children (works if PlayerVisual holds Animator)
        if (animator == null)
            animator = GetComponentInChildren<Animator>(true);

        // Auto-find sprite renderer in children (works if PlayerVisual holds SpriteRenderer)
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
    }

    private void Start()
    {
        currentHP = maxHP;
        isDead = false;
        hurtLockTimer = 0f;

        UpdateUI();

        if (animator != null)
            animator.SetBool(HashIsDead, false);
    }

    private void Update()
    {
        // Unscaled para que funcione aunque el juego se pause por otros sistemas
        if (hurtLockTimer > 0f)
            hurtLockTimer -= Time.unscaledDeltaTime;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        if (isInvulnerable) return;
        if (amount <= 0) return;

        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;

        UpdateUI();

        if (audioSource != null && hurtClip != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f); // Pequeña variación de pitch para evitar repetición
            audioSource.PlayOneShot(hurtClip);
        }
            

        if (currentHP == 0)
        {
            Die();
            return;
        }

        PlayHurtAnimation();
        StartInvulnerability();
    }

    private void PlayHurtAnimation()
    {
        // Avoid spam trigger if enemy hits rapidly
        if (hurtLockTimer > 0f) return;
        hurtLockTimer = hurtLockTime;

        if (animator != null)
            animator.SetTrigger(HashHurt);
    }

    private void StartInvulnerability()
    {
        if (invulnerabilityRoutine != null)
            StopCoroutine(invulnerabilityRoutine);
        invulnerabilityRoutine = StartCoroutine(InvulnerabilityRoutine());
    }

    private void Die()
    {
        isInvulnerable = true;
        if (isDead) return;
        isDead = true;

        Debug.Log("[PlayerHealth] PLAYER DIED");

        if (invulnerabilityRoutine != null)
        {
            StopCoroutine(invulnerabilityRoutine);
            invulnerabilityRoutine = null;
        }

        if (spriteRenderer != null)
            spriteRenderer.enabled = true; // Asegura que el sprite esté visible para la animación de muerte

        // Trigger death animation
        if (animator != null)
            animator.SetBool(HashIsDead, true);

        // Si ya había una rutina (por cualquier razón), la reiniciamos
        if (deathRoutine != null)
            StopCoroutine(deathRoutine);

        deathRoutine = StartCoroutine(DieRoutine());
    }

    private IEnumerator DieRoutine()
    {
        // Espera en tiempo real para que se vea la animación de morir
        yield return new WaitForSecondsRealtime(Mathf.Max(0f, gameOverDelayAfterDeath));

        // Notifica al flujo unificado (desactiva input/IA, congela cámara, muestra GameOver)
        if (GameFlowManager.Instance != null)
            GameFlowManager.Instance.RequestGameOver();
        else
            Debug.LogWarning("[PlayerHealth] GameFlowManager.Instance is null. No GameOver was requested.");
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        float elapsed = 0f;
        while (elapsed < invulnerabilityTime)
        {
            // Parpadeo del sprite para indicar invulnerabilidad
            if (spriteRenderer != null)
                spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }
        // Asegura que el sprite esté visible al final
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
        isInvulnerable = false;
    }

    private void UpdateUI()
    {
        if (txtHP != null)
            txtHP.text = $"{currentHP}";
    }

    // Útil si reinicias sin recargar escena (opcional)
    public void ResetHP()
    {
        isDead = false;
        currentHP = maxHP;
        hurtLockTimer = 0f;

        if (deathRoutine != null)
        {
            StopCoroutine(deathRoutine);
            deathRoutine = null;
        }

        UpdateUI();

        if (animator != null)
        {
            animator.SetBool(HashIsDead, false);
            // No hace falta resetear el trigger hurt
        }
    }
}

