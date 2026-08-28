using UnityEngine;
using UnityEngine.InputSystem;

public class BrujitaFlight : MonoBehaviour
{
    [Header("Configuración del Vuelo")]
    public GameObject objetoVuelo;

    [Header("Desbloqueo")]
    public bool vueloDesbloqueado = false;

    [Header("Límite de Vuelo")]
    public LayerMask limiteVuelo;
    public float radioDeteccion = 0.2f;

    [Header("Aterrizaje")]
    public LayerMask obstaculos;

    public bool IsFlying { get; private set; }

    // Indica si la brujita está dentro de una DepthZone
    public bool EstaEnDepthZone { get; private set; }

    private void Start()
    {
        IsFlying = false;
        EstaEnDepthZone = false;

        if (objetoVuelo != null)
        {
            objetoVuelo.SetActive(false);
        }
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ToggleFlight();
        }
    }

    public void ToggleFlight()
    {
        if (!IsFlying)
        {
            // Todavía no tiene desbloqueada la habilidad
            if (!vueloDesbloqueado)
            {
                return;
            }

            // No puede activar vuelo dentro de DepthZone
            if (EstaEnDepthZone)
            {
                return;
            }

            IsFlying = true;
        }
        else
        {
            // Si está tocando un obstáculo,
            // no puede aterrizar.
            if (!PuedeAterrizar())
            {
                return;
            }

            IsFlying = false;
        }

        if (objetoVuelo != null)
        {
            objetoVuelo.SetActive(IsFlying);
        }
    }

    public bool PuedeMoverseVolando(Vector2 posicion)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            posicion,
            radioDeteccion,
            limiteVuelo
        );

        return colliders.Length == 0;
    }

    public bool PuedeAterrizar()
    {
        // No puede aterrizar dentro de una DepthZone
        if (EstaEnDepthZone)
        {
            return false;
        }

        // No puede aterrizar sobre Obstaculos
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position,
            radioDeteccion,
            obstaculos
        );

        return colliders.Length == 0;
    }

    public void EntrarDepthZone()
    {
        EstaEnDepthZone = true;
    }

    public void SalirDepthZone()
    {
        EstaEnDepthZone = false;
    }
}