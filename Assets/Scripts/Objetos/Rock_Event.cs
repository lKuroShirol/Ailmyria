using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Rock_Event : MonoBehaviour
{
    [Header("Cambio de Sprite")]
    [SerializeField] private GameObject objetoFondo;
    [SerializeField] private Sprite spriteNuevo;

    private SpriteRenderer spriteFondo;
    private Objects_destroyd objetoDestruible;

    [Header("Panel")]
    [SerializeField] private GameObject Panel;
    [SerializeField] private CanvasGroup panelFade;

    [Header("Sonido")]
    [SerializeField] private AudioSource audioSource;

    [Header("Fade")]
    [SerializeField] private float duracionFade = 0.5f;

    private BrujitaMove brujitaMove;

    private bool eventoActivo = false;

    [SerializeField] private GameObject escoba1;
    [SerializeField] private GameObject escoba2;
    [Header("ID Escoba")]
    [SerializeField] private int idEscoba1;

    private void Awake()
    {
        objetoDestruible = GetComponent<Objects_destroyd>();

        brujitaMove = FindAnyObjectByType<BrujitaMove>();

        if (objetoFondo != null)
        {
            spriteFondo = objetoFondo.GetComponent<SpriteRenderer>();
        }

        if (Panel != null)
        {
            Panel.SetActive(false);
        }

        ComprobarEstado();

        Manager_Objects manager = FindFirstObjectByType<Manager_Objects>();

        if (manager != null)
        {
            string escena = SceneManager.GetActiveScene().name;

            if (manager.EstaDestruido(escena, idEscoba1))
            {
                escoba1.SetActive(false);
                escoba2.SetActive(true);
            }
            else
            {
                escoba1.SetActive(true);
                escoba2.SetActive(false);
            }
        }
    }

    //private void Update()
    //{
    //    if (Keyboard.current.rKey.wasPressedThisFrame)
    //    {
    //        if (objetoDestruible != null)
    //        {
    //            objetoDestruible.DestruirObjeto();
    //        }
    //    }
    //}

    private void ComprobarEstado()
    {
        if (objetoDestruible == null)
            return;

        Manager_Objects manager = FindFirstObjectByType<Manager_Objects>();

        if (manager == null)
            return;

        string escena = SceneManager.GetActiveScene().name;

        if (manager.EstaDestruido(escena, objetoDestruible.Id))
        {
            CambiarSprite();
        }
    }

    public void EventRoca()
    {
        if (eventoActivo)
            return;

        StartCoroutine(EventoRoca());
    }

    private IEnumerator EventoRoca()
    {
        eventoActivo = true;

        if (brujitaMove != null)
        {
            brujitaMove.enabled = false;
        }

        if (Panel != null)
        {
            Panel.SetActive(true);
        }

        yield return StartCoroutine(FadePanel(0f, 1f));

        Manager_Objects manager = FindFirstObjectByType<Manager_Objects>();

        if (manager != null)
        {
            manager.RegistarDestruccion(
                SceneManager.GetActiveScene().name,
                idEscoba1
            );
        }

        escoba1.SetActive(false);
        escoba2.SetActive(true);
      
        CambiarSprite();

        if (audioSource != null)
        {
            audioSource.Play();

            yield return new WaitWhile(() => audioSource.isPlaying);
        }

  
        SpriteRenderer spriteRoca = GetComponent<SpriteRenderer>();

        if (spriteRoca != null)
        {
            spriteRoca.enabled = false;
        }

     
        yield return StartCoroutine(FadePanel(1f, 0f));

        if (Panel != null)
        {
            Panel.SetActive(false);
        }

        if (brujitaMove != null)
        {
            brujitaMove.enabled = true;
        }

        eventoActivo = false;

        Destroy(gameObject);
    }

    private void CambiarSprite()
    {
        if (spriteFondo != null && spriteNuevo != null)
        {
            spriteFondo.sprite = spriteNuevo;
        }
    }

    private IEnumerator FadePanel(float inicio, float final)
    {
        if (panelFade == null)
            yield break;

        float tiempo = 0f;

        panelFade.alpha = inicio;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;

            float porcentaje = tiempo / duracionFade;

            panelFade.alpha = Mathf.Lerp(inicio, final, porcentaje);

            yield return null;
        }

        panelFade.alpha = final;
    }
}

