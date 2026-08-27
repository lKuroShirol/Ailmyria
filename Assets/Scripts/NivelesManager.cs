using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NivelesManager : MonoBehaviour
{
    public string nombreDeLaEscena;

    [Header("Configuración del Fade")]
    public CanvasGroup panelFade;
    [Tooltip("Cuánto tarda en oscurecerse la pantalla en segundos")]
    public float duracionFade = 1f;
    public GameObject pantallaUI;

    void Start()
    {
        // Al iniciar la escena, nos aseguramos de que la pantalla empiece visible (transparencia en 0)
        if (panelFade != null)
        {
            panelFade.alpha = 0f;
            panelFade.gameObject.SetActive(false);
        }
    }

    // Método para botones (se llama desde el Inspector)
    public void CambiarEscena()
    {
        if (!string.IsNullOrEmpty(nombreDeLaEscena))
        {
            // Iniciamos la corrutina de fundido antes de cambiar de escena
            StartCoroutine(EfectoFadeYCargar());
        }
    }
    public void AbrirMenu()
    {
        pantallaUI.SetActive(true); ;
    }
    public void CerrarMenu()
    {
        pantallaUI.SetActive(false);
    }
    

    IEnumerator EfectoFadeYCargar()
    {
        if (panelFade != null)
        {
            panelFade.gameObject.SetActive(true);
            float tiempoTranscurrido = 0f;

         
            while (tiempoTranscurrido < duracionFade)
            {
                tiempoTranscurrido += Time.deltaTime;
                panelFade.alpha = Mathf.Clamp01(tiempoTranscurrido / duracionFade);
                yield return null; 
            }
        }

        SceneManager.LoadScene(nombreDeLaEscena);
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("FUNCIONAA");
            CambiarEscena();
        }
    }
}
