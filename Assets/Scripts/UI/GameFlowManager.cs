using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public enum GameState { Gameplay, GameOver, Victory, Paused, HighScoreEntry}

    public static GameFlowManager Instance { get; private set; }

    [Header("State (read-only)")]
    [SerializeField] private GameState currentState = GameState.Gameplay;
    public GameState CurrentState => currentState;
    public bool IsGameplay => currentState == GameState.Gameplay;

    [Header("Controllers to Disable (drag & drop)")]
    [Tooltip("Arrastra tu PlayerStateManager aquí")]
    [SerializeField] private MonoBehaviour playerController;

    [Tooltip("Arrastra aquí tus EnemyStateManager2D (uno o varios)")]
    [SerializeField] private List<MonoBehaviour> enemyControllers = new List<MonoBehaviour>();

    [Tooltip("Arrastra tu CameraFollow aquí (tu script de cámara)")]
    [SerializeField] private CameraFollow cameraFollow;

    [Header("Core Systems")]
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("UI Panels")]
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

    [Header("Delays (seconds)")]
    [Tooltip("Tiempo para que se vea la animación de morir antes de GameOver")]
    [SerializeField] private float gameOverDelay = 1.0f;

    [Tooltip("Pequeño delay opcional antes de mostrar Victoria")]
    [SerializeField] private float victoryDelay = 0.5f;

    [Header("Victory Score Bonus")]
    [SerializeField] private int victoryBonus = 100;
    [SerializeField] private int healthBonusPerHP = 25;

    [Header("High Scores")]
    [SerializeField] private GameObject highScoreInputPanel;

    private Coroutine endRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetState(GameState.Gameplay);
        ShowPanels(gameplay: true);
        if (cameraFollow != null) cameraFollow.SetFrozen(false);
    }

    private void Update()
    {
        // Detectar tecla de Pausa (Esc o P) usando el nuevo Input System
        if (Keyboard.current != null && (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame))
        {
            if (currentState == GameState.Gameplay)
                PauseGame();
            else if (currentState == GameState.Paused)
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        if (!IsGameplay) return; // No pausar si ya morimos o ganamos

        Cursor.visible = true; // Mostrar cursor al pausar
        SetState(GameState.Paused);
        Time.timeScale = 0f; // Congela el motor de física y tiempo
        ShowPanels(pause: true);
    }

    public void ResumeGame()
    {
        // Solo podemos reanudar si estamos en Pausa u Opciones
        if (currentState != GameState.Paused && currentState != GameState.Gameplay) return;
        Cursor.visible = false; // Ocultar cursor al reanudar
        SetState(GameState.Gameplay);
        Time.timeScale = 1f; // Devuelve el tiempo a la normalidad
        ShowPanels(gameplay: true);
    }
    public void RequestGameOver()
    {
        if (!IsGameplay) return;
        Cursor.visible = true; // Mostrar cursor al morir
        if (endRoutine != null) StopCoroutine(endRoutine);
        endRoutine = StartCoroutine(EndRoutine(GameState.GameOver, gameOverDelay));
        Time.timeScale = 0f;
    }

    public void RequestVictory()
    {
        if (!IsGameplay) return;
        Cursor.visible = true; // Mostrar cursor al ganar
        if (scoreManager != null)
        {
            scoreManager.AddVictoryBonus(victoryBonus);

            if (playerHealth != null)
                scoreManager.AddHealthBonus(playerHealth.CurrentHP, healthBonusPerHP);
        }

        if (endRoutine != null) StopCoroutine(endRoutine);
        endRoutine = StartCoroutine(EndRoutine(GameState.Victory, victoryDelay));
        Time.timeScale = 0f;

    }

    private IEnumerator EndRoutine(GameState endState, float delay)
    {
        // 1) Cortamos el gameplay inmediatamente (para que ya no haya input/IA)
        DisableGameplayControllers();

        // 2) Esperamos en tiempo real (no depende de Time.timeScale)
        yield return new WaitForSecondsRealtime(Mathf.Max(0f, delay));

        // 3) Congelamos cámara + mostramos panel final
        SetState(endState);
        if (cameraFollow != null) cameraFollow.SetFrozen(true);

        if (endState == GameState.GameOver) ShowPanels(gameOver: true);
        else ShowPanels(victory: true);

        if (highScoreInputPanel != null) highScoreInputPanel.SetActive(false);

        EvaluateHighScore();
    }

    private void DisableGameplayControllers()
    {
        if (playerController != null)
        {
            playerController.enabled = false;

            Rigidbody2D rb = playerController.GetComponent<Rigidbody2D>();
            if (rb != null) rb.simulated = false;
        }

        for (int i = 0; i < enemyControllers.Count; i++)
            if (enemyControllers[i] != null)
                enemyControllers[i].enabled = false;
    }

    private void SetState(GameState newState)
    {
        currentState = newState;
    }

    private void ShowPanels(bool gameplay = false, bool pause = false, bool gameOver = false, bool victory = false)
    {
        if (hudPanel != null) hudPanel.SetActive(gameplay);
        if (pausePanel != null) pausePanel.SetActive(pause);
        if (gameOverPanel != null) gameOverPanel.SetActive(gameOver);
        if (victoryPanel != null) victoryPanel.SetActive(victory);
    }

    // -------------------------
    // UI Buttons
    // -------------------------
    public void RestartScene()
    {
        Instance = null; // resetea singleton
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        Cursor.visible = true; // Asegura que el cursor esté visible al volver al menú
        SceneManager.LoadScene(0); // Carga la escena con índice 0 (Menú)
    }

    private void EvaluateHighScore()
    {
        if (HighScoreManager.instance == null || scoreManager == null) return;

        int finalScore = scoreManager.GetScore();

        if (HighScoreManager.instance.IsHighScore(finalScore))
        {
            Debug.Log("¡Nuevo High Score!");

            if (highScoreInputPanel != null)
            {
                highScoreInputPanel.SetActive(true);
            }
        }
    }

    public void SubmitHighScore(string playerName)
    {
        if (HighScoreManager.instance == null || scoreManager == null) return;

        int finalScore = scoreManager.GetScore();
        int currentHP = playerHealth != null ? playerHealth.CurrentHP : 0;
        float survivalTime = Time.timeSinceLevelLoad; // Tiempo total de supervivencia en segundos

        int enemiesKilled = FindFirstObjectByType<SpawnManager>()?.TotalEnemiesKilled ?? 0; // Asumiendo que SpawnManager tiene un contador de enemigos eliminados

        HighScoreEntry newEntry = new HighScoreEntry
        {
            playerName = playerName,
            score = finalScore,
            livesRemaining = currentHP,
            timeSurvived = survivalTime,
            enemiesKilled = enemiesKilled
        };


        HighScoreManager.instance.AddHighScore(newEntry);

        BackToMenu();
    }
}
