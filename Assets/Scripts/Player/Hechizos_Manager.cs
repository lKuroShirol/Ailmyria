using UnityEngine;

public class Hechizos_Manager : MonoBehaviour
{
    [Header("Hechizos")]
    [SerializeField] private Chispa chispa;
    public bool chispaDesbloqueada = false;

    [SerializeField] private Viento viento;
    public bool vientoDesbloqueada = false;

    [SerializeField] private Vendaval vendaval;
    public bool vendavalDesbloqueada = false;

    [Header("Mejoras")]
    public bool anilloDesbloqueado = false;

    [Header("Objetos")]
    public bool varitaDesbloqueada = false;

    private static Hechizos_Manager instance;

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
        if (vendaval == null)
        {
            vendaval = GetComponent<Vendaval>();
        }
    }
    public void ActivarVarita()
    {
      
            varitaDesbloqueada = true; 
        
    }

    public void DesactivarVarita()
    {
        varitaDesbloqueada = false;
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

    public void ActivarVendaval()
    {
        vendavalDesbloqueada = true;
    }

    public void DesactivarVendaval()
    {
        vendavalDesbloqueada = false;
    }
}