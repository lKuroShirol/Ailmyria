using UnityEngine;

public class Conseguir_Vendaval : MonoBehaviour
{
    Hechizos_Manager hechizosManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hechizosManager = collision.gameObject.GetComponent<Hechizos_Manager>();
            hechizosManager.ActivarVendaval();
            Destroy(gameObject);
        }
    }
}
