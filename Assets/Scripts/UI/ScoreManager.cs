using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text finalScoreText;

    [Header("Current Score")]    
    [SerializeField] private int score = 0;

    [Header("Survival Score")]
    [Tooltip("PUntos ganados por seguir vivo durante el gameplay")]
    [SerializeField] private int survivalScorePerTick = 1;

    [Tooltip("Cada cuántos segundos se suman puntos por supervivencia")]
    [SerializeField] private float survivalTickInterval = 1.0f;

    [Header("Popups")]
    [Tooltip("Prefab de popup para mostrar puntos ganados o perdidos")]
    [SerializeField] private GameObject scorePopupPrefab;
    [Tooltip("Canvas donde se instanciarán los popups")]
    [SerializeField] private Canvas uiCanvas; // Canvas donde se instanciarán los popups
    [Tooltip("RectTransform del área donde se mostrarán los popups (opcional)")]
    [SerializeField] private RectTransform scoreRect;

    private float survivalTimer = 0f;

    public int CurrentScore => score;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (GameFlowManager.Instance == null) return;
        if (!GameFlowManager.Instance.IsGameplay) return;

        survivalTimer += Time.deltaTime;

        if (survivalTimer >= survivalTickInterval)
        {
            survivalTimer -= survivalTickInterval;
            AddScore(survivalScorePerTick, false);
        }
    }

    public void AddScore(int amount, bool showPopup = true)
    {
        score += amount;

        if (score < 0) score = 0;

        UpdateUI();
        if ( showPopup) SpawnPopup(amount);
    }

    public void AddVictoryBonus(int amount)
    {
        AddScore(amount);
    }

    public void AddHealthBonus(int currentHP, int pointsPerHP)
    {
        if (currentHP <= 0) return;
        if (pointsPerHP <= 0) return;

        int bonus = currentHP * pointsPerHP;
        AddScore(bonus);
    }

    public int GetScore() => score;

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"{score}";
        if (finalScoreText != null)
            finalScoreText.text = $"{score}";
    }

    private void SpawnPopup(int amount)
    {
        if (scorePopupPrefab == null || uiCanvas == null || scoreRect == null)  return;
        // Instanciar el popup como hijo del canvas para que se posicione correctamente en la UI
        GameObject popupInstance = Instantiate(scorePopupPrefab, uiCanvas.transform);

        Vector3 worldPos = scoreRect.position;

        Vector3 offset = new Vector3(Random.Range(-100, 100), Random.Range(-80, -20), 0); // Offset aleatorio para evitar superposición
        popupInstance.transform.position = worldPos + offset;

        ScorePopup popupScript = popupInstance.GetComponent<ScorePopup>();
        if (popupScript != null)
            popupScript.Setup(amount);
    }
}

