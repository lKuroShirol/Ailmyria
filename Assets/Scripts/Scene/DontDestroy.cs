using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static GameObject[] persistentObject = new GameObject[10];
    public int objectIndex;
    void Awake()
    {
        if(persistentObject[objectIndex] == null)
        {

            persistentObject[objectIndex] = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else if(persistentObject[objectIndex] != gameObject)
        {
            Destroy(gameObject);
        }
    }
}
