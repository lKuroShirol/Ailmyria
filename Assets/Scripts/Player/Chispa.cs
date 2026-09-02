using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chispa : MonoBehaviour
{
    public bool isCasting = false;

    [SerializeField] private GameObject chispaObject;
    [SerializeField] private Animator chispaAnimator;
    [SerializeField] private Collider2D chispaCollider;

    [Header("Lanzamiento")]
    [SerializeField] private float launchDelay = 1.5f;
    [SerializeField] private float velocidadChispa = 5f;

    private bool isLaunching = false;

    private Hechizos_Manager hechizosManager;
    private BrujitaMove brujitaMove;
    public Key TeclaChispa = Key.C;

    private Vector3 posicionInicial;
    private Animator brujitaAnimator;
    private SpriteRenderer chispaSprite;

    private void Awake()
    {
        posicionInicial = chispaObject.transform.localPosition;

        chispaSprite = chispaObject.GetComponent<SpriteRenderer>();

        chispaObject.SetActive(false);

        if (chispaCollider != null)
        {
            chispaCollider.enabled = false;
        }

        brujitaMove = GetComponent<BrujitaMove>();
        hechizosManager = GetComponent<Hechizos_Manager>();
        brujitaAnimator = GetComponent<Animator>();

        isCasting = false;
        isLaunching = false;
    }

    private void Update()
    {
        if (!hechizosManager.chispaDesbloqueada)
            return;

        if (Keyboard.current[TeclaChispa].wasPressedThisFrame)
        {
            if (!isCasting)
            {
                isCasting = true;
                chispaObject.SetActive(true);

                chispaObject.transform.localPosition = posicionInicial;

                Debug.Log("Chispa encendida");
            }
            else if (!isLaunching)
            {
                StartCoroutine(LanzarChispa());
            }
        }
    }

    private IEnumerator LanzarChispa()
    {
        isLaunching = true;
        isCasting = true;

        chispaObject.transform.localPosition = posicionInicial;

        Vector2 direccion = brujitaMove.DireccionHorizontal;

        if (direccion == Vector2.right)
        {
            chispaSprite.flipX = false;
        }
        else if (direccion == Vector2.left)
        {
            chispaSprite.flipX = true;
        }

        if (brujitaMove != null)
        {
            brujitaMove.enabled = false;
        }

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

        if (chispaCollider != null)
        {
            chispaCollider.enabled = true;
        }

        chispaAnimator.SetTrigger("Lanzar");

        Debug.Log("Chispa lanzada hacia: " + direccion);

        float tiempo = 0f;

        while (tiempo < launchDelay)
        {
            chispaObject.transform.localPosition +=
                (Vector3)direccion * velocidadChispa * Time.deltaTime;

            tiempo += Time.deltaTime;

            yield return null;
        }

        if (chispaCollider != null)
        {
            chispaCollider.enabled = false;
        }

        chispaObject.SetActive(false);
        chispaObject.transform.localPosition = posicionInicial;

        isCasting = false;
        isLaunching = false;

        brujitaAnimator.SetBool("IsCasting", false);

        if (brujitaMove != null)
        {
            brujitaMove.enabled = true;
        }
    }
}