using UnityEngine;
using UnityEngine.InputSystem;

public class TroncoControl : MonoBehaviour
{
    [Header("Referencias Clave")]
    public Transform player;
    public Transform objetoPersonalizable;
    public CamaraBordes scriptCamara;

    [Header("Configuración de Zona")]
    [Tooltip("Distancia máxima a la que la brujita debe estar para ACTIVAR la edición por primera vez.")]
    public float distanciaInteraccion = 2f;

    [Header("Configuración de Celdas")]
    public float tamañoCelda = 1f;
    public float velocidadMovimientoTeclado = 0.15f;
    private float tiempoSiguienteMovimiento = 0f;

    [Header("Configuración de Rotación")]
    public float gradosPorPaso = 90f;
    public Key teclaRotar = Key.R;
    [Tooltip("Elige si quieres que rote en el Eje Z (2D plano), Horizontal (Y) o Vertical (X).")]
    public bool rotarEnEjeZPlano2D = true;

    private bool modoEdicionActivo = false;
    private BrujitaMove scriptMovimientoPlayer;

    void Start()
    {
        if (player != null)
        {
            scriptMovimientoPlayer = player.GetComponent<BrujitaMove>();
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // Detectar Shift
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame || Keyboard.current.rightShiftKey.wasPressedThisFrame)
        {
            if (!modoEdicionActivo)
            {
                // INTENTO DE ACTIVAR: Exigimos que la brujita esté cerca del tronco
                if (EstaJugadorEnArea())
                {
                    modoEdicionActivo = true;
                    CambiarModo(modoEdicionActivo);
                }
                else
                {
                    Debug.Log("--- Estás muy lejos del tronco para comenzar a editar ---");
                }
            }
            else
            {
                // INTENTO DE DESACTIVAR: Se puede apagar en cualquier momento, sin importar dónde esté el tronco
                modoEdicionActivo = false;
                CambiarModo(modoEdicionActivo);
            }
        }

        // Movimiento en modo edición (funciona con libertad total de distancia)
        if (modoEdicionActivo && objetoPersonalizable != null)
        {
            MoverObjetoConTeclado();
        }

        // Rotación con la tecla asignada
        if (modoEdicionActivo && Keyboard.current[teclaRotar].wasPressedThisFrame)
        {
            if (rotarEnEjeZPlano2D)
            {
                transform.Rotate(0f, 0f, gradosPorPaso, Space.Self);
            }
            else
            {
                transform.Rotate(0f, gradosPorPaso, 0f, Space.Self);
            }
        }
    }

    // Comprueba si el jugador está cerca del tronco (medido respecto a la posición original del tronco/script)
    bool EstaJugadorEnArea()
    {
        if (player == null) return false;

        float distanciaActual = Vector2.Distance(transform.position, player.position);
        return distanciaActual <= distanciaInteraccion;
    }

    void CambiarModo(bool editando)
    {
        if (scriptMovimientoPlayer != null)
        {
            scriptMovimientoPlayer.enabled = !editando;
        }

        if (scriptCamara != null)
        {
            if (editando)
            {
                scriptCamara.SeguirObjeto();
                Debug.Log("--- MODO EDICIÓN: Cámara enfocando al tronco ---");
            }
            else
            {
                scriptCamara.SeguirJugador();
                Debug.Log("--- MODO JUEGO: Cámara enfocando a la Brujita ---");
            }
        }
    }

    void MoverObjetoConTeclado()
    {
        if (Time.time < tiempoSiguienteMovimiento) return;

        Vector2 direccion = Vector2.zero;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) direccion = Vector2.up;
        else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) direccion = Vector2.down;
        else if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) direccion = Vector2.left;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) direccion = Vector2.right;

        if (direccion != Vector2.zero)
        {
            Vector3 nuevaPosicion = objetoPersonalizable.position + (Vector3)direccion * tamañoCelda;

            float posX = Mathf.Round(nuevaPosicion.x / tamañoCelda) * tamañoCelda;
            float posY = Mathf.Round(nuevaPosicion.y / tamañoCelda) * tamañoCelda;
            objetoPersonalizable.position = new Vector3(posX, posY, objetoPersonalizable.position.z);

            tiempoSiguienteMovimiento = Time.time + velocidadMovimientoTeclado;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaInteraccion);
    }
}
