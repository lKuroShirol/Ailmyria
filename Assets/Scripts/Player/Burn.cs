using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;

public class Burn : MonoBehaviour
{
    private Chispa chispa;
    private Objects_destroyd objeto;

    [SerializeField] private Key teclaQuemar = Key.E;

    private bool jugadorDentro = false;
    private bool isBurning = false;

    [SerializeField] private GameObject tecla;

    private void Awake()
    {
        chispa = FindAnyObjectByType<Chispa>();
        objeto = GetComponentInParent<Objects_destroyd>();

        tecla.SetActive(false);
    }

    private void Update()
    {
        if (isBurning)
        {
            tecla.SetActive(false);
            return;
        }

        if (!jugadorDentro)
        {
            tecla.SetActive(false);
            return;
        }

        // La tecla solo aparece con la Chispa encendida
        if (!chispa.isCasting)
        {
            tecla.SetActive(false);
            return;
        }

        tecla.SetActive(true);

        if (Keyboard.current[teclaQuemar].wasPressedThisFrame)
        {
            isBurning = true;
            tecla.SetActive(false);

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
            tecla.SetActive(false);
        }
    }
}

