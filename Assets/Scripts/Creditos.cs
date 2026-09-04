using UnityEngine;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float tiempoEspera = 23f; // Tiempo de espera en segundos
    void Start()
    {
       Invoke("WaitForEnd", tiempoEspera); // Llama a WaitForEnd después del tiempo de espera
    }


    public void WaitForEnd()
    {
        SceneManager.LoadScene("INTRO");
    }
}
