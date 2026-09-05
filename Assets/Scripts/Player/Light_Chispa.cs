using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Light_Chispa : MonoBehaviour
{
    private GameObject light_chispa;
    Chispa chispa;
    Hechizos_Manager hechizosManager;
    private Light2D luz;

    [SerializeField]float radioLuzNormal = 2f;
    [SerializeField]float radioLuzAnillo = 4f;
    [SerializeField]float radioLuzVendaval = 6f;

    private void Awake()
    {
        light_chispa = GetComponentInChildren<Light2D>().gameObject;
        chispa = GetComponentInParent<Chispa>();
        hechizosManager = GetComponentInParent<Hechizos_Manager>();
        luz = GetComponentInChildren<Light2D>();
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "Cueva" && chispa.isCasting)
        {
            light_chispa.SetActive(true);

            if(hechizosManager.anilloDesbloqueado && hechizosManager.vendavalDesbloqueada)
            {
                luz.pointLightOuterRadius = 6f;
            }
            else if(hechizosManager.anilloDesbloqueado )
            {
                luz.pointLightOuterRadius = 4f;
            }
            else
            {
                luz.pointLightOuterRadius = 2f;
            }


        }
        else
        {
            light_chispa.SetActive(false);
        }
    }

}
   


