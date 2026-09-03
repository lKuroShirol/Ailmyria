using System.Collections;
using TMPro;
using UnityEngine;

public class LetrasLocas : MonoBehaviour
{
    [Header("Configuración del Lugar")]
    [Tooltip("El nombre del lugar que aparecerá en pantalla.")]
    public string nombreDelLugar = "Nombre de la Zona";

    [Header("Tiempos de Espera")]
    [Tooltip("Segundos que el texto se mantiene INVISIBLE antes de empezar a aparecer.")]
    public float tiempoDeEsperaInicial = 1f;
    [Tooltip("Segundos que el texto se queda completamente visible antes de empezar a desaparecer.")]
    public float tiempoEnPantalla = 2.5f;

    [Header("Tiempos y Efectos")]
    [Tooltip("Tiempo que tarda en aparecer cada letra (efecto máquina de escribir).")]
    public float velocidadEscritura = 0.08f;
    [Tooltip("Qué tan alto saltan las letras al aparecer.")]
    public float alturaRebote = 30f;

    [Header("Velocidades de Transición (Independientes)")]
    [Tooltip("Velocidad con la que las letras aparecen (Fade In) al inicio.")]
    public float velocidadAparicion = 3f;
    [Tooltip("Velocidad con la que el texto se desvanece (Fade Out) al final.")]
    public float velocidadDesvanecimiento = 2f;

    private TextMeshProUGUI textoMesh;

    void Awake()
    {
        textoMesh = GetComponent<TextMeshProUGUI>();

        // Nos aseguramos de que el texto empiece vacío e invisible al cargar la escena
        if (textoMesh != null)
        {
            textoMesh.text = "";
        }
    }

    void Start()
    {
        if (textoMesh != null)
        {
            StartCoroutine(SecuenciaAparecerLugar());
        }
    }

    private IEnumerator SecuenciaAparecerLugar()
    {
        // 1. ESPERA INICIAL (El texto permanece totalmente invisible/vacío durante este tiempo)
        yield return new WaitForSeconds(tiempoDeEsperaInicial);

        // 2. Asignamos el texto real y lo preparamos oculto
        textoMesh.text = nombreDelLugar;
        textoMesh.ForceMeshUpdate();

        TMP_TextInfo textInfo = textoMesh.textInfo;

        // Ocultamos todas las letras al inicio (Alfa a 0)
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
            Color32[] colors = textInfo.meshInfo[materialIndex].colors32;
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;

            for (int j = 0; j < 4; j++)
            {
                colors[vertexIndex + j].a = 0;
            }
        }
        textoMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        textoMesh.alpha = 1f;

        // 3. APARICIÓN LETRA POR LETRA CON REBOTE Y FADE-IN SUAVE
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            StartCoroutine(AnimarAparicionLetra(i, alturaRebote));

            yield return new WaitForSeconds(velocidadEscritura);
        }

        // 4. ESPERAR EN PANTALLA
        yield return new WaitForSeconds(tiempoEnPantalla);

        // 5. DESVANECIMIENTO LENTO (FADE OUT)
        float alfaActual = 1f;
        while (alfaActual > 0f)
        {
            alfaActual -= Time.deltaTime * velocidadDesvanecimiento;
            textoMesh.alpha = Mathf.Clamp01(alfaActual);
            yield return null;
        }

        gameObject.SetActive(false);
    }

    private IEnumerator AnimarAparicionLetra(int charIndex, float alturaInicial)
    {
        TMP_TextInfo textInfo = textoMesh.textInfo;
        int materialIndex = textInfo.characterInfo[charIndex].materialReferenceIndex;
        Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
        Color32[] colors = textInfo.meshInfo[materialIndex].colors32;
        int vertexIndex = textInfo.characterInfo[charIndex].vertexIndex;

        float duracionAnimacion = 0.3f;
        float tiempo = 0f;

        Vector3[] posicionesOriginales = new Vector3[4];
        for (int j = 0; j < 4; j++)
        {
            posicionesOriginales[j] = vertices[vertexIndex + j];
        }

        float alphaLetra = 0f;

        while (tiempo < duracionAnimacion)
        {
            tiempo += Time.deltaTime;
            float progreso = tiempo / duracionAnimacion;

            // Rebote vertical
            float desplazamientoY = Mathf.Sin(progreso * Mathf.PI) * alturaInicial * (1f - progreso);

            // Incrementamos el alpha progresivamente usando la velocidad de aparición
            alphaLetra += Time.deltaTime * velocidadAparicion * 255f;
            byte alphaFinal = (byte)Mathf.Clamp(alphaLetra, 0, 255);

            for (int j = 0; j < 4; j++)
            {
                colors[vertexIndex + j].a = alphaFinal;
                vertices[vertexIndex + j] = posicionesOriginales[j] + new Vector3(0, desplazamientoY, 0);
            }

            textoMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices | TMP_VertexDataUpdateFlags.Colors32);
            yield return null;
        }

        // Aseguramos posición y opacidad total al terminar
        for (int j = 0; j < 4; j++)
        {
            colors[vertexIndex + j].a = 255;
            vertices[vertexIndex + j] = posicionesOriginales[j];
        }
        textoMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices | TMP_VertexDataUpdateFlags.Colors32);
    }
}
