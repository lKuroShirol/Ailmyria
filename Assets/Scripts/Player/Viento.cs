using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Viento : MonoBehaviour
{
    //public bool isCasting = false;

    //[SerializeField] private GameObject VientoObject;
    //[SerializeField] private Animator VientoAnimator;
    //[SerializeField] private Collider2D VientoCollider;

    //[Header("Lanzamiento")]
    //[SerializeField] private float launchDelay = 1.5f;
    //[SerializeField] private float velocidadViento = 5f;
    //private bool isLaunching = false;

    //private Hechizos_Manager hechizosManager;
    //private BrujitaMove brujitaMove;
    //public Key TeclaViento = Key.V;

    //private Vector3 posicionInicial;
    //private Animator brujitaAnimator;
    //private SpriteRenderer VientoSprite;
    //private void Awake()
    //{
    //    posicionInicial = VientoObject.transform.localPosition;

    //    VientoSprite = VientoObject.GetComponent<SpriteRenderer>();

    //    VientoObject.SetActive(false);

    //    if (VientoCollider != null)
    //    {
    //        VientoCollider.enabled = false;
    //    }

    //    brujitaMove = GetComponent<BrujitaMove>();
    //    hechizosManager = GetComponent<Hechizos_Manager>();
    //    brujitaAnimator = GetComponent<Animator>();

    //    isCasting = false;
    //    isLaunching = false;
    //}

    //private void Update()
    //{
    //    if (!hechizosManager.vientoDesbloqueada)
    //        return;

    //    if (Keyboard.current[TeclaViento].wasPressedThisFrame)
    //    {
    //        if (!isCasting)
    //        {
    //            isCasting = true;
    //            VientoObject.SetActive(true);

    //            VientoObject.transform.localPosition = posicionInicial;

    //            Debug.Log("Viento encendido");
    //        }
    //        else if (!isLaunching)
    //        {
    //            StartCoroutine(LanzarViento());
    //        }
    //    }
    //}

    //private IEnumerator LanzarViento()
    //{
    //    isLaunching = true;
    //    isCasting = true;

    //    VientoObject.transform.localPosition = posicionInicial;

    //    Vector2 direccion = brujitaMove.DireccionHorizontal;

    //    if (direccion == Vector2.right)
    //    {
    //        VientoSprite.flipX = false;
    //    }
    //    else if (direccion == Vector2.left)
    //    {
    //        VientoSprite.flipX = true;
    //    }

    //    if (brujitaMove != null)
    //    {
    //        brujitaMove.enabled = false;
    //    }

    //    if (direccion.x > 0)
    //    {
    //        brujitaAnimator.SetFloat("LanzamientoDireccion", 1f);
    //    }
    //    else
    //    {
    //        brujitaAnimator.SetFloat("LanzamientoDireccion", -1f);
    //    }

    //    brujitaAnimator.SetBool("IsCasting", true);

    //    yield return new WaitForSeconds(0.3f);

    //    if (VientoCollider != null)
    //    {
    //        VientoCollider.enabled = true;
    //    }

    //    VientoAnimator.SetTrigger("Lanzar");

    //    Debug.Log("Viento lanzado hacia: " + direccion);

    //    float tiempo = 0f;

    //    while (tiempo < launchDelay)
    //    {
    //        VientoObject.transform.localPosition +=
    //            (Vector3)direccion * velocidadViento * Time.deltaTime;

    //        tiempo += Time.deltaTime;

    //        yield return null;
    //    }

    //    if (VientoCollider != null)
    //    {
    //        VientoCollider.enabled = false;
    //    }

    //    VientoObject.SetActive(false);
    //    VientoObject.transform.localPosition = posicionInicial;
    //    isCasting = false;
    //    isLaunching = false;

    //    brujitaAnimator.SetBool("IsCasting", false);

    //    if (brujitaMove != null)
    //    {
    //        brujitaMove.enabled = true;
    //    }
    //}
}

//using UnityEngine;

//public class Flotar : MonoBehaviour
//{
//    [Header("Configuración de Flotación")]
//    public float amplitud = 0.5f;

//    [Tooltip("Qué tan rápido realizará el ciclo de arriba a abajo (velocidad).")]
//    public float frecuencia = 2f;

//    [Header("Rotación Opcional")]
//    public bool girarObjeto = true;
//    public float velocidadGiro = 50f;

//    private Vector3 posicionInicial;

//    private BrujitaMove brujitamove;

//    void Start()
//    {
//        posicionInicial = transform.position;
//        brujitamove = FindAnyObjectByType<BrujitaMove>();
//    }

//    void Update()
//    {
//        float desplazamientoY = Mathf.Sin(Time.time * frecuencia) * amplitud;

//        transform.position = new Vector3(
//            posicionInicial.x,
//            posicionInicial.y + desplazamientoY,
//            posicionInicial.z
//        );

//        if (girarObjeto)
//        {
//            transform.Rotate(
//                Vector3.up,
//                velocidadGiro * Time.deltaTime,
//                Space.World
//            );
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            print("FUNCIONAA");

//            if (brujitamove != null)
//            {
//                brujitamove.TATATATAAA();
//            }

//            gameObject.SetActive(false);
//        }
//    }
//}