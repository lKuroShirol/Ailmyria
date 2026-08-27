using System.Collections;
using UnityEngine;

public class MusiquitaManager : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private AudioSource audioSource;

    [Header("Rangos de Tiempo (Segundos)")]
    [Tooltip("Tiempo mínimo (a) de espera entre audios.")]
    public float tiempoMinimo = 5f;
    [Tooltip("Tiempo máximo (b) de espera entre audios.")]
    public float tiempoMaximo = 15f;

    [Header("Variación de Tono (Pitch)")]
    [Tooltip("Tono mínimo (ej. 0.9 para un sonido un poco más grave).")]
    public float tonoMinimo = 0.9f;
    [Tooltip("Tono máximo (ej. 1.1 para un sonido un poco más agudo).")]
    public float tonoMaximo = 1.1f;

    [Header("Opciones")]
    public bool reproducirAlIniciar = true;

    private void Awake()
    {
        
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        if (reproducirAlIniciar)
        {
            IniciarReproduccionAleatoria();
        }
    }

    public void IniciarReproduccionAleatoria()
    {
        StartCoroutine(BucleReproduccionAleatoria());
    }

    public void DetenerReproduccionAleatoria()
    {
        StopAllCoroutines();
    }

    private IEnumerator BucleReproduccionAleatoria()
    {
        while (true)
        {
           
            float tiempoEspera = Random.Range(tiempoMinimo, tiempoMaximo);
            yield return new WaitForSeconds(tiempoEspera);

            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.pitch = Random.Range(tonoMinimo, tonoMaximo);

                audioSource.Play();
            }
        }
    }
}
