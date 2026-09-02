using UnityEngine;

public class Chispa_Collider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("DesObjects"))
        {
            Objects_destroyd objeto = collision.GetComponent<Objects_destroyd>();

            if (objeto != null)
            {
                objeto.DestruirObjeto();
            }
        }
    }
}
