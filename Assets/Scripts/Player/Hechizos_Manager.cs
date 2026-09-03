using UnityEngine;

public class Hechizos_Manager : MonoBehaviour
{
    [Header("Hechizos")]
    [SerializeField] private Chispa chispa;
    public bool chispaDesbloqueada = false;

    [SerializeField] private Viento viento;
    public bool vientoDesbloqueada = false;


    [Header("Mejoras")]
    public bool anilloDesbloqueado = false;


    private void Awake()
    {
        if (chispa == null)
        {
            chispa = GetComponent<Chispa>();
        }
        if (viento == null)
        {
            viento = GetComponent<Viento>();
        }
    }

    public void ActivarViento()
    {
        vientoDesbloqueada = true;
    }

    public void DesactivarViento()
    {
        vientoDesbloqueada = false;
    }

    public void ActivarChispa()
    {
        chispaDesbloqueada = true;
    }

    public void DesactivarChispa()
    {
        chispaDesbloqueada = false;
    }

    public void ActivarAnillo()
    {
        anilloDesbloqueado = true;
    }
    public void DesactivarAnillo()
    {
        anilloDesbloqueado = false;
    }
}