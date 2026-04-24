using UnityEngine;
using TMPro;

public class HighScoreInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerNameInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSubmit()
    {
        string playerName = playerNameInput.text;

        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "No Name";
        }

        GameFlowManager.Instance.SubmitHighScore(playerName);
    }
}
