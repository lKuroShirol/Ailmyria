using UnityEngine;

public class Hechizos_Manager : MonoBehaviour
{
    [Header("Hechizos")]
    [SerializeField] private Chispa chispa;
    public bool chispaDesbloqueada = false;

    private void Awake()
    {
        if (chispa == null)
        {
            chispa = GetComponent<Chispa>();
        }
    }

    public void ActivarChispa()
    {
        chispaDesbloqueada = true;
    }

    public void DesactivarChispa()
    {
        chispaDesbloqueada = false;
    }
}
