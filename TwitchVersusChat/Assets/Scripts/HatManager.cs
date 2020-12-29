using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hat
{
    public string Name;
    public GameObject Model;
}

public class HatManager : MonoBehaviour
{
    public static HatManager SharedInstance;
    public List<Hat> Hats = new List<Hat>();

    private void Awake()
    {
        SharedInstance = this;
    }

    public Hat FindHatByName(string name)
    {
        foreach (Hat hat in Hats)
        {
            string[] splitHatNames = hat.Name.Split(',');
            foreach (string nameVariant in splitHatNames)
            {
                if (name.ToLower().Contains(nameVariant))
                {
                    return hat;
                }
            }
        }

        return null;
    }
    
}
