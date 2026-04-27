using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;//Utilizado para la gestión de entradas y controles en Unity.
using TMPro;//Utilizado para trabajar con textos en Unity.
using System.Collections;

public class UIStateManager : MonoBehaviour
{
    public enum UIState
    {
        MainMenu,
        Options,
        Gameplay,
        Pause,
        GameOver,
        HighScoreEntry
    }

    [Header("Paneles")]//Encabezado para organizar las variables en el inspector de Unity.

    [SerializeField] private GameObject panelMainMenu;//Panel del menú principal. Se declara una variable privada para el panel del menú principal.
    [SerializeField] private GameObject panelOptions;//Panel de opciones. Se declara una variable privada para el panel de opciones.
    [SerializeField] private GameObject panelHUD;//Panel de la interfaz de usuario durante el juego. Se declara una variable privada para el panel HUD.
    [SerializeField] private GameObject panelPause;//Panel de pausa. Se declara una variable privada para el panel de pausa. La sintaxis [SerializeField] permite que estas variables sean asignadas desde el editor de Unity.
    [SerializeField] private GameObject panelGameOver;//Panel de game over. Se declara una variable privada para el panel. 
    [SerializeField] private GameObject panelHighScoreInput;//Panel de ingreso de puntuación alta. Se declara una variable privada para el panel de ingreso de puntuación alta.

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;

    [Header("debug")]

    [SerializeField] private TextMeshProUGUI txtStateDebug;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clipButtonStart;
    [SerializeField] private float startDelay = 1.5f;


    private UIState currentState;//Estado actual de la interfaz de usuario. Se declara una variable privada para almacenar el estado actual de la UI.

    private void Start()
    {
        ChangeState(UIState.MainMenu);//Inicia el estado de la UI en el menú principal.
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if(Keyboard.current!=null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (currentState == UIState.Gameplay)
            {
                ChangeState(UIState.Pause);
                Time.timeScale = 0f;
            }
            else if(currentState==UIState.Pause) 
            {
                ChangeState(UIState.Gameplay);
                Time.timeScale = 1f;
            }
        }
    }
    public void ChangeState(UIState nextState)
    {
        currentState = nextState;

        // --------------------------
        // Desactivar todos los paneles y player/enemy
        // --------------------------
        if (panelMainMenu != null) panelMainMenu.SetActive(false);
        if (panelOptions != null) panelOptions.SetActive(false);
        if (panelHUD != null) panelHUD.SetActive(false);
        if (panelPause != null) panelPause.SetActive(false);
        if (panelGameOver != null) panelGameOver.SetActive(false);
        if (panelHighScoreInput != null) panelHighScoreInput.SetActive(false);

        if (player != null) player.SetActive(false);
        if (enemy != null) enemy.SetActive(false);

        // --------------------------
        // Activar solo el panel correspondiente
        // --------------------------
        switch (currentState)
        {
            case UIState.MainMenu:
                if (panelMainMenu != null)
                {
                    Cursor.visible = true; // Muestra el cursor en el menú principal
                    panelMainMenu.SetActive(true);
                    // Asegura que el MainMenu quede al frente en la jerarquía para recibir clicks
                    panelMainMenu.transform.SetAsLastSibling();
                }
                break;

            case UIState.Options:
                if (panelOptions != null)
                {
                    Cursor.visible = true; // Muestra el cursor en opciones
                    panelOptions.SetActive(true);
                    panelOptions.transform.SetAsLastSibling();
                }
                break;

            case UIState.Gameplay:
                if (panelHUD != null)
                {
                    Cursor.visible = false; // Oculta el cursor durante el juego
                    panelHUD.SetActive(true);
                    panelHUD.transform.SetAsFirstSibling(); // HUD debajo de menús
                }
                if (player != null) player.SetActive(true);
                if (enemy != null) enemy.SetActive(true);
                break;

            case UIState.Pause:
                if (panelPause != null)
                {
                    Cursor.visible = true; // Muestra el cursor al pausar
                    panelPause.SetActive(true);
                    panelPause.transform.SetAsLastSibling();
                }
                break;

            case UIState.GameOver:
                if (panelGameOver != null)
                {
                    Cursor.visible = true; // Muestra el cursor en Game Over
                    panelGameOver.SetActive(true);
                    panelGameOver.transform.SetAsLastSibling();
                }
                break;
            case UIState.HighScoreEntry:
                if (panelHighScoreInput != null)
                {
                    Cursor.visible = true; // Muestra el cursor al ingresar puntuación alta
                    panelHighScoreInput.SetActive(true);
                    panelHighScoreInput.transform.SetAsLastSibling();
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
        Cursor.visible = false; // Oculta el cursor al iniciar el juego
        StartCoroutine(StartGameAfterDelay());
    }

    public void OnClickOptions()
    {
        ChangeState(UIState.Options);
    }

    public void OnClickBackToMenu()
    {
        // Reinicia tiempo real
        Time.timeScale = 1f;

        // Reinicia la escena principal (MainMenu) o la actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Asegúrate que el UIStateManager esté activo en el MainMenu
    }

    public void OnClickPause()
    {
        ChangeState(UIState.Pause);
        Cursor.visible = true; // Muestra el cursor al pausar
        Time.timeScale = 0f;
    }

    public void OnClickResume()
    {
        ChangeState(UIState.Gameplay);
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void OnClickExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private IEnumerator StartGameAfterDelay()
    {
        if (audioSource != null && clipButtonStart != null)
        {
            audioSource.PlayOneShot(clipButtonStart);
        }
        yield return new WaitForSecondsRealtime(startDelay);
        ChangeState(UIState.Gameplay);
        Time.timeScale = 1f;
    }
}
