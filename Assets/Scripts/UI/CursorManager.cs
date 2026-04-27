using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false; // Oculta el cursor del sistema operativo
   
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.position = Input.mousePosition; // Mueve el cursor personalizado a la posición del mouse
    }

 
}
