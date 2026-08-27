using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class NPCInteraccionSecuencial : MonoBehaviour
{
    [Header("Configuración de Distancia")]
    public float distanciaInteraccion = 2f;
    public Transform brujita;

    [Header("Elementos Visuales e Indicador")]
    public GameObject indicadorObjeto;

    [Header("Paneles Globales")]
    public GameObject panelNPC;
    public TextMeshProUGUI textoNPC;
    public GameObject panelBrujita;
    public TextMeshProUGUI textoBrujita;

    [Header("Reacciones del NPC (Arrástralos una sola vez)")]
    public GameObject reaccionNPC1;
    public GameObject reaccionNPC2;
    public GameObject reaccionNPC3;
   

    [Header("Reacciones de la Brujita (Arrástralos una sola vez)")]
    public GameObject reaccionBrujita1;
    public GameObject reaccionBrujita2;
    public GameObject reaccionBrujita3;
    public GameObject reaccionBrujita4;

    public bool CondicionDialogo;
    [System.Serializable]
    public struct DialogoTurno
    {
        [Header("¿Habla el NPC? (Desmarcado = Habla la Brujita)")]
        public bool hablaElNPC;

        [Header("Mensaje")]
        [TextArea(2, 5)]
        public string mensaje;

        [Header("¿Qué reacción mostrar? (Marca solo una)")]
        public bool usarReaccion1;
        public bool usarReaccion2;
        public bool usarReaccion3;
        public bool usarReaccion4;
    }

    [Header("Sistema de Conversación")]
    public DialogoTurno[] conversacion;
    private int indiceConversacion = 0;

    [Header("Efecto de Escritura")]
    public float velocidadEscritura = 0.05f;
    private Coroutine rutinaEscritura;
    private bool textoEscribiendo = false;

    [Header("Controles")]
    public Key teclaInteraccion = Key.E;

    private bool jugadorCerca = false;
    private bool dialogoAbierto = false;

    void Start()
    {
        if (indicadorObjeto != null) indicadorObjeto.SetActive(false);
        OcultarTodo();
    }

    void Update()
    {
        if (brujita == null) return;

        float distancia = Vector2.Distance(transform.position, brujita.position);

        if (distancia <= distanciaInteraccion)
        {
            if (!jugadorCerca)
            {
                jugadorCerca = true;
                if (indicadorObjeto != null) indicadorObjeto.SetActive(true);
            }

            if (Keyboard.current != null && Keyboard.current[teclaInteraccion].wasPressedThisFrame)
            {
                ManejarDialogo();
            }
        }
        else
        {
            if (jugadorCerca)
            {
                jugadorCerca = false;
                if (indicadorObjeto != null) indicadorObjeto.SetActive(false);
                CerrarDialogo();
            }
        }
    }

    void ManejarDialogo()
    {
        if (!dialogoAbierto)
        {
            if (conversacion == null || conversacion.Length == 0) return;

            dialogoAbierto = true;
            indiceConversacion = 0;

            MostrarTurnoActual();
            PausarMovimientoJugador(true);
        }
        else
        {
            var turnoActual = conversacion[indiceConversacion];
            TextMeshProUGUI textoObjetivo = turnoActual.hablaElNPC ? textoNPC : textoBrujita;

            if (textoEscribiendo)
            {
                StopAllCoroutines();
                if (textoObjetivo != null)
                {
                    textoObjetivo.text = turnoActual.mensaje;
                }
                textoEscribiendo = false;
                return;
            }

            indiceConversacion++;

            if (indiceConversacion < conversacion.Length)
            {
                MostrarTurnoActual();
            }
            else
            {
                CerrarDialogo();
            }
        }
    }

    void MostrarTurnoActual()
    {
        OcultarTodo();

        var turno = conversacion[indiceConversacion];
        GameObject panelAActivar = null;
        TextMeshProUGUI textoAUsar = null;

       
        if (turno.hablaElNPC)
        {
            panelAActivar = panelNPC;
            textoAUsar = textoNPC;

            if (panelAActivar != null) panelAActivar.SetActive(true);

            
            if (turno.usarReaccion1 && reaccionNPC1 != null) reaccionNPC1.SetActive(true);
            if (turno.usarReaccion2 && reaccionNPC2 != null) reaccionNPC2.SetActive(true);
            if (turno.usarReaccion3 && reaccionNPC3 != null) reaccionNPC3.SetActive(true);
            
        }
        else
        {
            panelAActivar = panelBrujita;
            textoAUsar = textoBrujita;

            if (panelAActivar != null) panelAActivar.SetActive(true);

            if (turno.usarReaccion1 && reaccionBrujita1 != null) reaccionBrujita1.SetActive(true);
            if (turno.usarReaccion2 && reaccionBrujita2 != null) reaccionBrujita2.SetActive(true);
            if (turno.usarReaccion3 && reaccionBrujita3 != null) reaccionBrujita3.SetActive(true);
            if (turno.usarReaccion4 && reaccionBrujita4 != null) reaccionBrujita4.SetActive(true);

        }

       
        if (rutinaEscritura != null)
        {
            StopCoroutine(rutinaEscritura);
        }

        if (textoAUsar != null)
        {
            rutinaEscritura = StartCoroutine(EscribirTexto(turno.mensaje, textoAUsar));
        }
    }

    IEnumerator EscribirTexto(string mensajeCompleto, TextMeshProUGUI campoTexto)
    {
        campoTexto.text = "";
        textoEscribiendo = true;

        foreach (char letra in mensajeCompleto.ToCharArray())
        {
            campoTexto.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        textoEscribiendo = false;
    }

    public void CerrarDialogo()
    {
        dialogoAbierto = false;
        indiceConversacion = 0;
        CondicionDialogo = true;

        if (rutinaEscritura != null)
        {
            StopCoroutine(rutinaEscritura);
        }

        OcultarTodo();
        PausarMovimientoJugador(false);
    }

    void OcultarTodo()
    {
        if (panelNPC != null) panelNPC.SetActive(false);
        if (panelBrujita != null) panelBrujita.SetActive(false);

        // Apagamos todas las reacciones de ambos personajes por defecto
        if (reaccionNPC1 != null) reaccionNPC1.SetActive(false);
        if (reaccionNPC2 != null) reaccionNPC2.SetActive(false);
        if (reaccionNPC3 != null) reaccionNPC3.SetActive(false);
        

        if (reaccionBrujita1 != null) reaccionBrujita1.SetActive(false);
        if (reaccionBrujita2 != null) reaccionBrujita2.SetActive(false);
        if (reaccionBrujita3 != null) reaccionBrujita3.SetActive(false);
        if (reaccionBrujita4 != null) reaccionBrujita4.SetActive(false);

    }

    void PausarMovimientoJugador(bool pausar)
    {
        var scriptMovimiento = brujita.GetComponent<BrujitaMove>();
        if (scriptMovimiento != null) scriptMovimiento.enabled = !pausar;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanciaInteraccion);
    }
}
