using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHojitas : MonoBehaviour
{
    [Header("Configuración de Prefabs")]
    [Tooltip("Lista de prefabs de hojas")]
    public List<GameObject> listaPrefabsHojas = new List<GameObject>();

    [Header("Frecuencia de Aparición")]
    [Tooltip("Segundos que pasan entre la aparición de cada hoja.")]
    public float frecuenciaSpawn = 1.5f;
    [Tooltip("Variación aleatoria")]
    public float variacionTiempo = 0.5f;

    [Header("Área de Aparición (Ancho)")]
    [Tooltip("Ancho de la línea horizontal superior donde se dispersarán las hojas.")]
    public float anchoAreaSpawn = 12f;

    void Start()
    {
        // Iniciamos la rutina de generación continua
        StartCoroutine(RutinaSpawnHojas());
    }

    IEnumerator RutinaSpawnHojas()
    {
        while (true)
        {
            SpawnearHoja();

            // Calculamos el tiempo de espera combinando la frecuencia base y una variación aleatoria
            float tiempoEspera = Mathf.Max(0.1f, frecuenciaSpawn + UnityEngine.Random.Range(-variacionTiempo, variacionTiempo));
            yield return new WaitForSeconds(tiempoEspera);
        }
    }

    void SpawnearHoja()
    {
        if (listaPrefabsHojas == null || listaPrefabsHojas.Count == 0)
        {
            Debug.LogWarning("Falta agregar prefabs de hojas en la lista del Spawner.");
            return;
        }

        // 1. Elegir un prefab al azar de la lista (puedes tener 2, 3 o los que quieras)
        int indiceAleatorio = UnityEngine.Random.Range(0, listaPrefabsHojas.Count);
        GameObject prefabSeleccionado = listaPrefabsHojas[indiceAleatorio];

        if (prefabSeleccionado != null)
        {
            // 2. Calcular una posición aleatoria a lo largo del ancho definido
            float randomX = UnityEngine.Random.Range(-anchoAreaSpawn / 2f, anchoAreaSpawn / 2f);
            Vector3 posicionSpawn = transform.position + new Vector3(randomX, 0f, 0f);

            // 3. Instanciar la hoja elegida
            Instantiate(prefabSeleccionado, posicionSpawn, Quaternion.identity);
        }
    }

    // Dibuja una línea roja en la escena para visualizar el área horizontal de spawn
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position + new Vector3(-anchoAreaSpawn / 2f, 0f, 0f),
            transform.position + new Vector3(anchoAreaSpawn / 2f, 0f, 0f)
        );
    }
}
