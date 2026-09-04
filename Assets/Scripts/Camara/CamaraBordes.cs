using UnityEngine;

public class CamaraBordes : MonoBehaviour
{
    [Header("Seguimiento")]
    public Transform objetivoActual;
    public Transform jugador;
    public Transform objetoEditando;
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    public float velocidadSuavizado = 5f;

    [Header("Límites exactos del mapa (Bordes visibles)")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private Camera camara;
    private float mitadAnchoCamara;
    private float mitadAltoCamara;

    private void Awake()
    {
        objetivoActual = GameObject.FindGameObjectWithTag("Player")?.transform;
        jugador = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Start()
    {
        camara = GetComponent<Camera>();

        ActualizarTamañoCamara();

        if (jugador != null && objetivoActual == null)
        {
            objetivoActual = jugador;
        }
    }

    private void LateUpdate()
    {
        if (objetivoActual == null)
            return;

        // Actualizamos por si otro script cambió el orthographicSize
        ActualizarTamañoCamara();

        Vector3 posicionDeseada = objetivoActual.position + offset;

        float minLimiteX = minX + mitadAnchoCamara;
        float maxLimiteX = maxX - mitadAnchoCamara;

        float minLimiteY = minY + mitadAltoCamara;
        float maxLimiteY = maxY - mitadAltoCamara;

        float clampedX = Mathf.Clamp(posicionDeseada.x, minLimiteX, maxLimiteX);
        float clampedY = Mathf.Clamp(posicionDeseada.y, minLimiteY, maxLimiteY);

        Vector3 posicionFinal = new Vector3(
            clampedX,
            clampedY,
            posicionDeseada.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            posicionFinal,
            velocidadSuavizado * Time.deltaTime
        );
    }

    private void ActualizarTamañoCamara()
    {
        mitadAltoCamara = camara.orthographicSize;
        mitadAnchoCamara = mitadAltoCamara * camara.aspect;
    }

    public void SeguirJugador()
    {
        if (jugador != null)
            objetivoActual = jugador;
    }

    public void SeguirObjeto()
    {
        if (objetoEditando != null)
            objetivoActual = objetoEditando;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 centroMapa = new Vector3(
            (minX + maxX) / 2f,
            (minY + maxY) / 2f,
            0f
        );

        Vector3 tamañoMapa = new Vector3(
            maxX - minX,
            maxY - minY,
            0f
        );

        Gizmos.DrawWireCube(centroMapa, tamañoMapa);
    }
}