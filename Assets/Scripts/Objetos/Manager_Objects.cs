using System.Collections.Generic;
using UnityEngine;

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
