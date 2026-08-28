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

        // El jugador está detrás del objeto,
        // así que el objeto debe dibujarse delante.
        objectRenderer.sortingLayerName = inFrontOfPlayerLayer;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.transform.root.CompareTag("Player"))
            return;

        playerColliders.Remove(other);

        if (playerColliders.Count == 0)
        {
            // El jugador ya no está detrás.
            objectRenderer.sortingLayerName = behindPlayerLayer;
        }
    }
}