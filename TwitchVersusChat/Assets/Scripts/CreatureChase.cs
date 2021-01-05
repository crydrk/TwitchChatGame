using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CreatureChase : EnemyBase
{
    public Transform SpawnCamera;
    
    public Transform HatAttach;
    public Transform HendLeftAttach;
    public Transform HandRightAttach;

    public string Description = "";
    
    protected override void Start()
    {
        base.Start();

        ProcessCreation(Description);

        CreatureManager.SharedInstance.AddCreature(this);
    }

    protected override void Update()
    {
        base.Update();
    }

    private void ProcessCreation(string input)
    {
        string[] keywords = input.Split(' ');

        for (int i = 0; i < keywords.Length; i++)
        {
            // Look for a name
            if (keywords[i] == "named" && i < keywords.Length - 1)
            {
                EnemyName = keywords[i + 1];
            }

            // Look for a hat
            if ((keywords[i].Contains("hat") || keywords[i].Contains("helmet") || keywords[i].Contains("cap") || keywords[i].Contains("crown")) && i != 0)
            {
                string hatName = keywords[i - 1];
                Hat hat = HatManager.SharedInstance.FindHatByName(hatName);
                if (hat != null)
                {
                    GameObject spawnedHat = (GameObject)Instantiate(hat.Model);
                    spawnedHat.transform.position = HatAttach.transform.position;
                    spawnedHat.transform.rotation = HatAttach.transform.rotation;
                    spawnedHat.transform.SetParent(HatAttach);
                    return;
                }

            }
        }
    }

    

    
}
