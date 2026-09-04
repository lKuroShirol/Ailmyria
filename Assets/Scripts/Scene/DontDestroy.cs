using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    private static GameObject[] persistentObject = new GameObject[30];

    public int objectIndex;

    private void Awake()
    {
        if (persistentObject[objectIndex] == null)
        {
            persistentObject[objectIndex] = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else if (persistentObject[objectIndex] != gameObject)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "INTRO")
        {
            for (int i = 0; i < persistentObject.Length; i++)
            {
                if (persistentObject[i] != null)
                {
                    Destroy(persistentObject[i]);
                    persistentObject[i] = null;
                }
            }
        }
    }
}