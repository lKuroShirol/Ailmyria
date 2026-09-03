using UnityEngine;
using UnityEngine.InputSystem;

public class Burn : MonoBehaviour
{
    private Chispa chispa;
    private Objects_destroyd objeto;

    [SerializeField] private Key teclaQuemar = Key.E;

    private bool jugadorDentro = false;
    private bool isBurning = false;

    private void Awake()
    {
        chispa = FindAnyObjectByType<Chispa>();
        objeto = GetComponentInParent<Objects_destroyd>();
    }

    private void Update()
    {
        if (isBurning)
            return;

        if (!jugadorDentro)
            return;

        if (!chispa.isCasting)
            return;

        if (Keyboard.current[teclaQuemar].wasPressedThisFrame)
        {
            isBurning = true;

            if (objeto != null)
            {
                objeto.DestruirObjeto();

                Debug.Log("Objeto quemado");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorDentro = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorDentro = false;
        }
    }
}