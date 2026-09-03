using UnityEngine;
using static Marea;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Marea : MonoBehaviour
{
    public enum DireccionOndulacion { Vertical, Horizontal }

    [Header("Configuración de Eje")]
    public DireccionOndulacion direccion = DireccionOndulacion.Vertical;

    [Header("Parámetros de la Marea")]
    public float amplitud = 0.1f;
    public float frecuencia = 3f;
    public float velocidad = 2f;

    private Mesh meshOriginal;
    private Mesh meshInstancia;
    private Vector3[] verticesOriginales;

    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            // Obtenemos la malla y creamos una copia única para este objeto
            meshOriginal = meshFilter.sharedMesh;
            meshInstancia = Instantiate(meshOriginal);
            meshFilter.mesh = meshInstancia;

            verticesOriginales = meshInstancia.vertices;
        }
    }

    void Update()
    {
        if (meshInstancia == null || verticesOriginales == null) return;

        Vector3[] verticesActuales = new Vector3[verticesOriginales.Length];

        for (int i = 0; i < verticesOriginales.Length; i++)
        {
            Vector3 v = verticesOriginales[i];

            // Calculamos la onda
            float onda = Mathf.Sin((v.x + Time.time * velocidad) * frecuencia) * amplitud;

            if (direccion == DireccionOndulacion.Vertical)
            {
                v.y = verticesOriginales[i].y + onda;
            }
            else
            {
                v.x = verticesOriginales[i].x + onda;
            }

            verticesActuales[i] = v;
        }

        meshInstancia.vertices = verticesActuales;
        meshInstancia.RecalculateBounds();
    }
}
