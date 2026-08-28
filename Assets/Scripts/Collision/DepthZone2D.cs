using System.Collections.Generic;
using UnityEngine;

public class DepthZone2D : MonoBehaviour
{
    [SerializeField] private SpriteRenderer objectRenderer;

    [Header("Sorting Layers")]
    [SerializeField] private string behindPlayerLayer = "WorldBehind";
    [SerializeField] private string inFrontOfPlayerLayer = "WorldFront";

    private readonly HashSet<Collider2D> playerColliders = new();

    private void Awake()
    {
        if (objectRenderer == null)
        {
            objectRenderer = GetComponentInParent<SpriteRenderer>();
        }

        objectRenderer.sortingLayerName = behindPlayerLayer;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.transform.root.CompareTag("Player"))
            return;

        playerColliders.Add(other);

        BrujitaFlight vuelo =
            other.transform.root.GetComponent<BrujitaFlight>();

        // Avisamos a BrujitaFlight que está dentro de la zona.
        if (vuelo != null)
        {
            vuelo.EntrarDepthZone();

            // Si está volando, la zona NO debe ponerse
            // delante del jugador.
            if (vuelo.IsFlying)
                return;
        }

        // Está caminando dentro de la zona.
        objectRenderer.sortingLayerName = inFrontOfPlayerLayer;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.transform.root.CompareTag("Player"))
            return;

        playerColliders.Remove(other);

        BrujitaFlight vuelo =
            other.transform.root.GetComponent<BrujitaFlight>();

        if (vuelo != null)
        {
            vuelo.SalirDepthZone();

            // Si está volando, ignoramos completamente
            // el sorting de la DepthZone.
            if (vuelo.IsFlying)
                return;
        }

        if (playerColliders.Count == 0)
        {
            objectRenderer.sortingLayerName = behindPlayerLayer;
        }
    }
}