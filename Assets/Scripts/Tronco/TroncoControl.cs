using UnityEngine;
using UnityEngine.InputSystem;

public class TroncoControl : MonoBehaviour
{
    [Header("Referencias Clave")]
    public Transform player;                  
    public Transform objetoPersonalizable;    
    public CamaraBordes scriptCamara;        

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

        
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame || Keyboard.current.rightShiftKey.wasPressedThisFrame)
        {
            modoEdicionActivo = !modoEdicionActivo;
            CambiarModo(modoEdicionActivo);
        }

        
        if (modoEdicionActivo && objetoPersonalizable != null)
        {
            MoverObjetoConTeclado();
        }

        
        
            if (Keyboard.current[teclaRotar].wasPressedThisFrame)
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

        if (Keyboard.current != null)
        {
            if (Keyboard.current[teclaRotar].wasPressedThisFrame)
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

        // Detectar flechas o WASD
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) direccion = Vector2.up;
        else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) direccion = Vector2.down;
        else if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) direccion = Vector2.left;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) direccion = Vector2.right;

        if (direccion != Vector2.zero)
        {
            Vector3 nuevaPosicion = objetoPersonalizable.position + (Vector3)direccion * tamañoCelda;

            // Ajuste estricto al grid
            float posX = Mathf.Round(nuevaPosicion.x / tamañoCelda) * tamañoCelda;
            float posY = Mathf.Round(nuevaPosicion.y / tamañoCelda) * tamañoCelda;
            objetoPersonalizable.position = new Vector3(posX, posY, objetoPersonalizable.position.z);

            tiempoSiguienteMovimiento = Time.time + velocidadMovimientoTeclado;
        }
    }
}
