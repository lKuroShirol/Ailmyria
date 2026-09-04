using UnityEngine;

public class TriggerDialogo : MonoBehaviour
{
    [Header("Configuración de la Zona")]
    [Tooltip("Si está marcado, esta zona de diálogo solo se podrá activar una vez y se destruirá o desactivará.")]
    public bool soloUnaVez = true;

    [Header("Referencia al Sistema de Diálogo")]
    [Tooltip("Arrastra aquí el objeto o NPC que contiene el script de DialogoNPC que debe activarse al cruzar esta zona.")]
    public NPCInteraccionSecuencial sistemaDialogoObjetivo;

    [Tooltip("Opcional: Si prefieres buscar el diálogo en este mismo objeto, déjalo vacío e intentará buscarlo aquí.")]
    private bool yaSeActivo = false;

    void Awake()
    {
        // Si no asignaste el script manualmente, intenta buscarlo en este mismo GameObject
        if (sistemaDialogoObjetivo == null)
        {
            sistemaDialogoObjetivo = GetComponent<NPCInteraccionSecuencial>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (soloUnaVez && yaSeActivo) return;

       
        if (collision.CompareTag("Player"))
        {
            print("Brujita choco con dialogo");
            // Activamos la condición de diálogo y forzamos el inicio
            sistemaDialogoObjetivo.ManejarDialogo();   
                yaSeActivo = true;
                GetComponent<Collider2D>().enabled = false;
  
        }
    }
}
