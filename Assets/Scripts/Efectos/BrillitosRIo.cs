using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;

public class BrillitosRio : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Lista de prefabs entre los que elegirá aleatoriamente (puedes poner 2 o más).")]
    public List<GameObject> listaPrefabs = new List<GameObject>();

    [Tooltip("Cantidad total de objetos que aparecerán.")]
    public int cantidadObjetos = 2;

    [Header("Área Rectangular")]
    [Tooltip("Centro del área de generación.")]
    public Vector2 centroArea = Vector2.zero;
    [Tooltip("Ancho del rectángulo.")]
    public float anchoArea = 10f;
    [Tooltip("Alto del rectángulo.")]
    public float altoArea = 5f;

    void Start()
    {
        GenerarObjetos();
    }

    void GenerarObjetos()
    {
        if (listaPrefabs == null || listaPrefabs.Count == 0)
        {
            Debug.LogWarning("Falta agregar al menos un Prefab en la lista del Inspector.");
            return;
        }

        float mitadAncho = anchoArea / 2f;
        float mitadAlto = altoArea / 2f;

        for (int i = 0; i < cantidadObjetos; i++)
        {
            float randomX = UnityEngine.Random.Range(centroArea.x - mitadAncho, centroArea.x + mitadAncho);
            float randomY = UnityEngine.Random.Range(centroArea.y - mitadAlto, centroArea.y + mitadAlto);

            Vector3 posicionSpawn = new Vector3(randomX, randomY, 0f);

          
            int indiceAleatorio = UnityEngine.Random.Range(0, listaPrefabs.Count);
            GameObject prefabSeleccionado = listaPrefabs[indiceAleatorio];

          
            if (prefabSeleccionado != null)
            {
                Instantiate(prefabSeleccionado, posicionSpawn, Quaternion.identity, transform);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(centroArea.x, centroArea.y, 0f), new Vector3(anchoArea, altoArea, 0f));
    }
}
