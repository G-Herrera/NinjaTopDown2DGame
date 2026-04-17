using System;//Utilizado para trabajar con textos en Unity.
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;//Utilizado para la gestión de entradas y controles en Unity.
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public enum UIState
    {
        MainMenu,
        Options,
    }

    [Header("Paneles")]//Encabezado para organizar las variables en el inspector de Unity.

    [SerializeField] private GameObject panelMainMenu;//Panel del menú principal. Se declara una variable privada para el panel del menú principal.
    [SerializeField] private GameObject panelOptions;//Panel de opciones. Se declara una variable privada para el panel de opciones.

    [Header("debug")]

    [SerializeField] private TextMeshProUGUI txtStateDebug;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip startGameSound;

    [Header("Start Delay")]
    [SerializeField] private float startDelay = 1.98f;

    private bool isStarting = false;

    private UIState currentState;//Estado actual de la interfaz de usuario. Se declara una variable privada para almacenar el estado actual de la UI.

    private void Awake()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        ChangeState(UIState.MainMenu);//Inicia el estado de la UI en el menú principal.
   
    }

    public void ChangeState(UIState nextState)
    {
        currentState = nextState;

        // --------------------------
        // Desactivar todos los paneles y player/enemy
        // --------------------------
        if (panelMainMenu != null) panelMainMenu.SetActive(false);
        if (panelOptions != null) panelOptions.SetActive(false);
       
        switch (currentState)
        {
            case UIState.MainMenu:
                if (panelMainMenu != null)
                {
                    panelMainMenu.SetActive(true);
                    // Asegura que el MainMenu quede al frente en la jerarquía para recibir clicks
                    panelMainMenu.transform.SetAsLastSibling();
                }
                break;

            case UIState.Options:
                if (panelOptions != null)
                {
                    panelOptions.SetActive(true);
                    panelOptions.transform.SetAsLastSibling();
                }
                break;

        }

        // --------------------------
        // Debug (opcional)
        // --------------------------
        if (txtStateDebug != null)
            txtStateDebug.text = "Current State: " + currentState.ToString();
    }

    public void OnClickStart()
    {
        if (isStarting) return;
        isStarting = true;

        StartCoroutine(StartGameRoutine());
    }

    public void OnClickOptions()
    {
        ChangeState(UIState.Options);
    }

    public void OnClickBackToMenu()
    {
        ChangeState(UIState.MainMenu);
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private System.Collections.IEnumerator StartGameRoutine()
    {
        //Reproducir sonido
        if (audioSource != null && startGameSound != null)
        {
            audioSource.PlayOneShot(startGameSound);
        }

        //Esperar
        yield return new WaitForSeconds(startDelay);

        //Cargar escena
        SceneManager.LoadScene(1);
    }
}
