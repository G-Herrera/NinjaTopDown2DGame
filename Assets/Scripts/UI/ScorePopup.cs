using UnityEngine;
using TMPro;


public class ScorePopup : MonoBehaviour
{
    [Header("Popup Settings")]
    [SerializeField] private float moveSpeed = 1.5f;           // Speed at which the popup moves upwards
    [SerializeField] private float lifeTime = 1.0f;             // Time before the popup is destroyed

    private TextMeshProUGUI txtScorePopup;
    private Color color;

    private void Awake()
    {
        txtScorePopup = GetComponent<TextMeshProUGUI>();
        color = txtScorePopup.color; // Store the original color
    }

    public void Setup( int amount)
    {
        txtScorePopup.text = (amount > 0 ? "+" : "") + amount.ToString(); // Show + for positive scores

       //Color based on score type (positive = green, negative = red)
        if (amount > 0)
            color = Color.green;
        else if (amount < 0)
            color = Color.red;
        else
            color = txtScorePopup.color; // Default color for zero

        Destroy(gameObject, lifeTime); // Destroy after lifeTime seconds
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Move the popup upwards over time
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        //Fade out the popup over its lifetime
        color.a -= Time.deltaTime / lifeTime; // Reduce alpha over time
        txtScorePopup.color = color; // Apply the new color with updated alpha
    }
}
