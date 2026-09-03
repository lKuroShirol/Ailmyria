using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowNPC : MonoBehaviour
{
    [Header("Configuración de Seguimiento")]
    public Transform jugador;
    public float speedMove = 5f;
    public bool estaSiguiendo = false;

    [Tooltip("Distancia máxima en celdas antes de empezar a moverse detrás del jugador.")]
    public int distanciaMaxima = 2;

    [Header("Obstáculos y Referencias (Igual que el Player)")]
    public LayerMask obstaculos;
    public float RadioCirculo;
    public Vector2 offsetPuntoMovimiento;

    private Vector2 puntoMovimiento;
    private bool IsMoving = false;

    [Header("Animación")]
    private Animator animator;
    private Vector2 ultimaDireccion = Vector2.down;


    private void Awake()
    {
        GameObject objetoEncontrado = GameObject.Find("BrujitaO");

        if (objetoEncontrado != null)
        {
            jugador = objetoEncontrado.transform;
            Debug.Log("¡Transform asignado con éxito a: " + objetoEncontrado.name + "!");
        }
    }
    void Start()
    {
        puntoMovimiento = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!estaSiguiendo || jugador == null)
        {
            ActualizarAnimaciones(ultimaDireccion, false);
            return;
        }

        // 1. SI EL NPC YA SE ESTÁ MOVIENDO HACIENDO UN PASO, TERMINAR EL DESPLAZAMIENTO
        if (IsMoving)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                puntoMovimiento,
                speedMove * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, puntoMovimiento) < 0.001f)
            {
                transform.position = puntoMovimiento;
                IsMoving = false;
            }
            return;
        }

        // 2. CALCULAR DISTANCIA EN CELDAS (Aproximación por rejilla / Manhattan)
        // Redondeamos las posiciones a enteros asumiendo que tus celdas miden 1x1 unidad
        Vector2Int posNpcGrid = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        Vector2Int posJugadorGrid = new Vector2Int(Mathf.RoundToInt(jugador.position.x), Mathf.RoundToInt(jugador.position.y));

        int distanciaX = Mathf.Abs(posNpcGrid.x - posJugadorGrid.x);
        int distanciaY = Mathf.Abs(posNpcGrid.y - posJugadorGrid.y);
        int distanciaTotal = distanciaX + distanciaY;

        // 3. SI ESTÁ MÁS LEJOS QUE EL LÍMITE PERMITIDO, DA UN PASO HACIA TI
        if (distanciaTotal > distanciaMaxima)
        {
            Vector2 direccionPaso = Vector2.zero;

            // Prioriza moverse en el eje donde esté más desalineado
            if (distanciaX > distanciaY)
            {
                direccionPaso = new Vector2(System.Math.Sign(posJugadorGrid.x - posNpcGrid.x), 0);
            }
            else
            {
                direccionPaso = new Vector2(0, System.Math.Sign(posJugadorGrid.y - posNpcGrid.y));
            }

            // Calculamos el punto a evaluar para ver si hay obstáculos
            Vector2 puntoEvaluar = (Vector2)transform.position + offsetPuntoMovimiento + direccionPaso;

            bool puedeMoverse = true;
            Collider2D[] collidersDetectados = Physics2D.OverlapCircleAll(puntoEvaluar, RadioCirculo);

            foreach (Collider2D col in collidersDetectados)
            {
                if (((1 << col.gameObject.layer) & obstaculos) != 0)
                {
                    puedeMoverse = false;
                    break;
                }
            }

            if (puedeMoverse)
            {
                puntoMovimiento += direccionPaso;
                IsMoving = true;
                ultimaDireccion = direccionPaso;
                ActualizarAnimaciones(direccionPaso, true);
            }
            else
            {
                // Si hay un obstáculo directo, intenta buscar el eje alternativo para no quedarse trabado
                Vector2 direccionAlternativa = (direccionPaso.x != 0) ? new Vector2(0, System.Math.Sign(posJugadorGrid.y - posNpcGrid.y)) : new Vector2(System.Math.Sign(posJugadorGrid.x - posNpcGrid.x), 0);

                if (direccionAlternativa != Vector2.zero)
                {
                    Vector2 puntoEvaluarAlt = (Vector2)transform.position + offsetPuntoMovimiento + direccionAlternativa;
                    bool puedeAlt = true;
                    Collider2D[] collidersAlt = Physics2D.OverlapCircleAll(puntoEvaluarAlt, RadioCirculo);
                    foreach (Collider2D col in collidersAlt)
                    {
                        if (((1 << col.gameObject.layer) & obstaculos) != 0) { puedeAlt = false; break; }
                    }
                    if (puedeAlt)
                    {
                        puntoMovimiento += direccionAlternativa;
                        IsMoving = true;
                        ultimaDireccion = direccionAlternativa;
                        ActualizarAnimaciones(direccionAlternativa, true);
                        return;
                    }
                }

                ActualizarAnimaciones(ultimaDireccion, false);
            }
        }
        else
        {
            // Si está a 2 celdas o menos, se detiene a esperarte
            ActualizarAnimaciones(ultimaDireccion, false);
        }
    }

    public void AlternarSeguimiento()
    {
        estaSiguiendo = !estaSiguiendo;
        if (estaSiguiendo && jugador != null)
        {
            puntoMovimiento = transform.position;
        }
    }

    public void SetSeguimiento(bool estado)
    {
        estaSiguiendo = estado;
        if (estaSiguiendo && jugador != null)
        {
            puntoMovimiento = transform.position;
        }
    }

    void ActualizarAnimaciones(Vector2 dir, bool moviendose)
    {
        if (animator == null) return;

        animator.SetBool("IsMoving", moviendose);

        if (dir.x != 0)
        {
            animator.SetFloat("MoveX", dir.x);
            animator.SetFloat("MoveY", 0f);
        }
        else if (dir.y != 0)
        {
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", dir.y);
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 centroGizmo = Application.isPlaying ? puntoMovimiento : (Vector2)transform.position;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(centroGizmo + offsetPuntoMovimiento, RadioCirculo);
    }
}
