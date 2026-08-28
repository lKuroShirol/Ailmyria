using UnityEngine;

public class ParallaxMENU : MonoBehaviour
{
    [System.Serializable]
    public class CapaParallax
    {
        public Transform transformCapa;  
        public float velocidadMovimiento;   
    }

    [Header("Configuración de Capas")]
    public CapaParallax[] capas;         

    private float anchoSprite;

    void Start()
    {
        if (capas.Length > 0 && capas[0].transformCapa != null)
        {
            SpriteRenderer spriteRenderer = capas[0].transformCapa.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
               
                anchoSprite = spriteRenderer.bounds.size.x;
            }
            else
            {
                Debug.LogWarning("ERROR ERROR ERROR WIIIIU WIIIU Esto funciona con SPR");
                anchoSprite = 20f;
            }
        }
    }

    void Update()
    {
       
        foreach (var capa in capas)
        {
            if (capa.transformCapa == null) continue;
         
            capa.transformCapa.Translate(Vector3.left * capa.velocidadMovimiento * Time.deltaTime);           
            if (Mathf.Abs(transform.position.x - capa.transformCapa.position.x) >= anchoSprite)
            {
                float offsetDesplazamiento = anchoSprite * 2f;
                capa.transformCapa.position = new Vector3(capa.transformCapa.position.x + offsetDesplazamiento, capa.transformCapa.position.y, capa.transformCapa.position.z);
            }
        }
    }
}
