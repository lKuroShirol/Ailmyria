using UnityEngine;

public class ConseguirVarita : MonoBehaviour
{
    Hechizos_Manager hechizosManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hechizosManager = collision.gameObject.GetComponent<Hechizos_Manager>();
            hechizosManager.ActivarVarita();
            Destroy(gameObject);
        }
    }
}
