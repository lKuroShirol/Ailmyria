using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrimorioUI : MonoBehaviour
 
{
    [Header("Configuración de Tecla")]
    public Key teclaSubirBajar = Key.E; 

    [Header("Posiciones (Mundo o UI)")]
    public Vector3 posicionAbajo;     
    public Vector3 posicionArriba;     
    public float velocidad = 5f;

    [Header("Objetos a Intercalar")]
    public GameObject objeto2A;        
    public GameObject objeto2B;       

    [Header("Referencia al Player")]
    public BrujitaMove scriptPlayer;   

    private bool estaArriba = false;
    private bool mostrandoOpcionA = true; 

    void Start()
    {
        transform.position = posicionAbajo;
       
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current[teclaSubirBajar].wasPressedThisFrame)
        {
            estaArriba = !estaArriba;

            if (scriptPlayer != null)
            {
                scriptPlayer.libroAbierto = estaArriba; 
            }

            if (!estaArriba)
            {
                ApagarObjeto2Completo();
            }
            else
            {
                ActualizarObjetos();
            }
        }

  
        Vector3 objetivo = estaArriba ? posicionArriba : posicionAbajo;
        transform.position = Vector3.Lerp(transform.position, objetivo, velocidad * Time.deltaTime);

        if (estaArriba)
        {
            bool teclaDerecha = Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame;
            bool teclaIzquierda = Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame;

            if (teclaDerecha || teclaIzquierda)
            {
                mostrandoOpcionA = !mostrandoOpcionA;
                ActualizarObjetos();
            }
        }
    }

    void ActualizarObjetos()
    {
        if (objeto2A != null) objeto2A.SetActive(mostrandoOpcionA);
        if (objeto2B != null) objeto2B.SetActive(!mostrandoOpcionA);
    }

    void ApagarObjeto2Completo()
    {
        if (objeto2A != null) objeto2A.SetActive(false);
        if (objeto2B != null) objeto2B.SetActive(false);
    }
}
