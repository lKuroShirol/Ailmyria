using UnityEngine;

public class TamañoVariable : MonoBehaviour
{
    private Vector3 escalaOriginal;
    private float velocidadPulsacion;
    private float multiplicadorMaximo;
    private float offsetAleatorio;

    void Start()
    {
        escalaOriginal = transform.localScale;

        // Asignamos valores aleatorios únicos a cada punto para que no se muevan sincronizados
        velocidadPulsacion = UnityEngine.Random.Range(2f, 5f);
        multiplicadorMaximo = UnityEngine.Random.Range(1.2f, 1.4f); // Qué tanto crece
        offsetAleatorio = UnityEngine.Random.Range(0f, 10f);
    }

    void Update()
    {
        // Usamos una función de seno para un efecto de "respiración" suave y continuo
        float escalaFuctuada = Mathf.Sin((Time.time + offsetAleatorio) * velocidadPulsacion);
        float escalaActual = Mathf.Lerp(escalaOriginal.x * 0.8f, escalaOriginal.x * multiplicadorMaximo, (escalaFuctuada + 1f) / 2f);

        transform.localScale = new Vector3(escalaActual, escalaActual, escalaActual);
    }
}
