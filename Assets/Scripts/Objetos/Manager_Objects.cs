using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager_Objects : MonoBehaviour
{
    private static Manager_Objects instance;

    private HashSet<string> destroyedObjects = new HashSet<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
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
            destroyedObjects.Clear();
        }
    }

    public void RegistarDestruccion(string nombreEscena, int id)
    {
        string key = $"{nombreEscena}_{id}";
        destroyedObjects.Add(key);
    }

    public bool EstaDestruido(string nombreEscena, int id)
    {
        string key = $"{nombreEscena}_{id}";
        return destroyedObjects.Contains(key);
    }
}