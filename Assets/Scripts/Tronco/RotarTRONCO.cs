using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoverYRotarObjetoGrid : MonoBehaviour
{
    [Header("Configuración de la Cuadrícula")]
    [Tooltip("El tamaño de cada celda (por ejemplo, 1 si tus casillas miden 1x1).")]
    public float tamañoCelda = 1f;

    [Header("Configuración de Rotación")]
    public float gradosPorPaso = 90f;
    public Key teclaRotar = Key.R;

    [Tooltip("Elige si quieres que rote en el Eje Z (2D plano), Horizontal (Y) o Vertical (X).")]
    public bool rotarEnEjeZPlano2D = true;

    [Header("Restricciones opcionales")]
    public LayerMask obstaculosParaColocar;
    public float radioDeteccion = 0.4f;

    private bool estaArrastrando = false;
    private bool estaSeleccionado = false; 
    private Camera camaraPrincipal;

    void Start()
    {
        camaraPrincipal = Camera.main;
    }

    void Update()
    {
        if (camaraPrincipal == null) return;

        
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 posicionMouseMundo = camaraPrincipal.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            Collider2D colisionador = Physics2D.OverlapPoint(posicionMouseMundo);
            if (colisionador != null && colisionador.gameObject == gameObject)
            {
                estaArrastrando = true;
                estaSeleccionado = true; 
            }
            else
            {
              
            }
        }

       
        if (estaArrastrando)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                Vector2 posicionMouseMundo = camaraPrincipal.ScreenToWorldPoint(Mouse.current.position.ReadValue());

                float posX = Mathf.Round(posicionMouseMundo.x / tamañoCelda) * tamañoCelda;
                float posY = Mathf.Round(posicionMouseMundo.y / tamañoCelda) * tamañoCelda;
                Vector2 nuevaPosicionGrid = new Vector2(posX, posY);

                if (obstaculosParaColocar.value == 0 || !Physics2D.OverlapCircle(nuevaPosicionGrid, radioDeteccion, obstaculosParaColocar))
                {
                    transform.position = nuevaPosicionGrid;
                }
            }
            else
            {
              
                estaArrastrando = false;
            }
        }

     
        if (estaSeleccionado && Keyboard.current != null)
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
    }
}
