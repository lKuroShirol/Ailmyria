using UnityEngine;

public class ParallaxMejor : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Velocidad a la que se moverá hacia abajo.")]
    public float velocidad = 5f;

    private float altoSprite;
    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            altoSprite = spriteRenderer.bounds.size.y;
        }
        else
        {
            altoSprite = 10f;
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * velocidad * Time.deltaTime);

        if (transform.position.y <= posicionInicial.y - altoSprite)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + altoSprite, transform.position.z);
        }
    }
}
