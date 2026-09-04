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

    [Header("Nuevo Panel Global (Narración / Objetos)")]
    public GameObject panelNarracion;
    public TextMeshProUGUI textoNarracion;

    [Header("Reacciones del NPC (Arrástralos una sola vez)")]
    public GameObject reaccionNPC1;
    public GameObject reaccionNPC2;
    public GameObject reaccionNPC3;


    [Header("Reacciones de la Brujita (Arrástralos una sola vez)")]
    public GameObject reaccionBrujita1;
    public GameObject reaccionBrujita2;
    public GameObject reaccionBrujita3;
    public GameObject reaccionBrujita4;


    public bool AhoraNuevaCharla;

    GrimorioUI grimorioUI;

    [Header("No Mover a menos de que termine la charla")]
    public GameObject NewDialogoNPC1;
    [Header("NPC ViejoDialogo")]
    public GameObject ActualDialogoNPC;

    [System.Serializable]
    public struct DialogoTurno
    {
        [Header("¿Habla el NPC? (Desmarcado = Habla la Brujita)")]
        public bool hablaElNPC;

        [Header("¿Usar Panel Global / Narración (Objeto)?")]
        public bool esPanelGeneral;

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
    public bool CondicionDialogo = false;
    private bool jugadorCerca = false;
    private bool dialogoAbierto = false;
    public bool TerminoLaInteraccion;

    [Header("Follow del NPC")]
    public FollowNPC followNPC;
    public bool QuieresQueSiga;
    public string NPCtag;

    [Header("Sistema de Drop de Objeto al Finalizar")]
    [Tooltip("Activa esta casilla si este NPC debe soltar un objeto al terminar toda la conversación.")]
    public bool soltarObjetoAlTerminar = false;
    [Tooltip("Prefab del objeto que se va a dropear (aparecer) en el mundo.")]
    public GameObject prefabObjetoDrop;
    [Tooltip("Punto exacto donde aparecerá el objeto (si lodejas vacío, aparecerá en la posición del NPC).")]
    public Transform puntoDeSpawnDrop;
    private bool objetoYaSoltado = false;

    void Awake()
    {
        grimorioUI = FindAnyObjectByType<GrimorioUI>();
        GameObject objetoEncontrado = GameObject.Find("BrujitaO");
        if (QuieresQueSiga) followNPC = GameObject.FindGameObjectWithTag(NPCtag).GetComponent<FollowNPC>();

        if (followNPC == null) print("No se encontró");
        if (objetoEncontrado != null)
        {
            brujita = objetoEncontrado.transform;
            Debug.Log("¡Transform asignado con éxito a: " + objetoEncontrado.name + "!");
        }
    }

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
                grimorioUI.isTalk = false;
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

        if (CondicionDialogo && TerminoLaInteraccion)
        {
            gameObject.SetActive(false);
            grimorioUI.isTalk = true;
        }

        if (TerminoLaInteraccion && QuieresQueSiga)
        {
            followNPC.estaSiguiendo = true;
        }
    }

    public void ManejarDialogo()
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
            TextMeshProUGUI textoObjetivo = turnoActual.esPanelGeneral ? textoNarracion : (turnoActual.hablaElNPC ? textoNPC : textoBrujita);

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
                if (AhoraNuevaCharla) ActivarNuevaConversación();
            }
        }
    }

    void MostrarTurnoActual()
    {
        OcultarTodo();

        var turno = conversacion[indiceConversacion];
        GameObject panelAActivar = null;
        TextMeshProUGUI textoAUsar = null;

        if (turno.esPanelGeneral)
        {
            panelAActivar = panelNarracion;
            textoAUsar = textoNarracion;

            if (panelAActivar != null) panelAActivar.SetActive(true);
        }
        else if (turno.hablaElNPC)
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

    private void ActivarNuevaConversación()
    {
        ActualDialogoNPC.SetActive(false);
        NewDialogoNPC1.SetActive(true);
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
        TerminoLaInteraccion = true;
        indiceConversacion = 0;
        grimorioUI.isTalk = true;

        // EJECUTAR EL DROP DEL OBJETO SI ESTÁ ACTIVADO Y NO SE HA SOLTADO AÚN
        if (soltarObjetoAlTerminar && !objetoYaSoltado)
        {
            SoltarObjeto();
        }

        if (rutinaEscritura != null)
        {
            StopCoroutine(rutinaEscritura);
        }

        OcultarTodo();
        PausarMovimientoJugador(false);
    }

    void SoltarObjeto()
    {
        if (prefabObjetoDrop != null)
        {
            // Determinamos la posición del drop (si hay un punto asignado usa ese, sino usa la posición del NPC)
            Vector3 posicionSpawn = (puntoDeSpawnDrop != null) ? puntoDeSpawnDrop.position : transform.position;

            Instantiate(prefabObjetoDrop, posicionSpawn, Quaternion.identity);
            objetoYaSoltado = true;
            Debug.Log("--- El NPC ha dropeado el objeto correctamente ---");
        }
        else
        {
            Debug.LogWarning("Se activó el drop en el NPC, pero falta asignar el Prefab del objeto.", this);
        }
    }

    void OcultarTodo()
    {
        if (panelNPC != null) panelNPC.SetActive(false);
        if (panelBrujita != null) panelBrujita.SetActive(false);
        if (panelNarracion != null) panelNarracion.SetActive(false);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CondicionDialogo && collision.CompareTag("Player"))
        {
            ManejarDialogo();
        }
    }
}
