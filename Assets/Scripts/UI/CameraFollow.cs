using UnityEngine;

/*
 * <summary>
 * Este script hace que la cámara siga al jugador de manera suave, manteniendo una distancia constante.
 * </summary>
 * - La cámara se mueve hacia la posición del jugador con un efecto de suavizado para evitar movimientos bruscos.
 * - El offset inicial se calcula al inicio para mantener la perspectiva establecida en el Editor.
 * - Se puede congelar el seguimiento de la cámara temporalmente mediante el método SetFrozen.
 */

public class CameraFollow : MonoBehaviour
{
    /* <summary>
     * Variables para configurar el objetivo a seguir y la suavidad del movimiento.
     * </summary>
     */
    [Header("Targeting")]
    // El objeto que la cámara debe seguir (normalmente el Jugador)
    [SerializeField] private Transform target;

    // Tiempo aproximado de respuesta. Menos tiempo = cámara más rígida.
    [SerializeField] private float smoothTime = 0.25f;

    [Header("Settings")]
    private Vector3 offset;
    private Vector3 currentVelocity = Vector3.zero; // Referencia interna para el cálculo de inercia

    private bool isFrozen = false;

    private void Start()
    {
        // Calculamos la distancia inicial para mantener la perspectiva elegida en el Editor
        if (target != null)
            offset = transform.position - target.position;
    }

    /* ¿Por qué LateUpdate?
     * En Unity, el movimiento del jugador ocurre en Update o FixedUpdate. 
     * Si moviéramos la cámara en Update, habría frames donde la cámara se mueve ANTES
     * que el jugador, causando que la imagen 'tiemble'. 
     * LateUpdate garantiza que la cámara se mueva después de que el jugador ya terminó su posición en ese frame.
     */

    private void LateUpdate()
    {
        if (isFrozen) return;

        if (target == null) return;

        // Calculamos la posición deseada sumando el desfase original
        Vector3 targetPos = target.position + offset;

        // IMPORTANTE: En 2D/TopDown, no queremos que la cámara cambie su profundidad (Z)
        // ya que podría acercarse o alejarse del suelo y perderíamos el renderizado.
        // esto asegura que la cámara siempre esté a la misma distancia del plano de juego.

        targetPos.z = transform.position.z;

        /*
         * En las propiedades del Rigidbody2D, interpolate debería estar en 'Interpolate' no en 'None'.
         * 1. La Física corre a una frecuencia fija (ej. 50Hz - FixedUpdate).
         * 2. La Cámara corre a los FPS del monitor (ej. 60Hz o 144Hz - Update/LateUpdate).
         * 3. Como los tiempos no coinciden, hay frames donde el objeto visualmente 'salta'.
         *
         * AL ACTIVAR 'INTERPOLATE' en el Rigidbody2D:
         * Unity crea una posición intermedia suavizada para el jugador basada en sus frames 
         * anteriores. Así, cuando SmoothDamp busca al jugador, este siempre está en una 
         * posición fluida, eliminando el jitter por completo.
         */

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref currentVelocity,
            smoothTime
        );
    }

    /* <summary>
     * Método para congelar o descongelar el seguimiento de la cámara.
     * </summary>
     * <param name="frozen">Si es true, la cámara dejará de seguir al jugador; si es false, retomará el seguimiento.</param>
     */
    public void SetFrozen(bool frozen)
    {
        isFrozen = frozen;
    }

}
