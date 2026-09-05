using UnityEngine;
using UnityEngine.InputSystem;


public class GrimorioUI : MonoBehaviour
{
    [Header("Configuración de Teclas")]
    public Key teclaG = Key.G;

    [Header("Referencias UI (Deben tener CanvasGroup)")]
    public RectTransform imagenA;
    public RectTransform imagenB;
    public RectTransform imagenC;

    [Header("Imagen que reemplaza a Imagen C")]
    public RectTransform switchImagen;

    [Header("Referencia al Player")]
    public BrujitaMove scriptPlayer;

    [Header("Referencia al sistema de hechizos")]
    public Hechizos_Manager hechizosManager;

    [Header("Posiciones para Imagen A")]
    public Vector2 imagenA_EnEscena;
    public Vector2 imagenA_FueraSube;

    [Header("Posiciones para Imagen B")]
    public Vector2 imagenB_EnEscena;
    public Vector2 imagenB_FueraSube;

    [Header("Posiciones para Imagen C")]
    public Vector2 imagenC_EnEscena;
    public Vector2 imagenC_FueraSube;

    [Header("Configuración de Animación")]
    public float velocidad = 8f;

    [Tooltip("Margen de píxeles para considerar que un objeto ya llegó a su destino")]
    public float margenLlegada = 2f;

    [Tooltip("Tiempo de espera extra en segundos después de que el objeto llegó a su destino")]
    public float retrasoPostLlegada = 0.5f;

    private bool menuG_Activo = false;
    private bool mostrandoB = true;
    private bool estaAnimando = false;

    private float tiempoEsperaRestante = 0f;
    private bool esperandoRetrasoExtra = false;

    private CanvasGroup cgA;
    private CanvasGroup cgB;
    private CanvasGroup cgC;
    private CanvasGroup cgSwitchImagen;

    public bool isTalk = true;

    private bool imagenChispaActivada = false;

    void Start()
    {
        cgA = ObtenerOCrearCanvasGroup(imagenA);
        cgB = ObtenerOCrearCanvasGroup(imagenB);
        cgC = ObtenerOCrearCanvasGroup(imagenC);
        cgSwitchImagen = ObtenerOCrearCanvasGroup(switchImagen);

        // Posiciones iniciales
        if (imagenA != null)
            imagenA.anchoredPosition = imagenA_EnEscena;

        if (imagenB != null)
            imagenB.anchoredPosition = imagenB_FueraSube;

        if (imagenC != null)
            imagenC.anchoredPosition = imagenC_FueraSube;

        // La imagen de Chispa empieza desactivada
        if (switchImagen != null)
        {
            switchImagen.gameObject.SetActive(false);
        }

        ActualizarVisibilidadEfectiva();
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        // Comprobar si se desbloqueó Chispa
        ComprobarChispa();

        // Abrir / cerrar grimorio
        if (isTalk && !estaAnimando && !esperandoRetrasoExtra)
        {
            if (Keyboard.current[teclaG].wasPressedThisFrame)
            {
                menuG_Activo = !menuG_Activo;
                estaAnimando = true;

                if (scriptPlayer != null)
                    scriptPlayer.libroAbierto = menuG_Activo;

                if (menuG_Activo)
                    mostrandoB = true;

                ActualizarVisibilidadEfectiva();
            }
        }

        // Cambiar de hoja
        if (menuG_Activo && !estaAnimando && !esperandoRetrasoExtra)
        {
            bool presionaIzquierda =
                Keyboard.current.aKey.wasPressedThisFrame ||
                Keyboard.current.leftArrowKey.wasPressedThisFrame;

            bool presionaDerecha =
                Keyboard.current.dKey.wasPressedThisFrame ||
                Keyboard.current.rightArrowKey.wasPressedThisFrame;

            if (presionaIzquierda || presionaDerecha)
            {
                mostrandoB = !mostrandoB;
                ActualizarVisibilidadEfectiva();
            }
        }

        // Retraso después de la animación
        if (esperandoRetrasoExtra)
        {
            tiempoEsperaRestante -= Time.deltaTime;

            if (tiempoEsperaRestante <= 0f)
            {
                esperandoRetrasoExtra = false;
            }
        }

        ProcesarMovimientosUI();
    }

    private void ComprobarChispa()
    {
        if (imagenChispaActivada)
            return;

        if (hechizosManager == null)
            return;

        if (hechizosManager.chispaDesbloqueada)
        {
            ActivarImagenChispa();
        }
    }

    private void ActivarImagenChispa()
    {
        imagenChispaActivada = true;

        // Ocultamos Imagen C
        if (imagenC != null)
        {
            imagenC.gameObject.SetActive(false);
        }

        // Mostramos la nueva imagen
        if (switchImagen != null)
        {
            switchImagen.gameObject.SetActive(true);
            switchImagen.anchoredPosition = imagenC_FueraSube;

            cgSwitchImagen.alpha = 0f;
            cgSwitchImagen.interactable = false;
            cgSwitchImagen.blocksRaycasts = false;
        }
    }

