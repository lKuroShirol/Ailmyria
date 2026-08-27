using UnityEngine;

public class CamaraBordes : MonoBehaviour
{
    [Header("Seguimiento")]
    public Transform objetivoActual; // El objetivo que la cámara está siguiendo en este momento
    public Transform jugador;        // Referencia a la Brujita
    public Transform objetoEditando; // Referencia al tronco/objeto personalizable
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    public float velocidadSuavizado = 5f;

    [Header("Límites exactos del mapa (Bordes visibles)")]
    public float minX; // Coordenada X del borde izquierdo de tu mapa
    public float maxX; // Coordenada X del borde derecho de tu mapa
    public float minY; // Coordenada Y del borde inferior de tu mapa
    public float maxY; // Coordenada Y del borde superior de tu mapa

    private Camera camara;
    private float mitadAnchoCamara;
    private float mitadAltoCamara;

    void Start()
    {
        camara = GetComponent<Camera>();

        mitadAltoCamara = camara.orthographicSize;
        mitadAnchoCamara = mitadAltoCamara * camara.aspect;

        // Por defecto, al iniciar el juego, el objetivo es el jugador
        if (jugador != null && objetivoActual == null)
        {
            objetivoActual = jugador;
        }
    }

    void LateUpdate()
    {
        if (objetivoActual == null) return;

        Vector3 posicionDeseada = objetivoActual.position + offset;

        // Ajustamos los límites sumando y restando el tamaño de la cámara para que 
        // el BORDE de la pantalla toque exactamente tu coordenada límite, no el centro.
        float minLimiteX = minX + mitadAnchoCamara;
        float maxLimiteX = maxX - mitadAnchoCamara;
        float minLimiteY = minY + mitadAltoCamara;
        float maxLimiteY = maxY - mitadAltoCamara;

        // Aplicamos el clamp con los límites corregidos
        float clampedX = Mathf.Clamp(posicionDeseada.x, minLimiteX, maxLimiteX);
        float clampedY = Mathf.Clamp(posicionDeseada.y, minLimiteY, maxLimiteY);

        Vector3 posicionFinal = new Vector3(clampedX, clampedY, posicionDeseada.z);

        transform.position = Vector3.Lerp(transform.position, posicionFinal, velocidadSuavizado * Time.deltaTime);
    }

    // Métodos públicos para cambiar el objetivo desde el script de Modo Edición
    public void SeguirJugador()
    {
        if (jugador != null) objetivoActual = jugador;
    }

    public void SeguirObjeto()
    {
        if (objetoEditando != null) objetivoActual = objetoEditando;
    }

    // El Gizmo dibuja exactamente el área visible que el objetivo podrá ver
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 centroMapa = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0f);
        Vector3 tamañoMapa = new Vector3(maxX - minX, maxY - minY, 0f);

        Gizmos.DrawWireCube(centroMapa, tamañoMapa);
    }
}
