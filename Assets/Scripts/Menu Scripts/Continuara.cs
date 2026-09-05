using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Continuara : MonoBehaviour
{
    [Header("Escena del menú")]
    [SerializeField] private string nombreEscenaMenu = "INTRO";

    [Header("Configuración del Fade")]
    [SerializeField] private float duracionFade = 1f;

    [Header("Nombre del objeto Fade")]
    [SerializeField] private string nombreObjetoFade = "PanelFade";

    private CanvasGroup panelFade;
    private bool cambiandoEscena = false;

    private void Awake()
    {
        // Busca automáticamente el objeto PanelFade en la escena.
        GameObject objetoFade = GameObject.Find(nombreObjetoFade);

        if (objetoFade != null)
        {
            panelFade = objetoFade.GetComponent<CanvasGroup>();

            if (panelFade != null)
            {
                // La pantalla comienza transparente.
                panelFade.alpha = 0f;
            }
            else
            {
                Debug.LogWarning(
                    "El objeto '" + nombreObjetoFade +
                    "' fue encontrado, pero no tiene CanvasGroup."
                );
            }
        }
        else
        {
            Debug.LogWarning(
                "No se encontró el objeto '" +
                nombreObjetoFade +
                "' en la escena."
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (cambiandoEscena)
            return;

        if (collision.CompareTag("Player"))
        {
            cambiandoEscena = true;

            StartCoroutine(VolverMenu());
        }
    }

    private IEnumerator VolverMenu()
    {
        // Si encontró el PanelFade, hacemos el fundido.
        if (panelFade != null)
        {
            panelFade.gameObject.SetActive(true);

            float tiempo = 0f;

            while (tiempo < duracionFade)
            {
                tiempo += Time.deltaTime;

                panelFade.alpha = Mathf.Clamp01(
                    tiempo / duracionFade
                );

                yield return null;
            }
        }

        // Cuando termina el fade, vamos al menú.
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}
