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

    [Header("Referencia Opcional al Player")]
    public BrujitaMove scriptPlayer;

    private bool menuG_Activo = false;
    private bool mostrandoB = true;
    private bool estaAnimando = false;

    private float tiempoEsperaRestante = 0f;
    private bool esperandoRetrasoExtra = false;

    private CanvasGroup cgA;
    private CanvasGroup cgB;
    private CanvasGroup cgC;

    public bool isTalk = true;

    void Start()
    {
        cgA = ObtenerOCrearCanvasGroup(imagenA);
        cgB = ObtenerOCrearCanvasGroup(imagenB);
        cgC = ObtenerOCrearCanvasGroup(imagenC);

        if (imagenA != null) imagenA.anchoredPosition = imagenA_EnEscena;
        if (imagenB != null) imagenB.anchoredPosition = imagenB_FueraSube;
        if (imagenC != null) imagenC.anchoredPosition = imagenC_FueraSube;

        ActualizarVisibilidadEfectiva();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (isTalk && !estaAnimando && !esperandoRetrasoExtra)
        {
            if (Keyboard.current[teclaG].wasPressedThisFrame)
            {
                menuG_Activo = !menuG_Activo;
                estaAnimando = true; 

                if (scriptPlayer != null) scriptPlayer.libroAbierto = menuG_Activo;
                if (menuG_Activo) mostrandoB = true;

                ActualizarVisibilidadEfectiva();
            }
        }

        if (menuG_Activo && !estaAnimando && !esperandoRetrasoExtra)
        {
            bool presionaIzquierda = Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame;
            bool presionaDerecha = Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame;

            if (presionaIzquierda || presionaDerecha)
            {
                mostrandoB = !mostrandoB;
                ActualizarVisibilidadEfectiva();
            }
        }

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

    void ActualizarVisibilidadEfectiva()
    {
        CambiarEstadoUI(cgA, true);

        if (!menuG_Activo)
        {
            ConfigurarInteraccionUI(cgB, false);
            ConfigurarInteraccionUI(cgC, false);
            cgB.alpha = 1f;
            cgC.alpha = 0f;
        }
        else
        {
            CambiarEstadoUI(cgB, mostrandoB);
            CambiarEstadoUI(cgC, !mostrandoB);
        }
    }

    void ProcesarMovimientosUI()
    {
        bool imagenATerminoDeSubir = imagenA != null && Vector2.Distance(imagenA.anchoredPosition, imagenA_FueraSube) <= margenLlegada;
        bool imagenBTerminoDeSubir = imagenB != null && Vector2.Distance(imagenB.anchoredPosition, imagenB_FueraSube) <= margenLlegada;

        bool imagenBTerminoDeBajar = imagenB != null && Vector2.Distance(imagenB.anchoredPosition, imagenB_EnEscena) <= margenLlegada;
        bool imagenATerminoDeBajar = imagenA != null && Vector2.Distance(imagenA.anchoredPosition, imagenA_EnEscena) <= margenLlegada;

        Vector2 objetivoA = menuG_Activo ? imagenA_FueraSube : imagenA_EnEscena;
        Vector2 objetivoB = menuG_Activo ? imagenB_EnEscena : imagenB_FueraSube;
        Vector2 objetivoC = menuG_Activo ? imagenC_EnEscena : imagenC_FueraSube;

        if (imagenA != null)
        {
            if (menuG_Activo || (!menuG_Activo && imagenBTerminoDeSubir))
            {
                imagenA.anchoredPosition = Vector2.Lerp(imagenA.anchoredPosition, objetivoA, velocidad * Time.deltaTime);
            }
        }
        if (imagenB != null)
        {
            if (!menuG_Activo || (menuG_Activo && imagenATerminoDeSubir))
            {
                imagenB.anchoredPosition = Vector2.Lerp(imagenB.anchoredPosition, objetivoB, velocidad * Time.deltaTime);
            }
        }
        if (imagenC != null)
        {
            if (!menuG_Activo || (menuG_Activo && imagenATerminoDeSubir))
            {
                imagenC.anchoredPosition = Vector2.Lerp(imagenC.anchoredPosition, objetivoC, velocidad * Time.deltaTime);
            }
        }
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

        if (!menuG_Activo && imagenBTerminoDeSubir)
        {
            cgB.alpha = 0f;
        }
    }

    private CanvasGroup ObtenerOCrearCanvasGroup(RectTransform rect)
    {
        if (rect == null) return null;
        CanvasGroup cg = rect.GetComponent<CanvasGroup>();
        if (cg == null) cg = rect.gameObject.AddComponent<CanvasGroup>();
        return cg;
    }

    private void CambiarEstadoUI(CanvasGroup cg, bool activado)
    {
        if (cg == null) return;
        cg.alpha = activado ? 1f : 0f;
        ConfigurarInteraccionUI(cg, activado);
    }

    private void ConfigurarInteraccionUI(CanvasGroup cg, bool activado)
    {
        if (cg == null) return;
        cg.interactable = activado;
        cg.blocksRaycasts = activado;
    }
}
