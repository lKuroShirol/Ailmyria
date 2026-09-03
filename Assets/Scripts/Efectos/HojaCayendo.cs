using UnityEngine;

public class HojaCayendo : MonoBehaviour
{
    [Header("Configuración de Caída (Rango)")]
    public float velocidadMinima = 1.5f;
    public float velocidadMaxima = 3f;
    private float velocidadCaida;

    [Header("Configuración del Zig-Zag (Rango)")]
    public float amplitudMinima = 1f;
    public float amplitudMaxima = 2.5f;
    private float amplitudZigZag;

    public float frecuenciaMinima = 0.8f;
    public float frecuenciaMaxima = 1.5f;
    private float frecuenciaZigZag;

    [Header("Configuración de Rotación (Rango)")]
    public float giroMinimo = 50f;
    public float giroMaximo = 150f;
    private float velocidadGiro;

    [Tooltip("Si el giro también debe oscilar")]
    public bool oscilarGiro = true;

    [Header("Limpieza")]
    [Tooltip("Tiempo en segundos antes de que la hoja se destruya sola.")]
    public float tiempoDeVida = 10f;

    private float tiempoInicio;

    void Start()
    {
        tiempoInicio = Time.time;

        velocidadCaida = UnityEngine.Random.Range(velocidadMinima, velocidadMaxima);
        amplitudZigZag = UnityEngine.Random.Range(amplitudMinima, amplitudMaxima);
        frecuenciaZigZag = UnityEngine.Random.Range(frecuenciaMinima, frecuenciaMaxima);
        velocidadGiro = UnityEngine.Random.Range(giroMinimo, giroMaximo);

        transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));
        Destroy(gameObject, tiempoDeVida);
    }

    void Update()
    {
        // Caída constante hacia abajo
        transform.Translate(Vector3.down * velocidadCaida * Time.deltaTime, Space.World);

        // Movimiento zig-zag lateral
        float desplazamientoX = Mathf.Sin((Time.time - tiempoInicio) * frecuenciaZigZag) * amplitudZigZag * Time.deltaTime;
        transform.Translate(new Vector3(desplazamientoX, 0f, 0f), Space.World);

        // Rotación sobre su propio eje
        float giroActual = velocidadGiro;

        if (oscilarGiro)
        {
            giroActual += Mathf.Sin((Time.time - tiempoInicio) * frecuenciaZigZag * 2f) * (velocidadGiro * 0.5f);
        }

        transform.Rotate(Vector3.forward * giroActual * Time.deltaTime);
    }
}