    void ActualizarVisibilidadEfectiva()
    {
        CambiarEstadoUI(cgA, true);

        if (!menuG_Activo)
        {
            ConfigurarInteraccionUI(cgB, false);
            ConfigurarInteraccionUI(cgC, false);

            cgB.alpha = 1f;
            cgC.alpha = 0f;

            if (switchImagen != null && imagenChispaActivada)
            {
                cgSwitchImagen.alpha = 0f;
                ConfigurarInteraccionUI(cgSwitchImagen, false);
            }
        }
        else
        {
            CambiarEstadoUI(cgB, mostrandoB);

            if (imagenChispaActivada)
            {
                // Si Chispa está desbloqueada,
                // SwitchImagen ocupa el lugar de Imagen C.
                CambiarEstadoUI(cgSwitchImagen, !mostrandoB);

                ConfigurarInteraccionUI(cgC, false);
                cgC.alpha = 0f;
            }
            else
            {
                CambiarEstadoUI(cgC, !mostrandoB);
            }
        }
    }

    void ProcesarMovimientosUI()
    {
        bool imagenATerminoDeSubir =
            imagenA != null &&
            Vector2.Distance(
                imagenA.anchoredPosition,
                imagenA_FueraSube
            ) <= margenLlegada;

        bool imagenBTerminoDeSubir =
            imagenB != null &&
            Vector2.Distance(
                imagenB.anchoredPosition,
                imagenB_FueraSube
            ) <= margenLlegada;

        bool imagenBTerminoDeBajar =
            imagenB != null &&
            Vector2.Distance(
                imagenB.anchoredPosition,
                imagenB_EnEscena
            ) <= margenLlegada;

        bool imagenATerminoDeBajar =
            imagenA != null &&
            Vector2.Distance(
                imagenA.anchoredPosition,
                imagenA_EnEscena
            ) <= margenLlegada;

        Vector2 objetivoA =
            menuG_Activo
            ? imagenA_FueraSube
            : imagenA_EnEscena;

        Vector2 objetivoB =
            menuG_Activo
            ? imagenB_EnEscena
            : imagenB_FueraSube;

        Vector2 objetivoC =
            menuG_Activo
            ? imagenC_EnEscena
            : imagenC_FueraSube;

        // Imagen A
        if (imagenA != null)
        {
            if (menuG_Activo ||
                (!menuG_Activo && imagenBTerminoDeSubir))
            {
                imagenA.anchoredPosition = Vector2.Lerp(
                    imagenA.anchoredPosition,
                    objetivoA,
                    velocidad * Time.deltaTime
                );
            }
        }

        // Imagen B
        if (imagenB != null)
        {
            if (!menuG_Activo ||
                (menuG_Activo && imagenATerminoDeSubir))
            {
                imagenB.anchoredPosition = Vector2.Lerp(
                    imagenB.anchoredPosition,
                    objetivoB,
                    velocidad * Time.deltaTime
                );
            }
        }

        // Imagen C
        if (imagenC != null && !imagenChispaActivada)
        {
            if (!menuG_Activo ||
                (menuG_Activo && imagenATerminoDeSubir))
            {
                imagenC.anchoredPosition = Vector2.Lerp(
                    imagenC.anchoredPosition,
                    objetivoC,
                    velocidad * Time.deltaTime
                );
            }
        }

        // SwitchImagen utiliza las mismas posiciones que Imagen C
        if (switchImagen != null && imagenChispaActivada)
        {
            Vector2 objetivoSwitch =
                menuG_Activo
                ? imagenC_EnEscena
                : imagenC_FueraSube;

            if (menuG_Activo &&
                imagenATerminoDeSubir)
            {
                switchImagen.anchoredPosition = Vector2.Lerp(
                    switchImagen.anchoredPosition,
                    objetivoSwitch,
                    velocidad * Time.deltaTime
                );
            }
            else if (!menuG_Activo)
            {
                switchImagen.anchoredPosition = Vector2.Lerp(
                    switchImagen.anchoredPosition,
                    objetivoSwitch,
                    velocidad * Time.deltaTime
                );
            }
        }

        // Finalizar animación
        if (estaAnimando)
        {
            if (menuG_Activo && imagenBTerminoDeBajar)
            {
                estaAnimando = false;
                esperandoRetrasoExtra = true;
                tiempoEsperaRestante = retrasoPostLlegada;
            }
            else if (!menuG_Activo && imagenATerminoDeBajar)
            {
                estaAnimando = false;
                esperandoRetrasoExtra = true;
                tiempoEsperaRestante = retrasoPostLlegada;
            }
        }

        // Ocultar B cuando el grimorio está cerrado
        if (!menuG_Activo && imagenBTerminoDeSubir)
        {
            cgB.alpha = 0f;
        }

        // Ocultar SwitchImagen cuando el grimorio está cerrado
        if (!menuG_Activo && imagenChispaActivada && switchImagen != null)
        {
            if (Vector2.Distance(
                switchImagen.anchoredPosition,
                imagenC_FueraSube
            ) <= margenLlegada)
            {
                cgSwitchImagen.alpha = 0f;
                ConfigurarInteraccionUI(cgSwitchImagen, false);
            }
        }
    }

    private CanvasGroup ObtenerOCrearCanvasGroup(RectTransform rect)
    {
        if (rect == null)
            return null;

        CanvasGroup cg = rect.GetComponent<CanvasGroup>();

        if (cg == null)
            cg = rect.gameObject.AddComponent<CanvasGroup>();

        return cg;
    }

    private void CambiarEstadoUI(CanvasGroup cg, bool activado)
    {
        if (cg == null)
            return;

        cg.alpha = activado ? 1f : 0f;
        ConfigurarInteraccionUI(cg, activado);
    }

    private void ConfigurarInteraccionUI(
        CanvasGroup cg,
        bool activado
    )
    {
        if (cg == null)
            return;

        cg.interactable = activado;
        cg.blocksRaycasts = activado;
    }
}
