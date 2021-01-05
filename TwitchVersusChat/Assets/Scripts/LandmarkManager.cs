using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkManager : MonoBehaviour
{
    public static LandmarkManager SharedInstance;
    public List<Landmark> Landmarks = new List<Landmark>();
    
    void Awake()
    {
        SharedInstance = this;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!Landmarks[0].IsDestroyed)
            {
                CreatureManager.SharedInstance.RetargetCreaturesByUsername("Steakosaurus_rex", Landmarks[0].transform);
            }
        }
    }
}
