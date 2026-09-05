using UnityEngine;

public class Flotar : MonoBehaviour
{
    [Header("Configuración de Flotación")]
    public float amplitud = 0.5f;

    [Tooltip("Qué tan rápido realizará el ciclo de arriba a abajo (velocidad).")]
    public float frecuencia = 2f;

    [Header("Rotación Opcional")]
    public bool girarObjeto = true;
    public float velocidadGiro = 50f;

    private Vector3 posicionInicial;

    [Header("Objeto a Desactivar")]
    public GameObject ObjetoDesactivable;

    private BrujitaMove brujitamove;

    void Start()
    {
        // Guarda la posición donde está colocado originalmente
        posicionInicial = transform.position;

        brujitamove = FindAnyObjectByType<BrujitaMove>();
    }

    void Update()
    {
        float desplazamientoY = Mathf.Sin(Time.time * frecuencia) * amplitud;

        transform.position = new Vector3(
            posicionInicial.x,
            posicionInicial.y + desplazamientoY,
            posicionInicial.z
        );

        if (girarObjeto)
        {
            transform.Rotate(
                Vector3.up,
                velocidadGiro * Time.deltaTime,
                Space.World
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("FUNCIONAA");

            if (brujitamove != null)
            {
                brujitamove.TATATATAAA();
            }

            if (ObjetoDesactivable != null)
            {
                ObjetoDesactivable.SetActive(false);
            }
        }
    }
}