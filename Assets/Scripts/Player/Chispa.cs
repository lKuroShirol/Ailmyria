using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Chispa : MonoBehaviour
{
    public bool isCasting = false;

    [SerializeField] private GameObject chispaObject;

    private Hechizos_Manager hechizosManager;

    public Key TeclaChispa = Key.C;

    private Vector3 posicionInicial;

    [SerializeField] private float tiempoEncendido = 10f;

    private Coroutine coroutineChispa;

    [Header("Animaciones")]
    [SerializeField] private RuntimeAnimatorController animatorNormal;
    [SerializeField] private RuntimeAnimatorController animatorMejorado;

    private Animator animator;
    private bool animatorMejoradoAplicado = false;

    private void Awake()
    {
        posicionInicial = chispaObject.transform.localPosition;

        chispaObject.SetActive(false);

        hechizosManager = GetComponent<Hechizos_Manager>();

        animator = chispaObject.GetComponent<Animator>();

        isCasting = false;
    }
    private void OnEnable()
    {
        SceneManager.sceneUnloaded += AlSalirDeEscena;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= AlSalirDeEscena;
    }

    private void AlSalirDeEscena(Scene escena)
    {
        ApagarChispa();
    }

    private void Update()
    {
        if (!hechizosManager.PuedeUsarHechizos())
            return;

        if (!hechizosManager.chispaDesbloqueada)
            return;

        // Cambiar al Animator mejorado cuando se desbloquee el anillo
        if (hechizosManager.anilloDesbloqueado && !animatorMejoradoAplicado)
        {
            animator.runtimeAnimatorController = animatorMejorado;
            animatorMejoradoAplicado = true;
        }

        if (Keyboard.current[TeclaChispa].wasPressedThisFrame)
        {
            // Si la chispa está apagada
            if (!isCasting)
            {
                coroutineChispa = StartCoroutine(EncenderLuz());
            }
            // Si la chispa está encendida y tiene el anillo
            else if (hechizosManager.anilloDesbloqueado)
            {
                ApagarChispa();
            }
        }
    }

    private IEnumerator EncenderLuz()
    {
        isCasting = true;

        chispaObject.SetActive(true);
        chispaObject.transform.localPosition = posicionInicial;

        Debug.Log("Chispa encendida");

        // Con el anillo no hay límite de tiempo
        if (hechizosManager.anilloDesbloqueado)
        {
            coroutineChispa = null;
            yield break;
        }

        // Sin el anillo, permanece encendida durante este tiempo
        yield return new WaitForSeconds(tiempoEncendido);

        isCasting = false;
        chispaObject.SetActive(false);

        coroutineChispa = null;

        Debug.Log("Chispa apagada por tiempo");
    }

    private void ApagarChispa()
    {
        if (coroutineChispa != null)
        {
            StopCoroutine(coroutineChispa);
            coroutineChispa = null;
        }

        isCasting = false;
        chispaObject.SetActive(false);

        Debug.Log("Chispa apagada");
    }
}
