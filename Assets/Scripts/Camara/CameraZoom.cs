using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraZoom : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField] private float zoomNormal = 5f;
    [SerializeField] private float zoomCueva = 2f;

    private Camera camara;
    private Hechizos_Manager hechizosManager;

    private void Awake()
    {
        camara = GetComponent<Camera>();
        hechizosManager = FindAnyObjectByType<Hechizos_Manager>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Cueva")
        {
            camara.orthographicSize = zoomNormal;
            return;
        }

        if (hechizosManager != null &&
            hechizosManager.anilloDesbloqueado &&
            hechizosManager.vendavalDesbloqueada)
        {
            camara.orthographicSize = zoomNormal;
        }
        else
        {
            camara.orthographicSize = zoomCueva;
        }
    }
}