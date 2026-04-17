using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;

    private void Awake()
    {
        if (_instance != null) 
        { 
            Destroy(gameObject); // Evita que haya más de una instancia del MusicManager
        } 
        else 
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene este objeto vivo entre escenas
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
