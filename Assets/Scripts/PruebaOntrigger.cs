using UnityEngine;

public class PruebaOntrigger : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.CompareTag("XD"))
        {
            Debug.Log("¡FUNCIONAA! El jugador ha cruzado el objeto.");
        }
           

        
    }
}
