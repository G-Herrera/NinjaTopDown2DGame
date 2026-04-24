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
            AddScore(survivalScorePerTick);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;

        if (score < 0) score = 0;

        UpdateUI();
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
}

