using UnityEngine;
using UnityEngine.SceneManagement;

public class Objects_destroyd : MonoBehaviour
{
    [SerializeField] private int id;

    private void Start()
    {
        Manager_Objects manager = FindFirstObjectByType<Manager_Objects>();

        if (manager != null)
        {
            string escena = SceneManager.GetActiveScene().name;

            if (manager.EstaDestruido(escena, id))
            {
                Debug.Log($"[OBJETO DESTRUIDO] Escena: {escena} | Objeto: {gameObject.name} | ID: {id}");
                Destroy(gameObject);
            }
        }
    }

    public void DestruirObjeto()
    {
        Manager_Objects manager = FindFirstObjectByType<Manager_Objects>();

        if (manager != null)
        {
            manager.RegistarDestruccion(
                SceneManager.GetActiveScene().name,
                id
            );
        }

        Destroy(gameObject);
    }
}
