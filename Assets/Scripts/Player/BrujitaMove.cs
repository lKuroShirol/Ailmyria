using UnityEngine;
using UnityEngine.InputSystem;

public class BrujitaMove : MonoBehaviour
{
    [Header("Movimiento")]
    public float speedMove;

    private Vector2 puntoMovimiento;
    public Vector2 offsetPuntoMovimiento;

    [Header("Obstáculos")]
    public LayerMask obstaculos;
    public float RadioCirculo;

    private bool IsMoving = false;
    private Vector2 direction;

    [Header("Control del Libro / Menú")]
    public bool libroAbierto = false;

    [Header("Animación")]
    private Animator animator;
    private Vector2 ultimaDireccion = Vector2.down;

    private BrujitaFlight vuelo;

    void Start()
    {
        puntoMovimiento = transform.position;

        animator = GetComponent<Animator>();
        vuelo = GetComponent<BrujitaFlight>();
    }

    void Update()
    {
        if (libroAbierto)
        {
            ActualizarAnimaciones(ultimaDireccion, false);
            return;
        }

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

        direction = Vector2.zero;

        if (Keyboard.current != null)
        {
            float moveX =
                (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1f : 0f) +
                (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? -1f : 0f);

            float moveY =
                (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed ? 1f : 0f) +
                (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed ? -1f : 0f);

            if (moveX != 0)
            {
                direction = new Vector2(moveX, 0);
            }
            else if (moveY != 0)
            {
                direction = new Vector2(0, moveY);
            }
        }

        if (direction != Vector2.zero)
        {
            Vector2 puntoEvaluar =
                (Vector2)transform.position +
                offsetPuntoMovimiento +
                direction;

            bool estaVolando = vuelo != null && vuelo.IsFlying;

            bool puedeMoverse = false;

            if (estaVolando)
            {
                puedeMoverse = vuelo.PuedeMoverseVolando(puntoEvaluar);
            }

            else
            {
                Collider2D[] collidersDetectados =
                    Physics2D.OverlapCircleAll(
                        puntoEvaluar,
                        RadioCirculo
                    );

                bool hayObstaculo = false;
                bool haySueloLibre = false;

                foreach (Collider2D col in collidersDetectados)
                {
                    if (((1 << col.gameObject.layer) & obstaculos) != 0)
                    {
                        hayObstaculo = true;
                        print("Bloqueado");
                    }

                    if (col.gameObject.layer == LayerMask.NameToLayer("Libre"))
                    {
                        haySueloLibre = true;
                    }
                }

                if (haySueloLibre)
                {
                    puedeMoverse = true;
                }
                else if (hayObstaculo)
                {
                    puedeMoverse = false;
                }
                else
                {
                    puedeMoverse = true;
                }
            }

            if (puedeMoverse)
            {
                puntoMovimiento += direction;
                IsMoving = true;

                ultimaDireccion = direction;

                ActualizarAnimaciones(direction, true);
            }
            else
            {
                ActualizarAnimaciones(direction, false);
            }
        }
        else
        {
            ActualizarAnimaciones(ultimaDireccion, false);
        }
    }

    void ActualizarAnimaciones(Vector2 dir, bool moviendose)
    {
        if (animator == null)
            return;

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
        Vector2 centroGizmo = Application.isPlaying
            ? puntoMovimiento
            : (Vector2)transform.position;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            centroGizmo + offsetPuntoMovimiento,
            RadioCirculo
        );
    }
}