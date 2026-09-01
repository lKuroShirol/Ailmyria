using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject panelPausa;
    [SerializeField] GameObject panelControls;

    [Header("Configuración del Fade")]
    public CanvasGroup panelFade;
    [Tooltip("Cuánto tarda en oscurecerse la pantalla en segundos")]
    public float duracionFade = 1f;


    private void Awake()
    {
        Time.timeScale = 1f;
        if (panelPausa != null)
            panelPausa.SetActive(false);

        if (panelControls != null)
            panelControls.SetActive(false);

        if (panelFade != null)
        {
            panelFade.alpha = 0f;
            panelFade.gameObject.SetActive(false);
        }
    }


    // Update is called once per frame
    public void StartGame()
    {
        StartCoroutine(EfectoFadeYCargar("Bosque"));
    }

    public void OnPause()
    {
        if (panelPausa == null)
            return;

        Time.timeScale = 0f;
        panelPausa.SetActive(true);
    }
    
    public void OnResume()
    {
        Time.timeScale = 1f;

        if (panelPausa != null)
            panelPausa.SetActive(false);
    }

    public void Menu()
    {  
        Time.timeScale = 1f;
        StartCoroutine(EfectoFadeYCargar("INTRO"));
    }

    public void Creditos()
    {
        Time.timeScale = 1f;
        StartCoroutine(EfectoFadeYCargar("Creditos"));

    }

    public void Controls()
    {
        if (panelControls == null)
            return;

        panelControls.SetActive(true);
    }

    public void Back()
    {
        if (panelControls == null)
            return;

        panelControls.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator EfectoFadeYCargar(string nombreEscena)
    {
        Time.timeScale = 1f;

        if (panelFade != null)
        {
            panelFade.gameObject.SetActive(true);

            float tiempoTranscurrido = 0f;

            while (tiempoTranscurrido < duracionFade)
            {
                tiempoTranscurrido += Time.unscaledDeltaTime;

                panelFade.alpha = Mathf.Clamp01(
                    tiempoTranscurrido / duracionFade
                );

                yield return null;
            }

            panelFade.alpha = 1f;
        }

        SceneManager.LoadScene(nombreEscena);
    }
}
