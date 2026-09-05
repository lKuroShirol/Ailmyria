using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Vendaval : MonoBehaviour
{
    public bool isCasting = false;

    [SerializeField] private GameObject VientoObject;
    [SerializeField] private Animator VientoAnimator;
    [SerializeField] private Collider2D VientoCollider;

    [Header("Lanzamiento")]
    [SerializeField] private float launchDelay = 1.5f;
    [SerializeField] private float velocidadViento = 5f;

    private bool isLaunching = false;

    private Hechizos_Manager hechizosManager;
    private BrujitaMove brujitaMove;

    public Key TeclaViento = Key.V;

    private Vector3 posicionInicial;

    private Animator brujitaAnimator;
    private SpriteRenderer VientoSprite;

    private void Awake()
    {
        posicionInicial = VientoObject.transform.localPosition;

        VientoSprite = VientoObject.GetComponent<SpriteRenderer>();

        VientoObject.SetActive(false);

        if (VientoCollider != null)
        {
            VientoCollider.enabled = false;
        }

        brujitaMove = GetComponent<BrujitaMove>();
        hechizosManager = GetComponent<Hechizos_Manager>();
        brujitaAnimator = GetComponent<Animator>();

        isCasting = false;
        isLaunching = false;
    }

    private void Update()
    {
        if (hechizosManager == null)
            return;

        // Debe tener el hechizo Vendaval desbloqueado
        if (!hechizosManager.vendavalDesbloqueada)
            return;

        // Debe tener la varita desbloqueada
        if (!hechizosManager.varitaDesbloqueada)
            return;

        if (Keyboard.current != null &&
            Keyboard.current[TeclaViento].wasPressedThisFrame)
        {
            if (!isCasting)
            {
                isCasting = true;

                VientoObject.SetActive(true);
                VientoObject.transform.localPosition = posicionInicial;

                Debug.Log("Vendaval preparado");
            }
            else if (!isLaunching)
            {
                StartCoroutine(LanzarViento());
            }
        }
    }

    private IEnumerator LanzarViento()
    {
        isLaunching = true;
        isCasting = true;

        VientoObject.transform.localPosition = posicionInicial;

        Vector2 direccion = brujitaMove.DireccionHorizontal;

        if (VientoSprite != null)
        {
            if (direccion == Vector2.right)
            {
                VientoSprite.flipX = false;
            }
            else if (direccion == Vector2.left)
            {
                VientoSprite.flipX = true;
            }
        }

        // Bloqueamos el movimiento mientras realiza el lanzamiento
        if (brujitaMove != null)
        {
            brujitaMove.enabled = false;
        }

        // Como ya comprobamos que tiene la varita,
        // podemos utilizar el Animator con varita.
        if (direccion.x > 0)
        {
            brujitaAnimator.SetFloat("LanzamientoDireccion", 1f);
        }
        else
        {
            brujitaAnimator.SetFloat("LanzamientoDireccion", -1f);
        }

        brujitaAnimator.SetBool("IsCasting", true);

        yield return new WaitForSeconds(0.3f);

        if (VientoCollider != null)
        {
            VientoCollider.enabled = true;
        }

        if (VientoAnimator != null)
        {
            VientoAnimator.SetTrigger("Lanzar");
        }

        Debug.Log("Vendaval lanzado hacia: " + direccion);

        float tiempo = 0f;

        while (tiempo < launchDelay)
        {
            VientoObject.transform.localPosition +=
                (Vector3)direccion * velocidadViento * Time.deltaTime;

            tiempo += Time.deltaTime;

            yield return null;
        }

        if (VientoCollider != null)
        {
            VientoCollider.enabled = false;
        }

        VientoObject.SetActive(false);
        VientoObject.transform.localPosition = posicionInicial;

        isCasting = false;
        isLaunching = false;

        brujitaAnimator.SetBool("IsCasting", false);

        if (brujitaMove != null)
        {
            brujitaMove.enabled = true;
        }
    }
}