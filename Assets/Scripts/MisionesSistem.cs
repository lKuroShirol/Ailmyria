using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MisionesSistem : MonoBehaviour
{
    [Header("Condiciones para la desactivación")]
    public string banderaRequerida = "Mision Cumplida :D";
    [Header("Objeto 1")]
    [Tooltip("Lista de objetos asignados al primer contenedor.")]
    public GameObject objeto1;

    [Header("Objeto 2")]
    public GameObject contenedor2;

    [Header("NPC NuevoDialogo")]
    public GameObject DialogoNPC1;
    [Header("NPC ViejoDialogo")]
    public GameObject OldDialogoNPC;
    [Header("NPC ViejoDialogo 2")]
    public GameObject OldDialogoNPC2;

    [Header("NPC 2 NuevoDialogo")]
    public GameObject contenedor4;

    [Header("Panel de Aviso")]
    public GameObject UIAviso;
    public float tiempoEnPantalla = 2f;
    private bool conversacionActivada = false;

    private void Update()
    {
        if (!conversacionActivada && objeto1 != null && !objeto1.activeSelf)
        {
            conversacionActivada = true;
            ActivarNuevaConversación();
            Debug.Log("Condición cumplida. Ahora puedes hablar con el NPC...");
            UIAviso.SetActive(true);
            StartCoroutine(DesactivarAvisoDespuesDeTiempo());
        }
    }
    private void ActivarNuevaConversación()
    {
        OldDialogoNPC.SetActive(false);
        if (OldDialogoNPC2 != null) OldDialogoNPC2.SetActive(false);
        else print("No hay 2do dialogo");

         DialogoNPC1.SetActive(true);

       
    }


    public void CerrarPanel()
    {
        if (Keyboard.current != null &&
           (Keyboard.current.eKey.wasPressedThisFrame ||
            Keyboard.current.enterKey.wasPressedThisFrame ||
            Keyboard.current.escapeKey.wasPressedThisFrame))
        {
            UIAviso.SetActive(false);
        }
    }

    private IEnumerator DesactivarAvisoDespuesDeTiempo()
    {
       
        yield return new WaitForSeconds(tiempoEnPantalla);
        UIAviso.SetActive(false);
    }

}

