using UnityEngine;
using UnityEngine.EventSystems;

public class BotonDesapear : MonoBehaviour
{
    [Header("Imagen que aparecerá")]
    [Tooltip("Arrastra aquí la imagen (UI Image) que quieres que aparezca al lado.")]
    public GameObject imagenAsociada;

    void Start()
    {
        // Asegurarnos de que la imagen esté oculta al iniciar el juego
        if (imagenAsociada != null)
        {
            imagenAsociada.SetActive(false);
        }
    }

    // Se ejecuta cuando el ratón ENTRA en el botón
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (imagenAsociada != null)
        {
            imagenAsociada.SetActive(true);
        }
    }

    // Se ejecuta cuando el ratón SALE del botón
    public void OnPointerExit(PointerEventData eventData)
    {
        if (imagenAsociada != null)
        {
            imagenAsociada.SetActive(false);
        }
    }
}

