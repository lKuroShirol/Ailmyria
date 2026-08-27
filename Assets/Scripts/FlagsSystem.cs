using System.Collections.Generic;
using UnityEngine;

public class FlagsSystem : MonoBehaviour
{
    private static Dictionary<string, bool> banderas = new Dictionary<string, bool>();

    public static void SetFlag(string nombreBandera, bool valor)
    {
        banderas[nombreBandera] = valor;
    }

    public static bool GetFlag(string nombreBandera)
    {
        if (banderas.TryGetValue(nombreBandera, out bool valor))
        {
            return valor;
        }
        return false; 
    }
}
