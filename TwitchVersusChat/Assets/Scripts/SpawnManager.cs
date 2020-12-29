using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreatureArchetype
{
    public string Name;
    public GameObject Prefab;
}

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager SharedInstance;

    public List<Transform> SpawnLocations = new List<Transform>();
    public List<CreatureArchetype> Creatures = new List<CreatureArchetype>();

    private bool spawnNextFrame = false;
    private string nextFrameUser;
    private string nextFrameDescription;
    private CreatureArchetype nextFrameCreature;

    private void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnNextFrame)
        {
            SpawnCreature(nextFrameUser, nextFrameDescription, nextFrameCreature);
            spawnNextFrame = false;
        }
    }

    void SpawnCreature(string username, string description, CreatureArchetype creatureType)
    {
        GameObject spawnedCreature = (GameObject)Instantiate(creatureType.Prefab);
        spawnedCreature.transform.position = SpawnLocations[Random.Range(0, SpawnLocations.Count)].position;
        CreatureChase chaseScript = spawnedCreature.GetComponent<CreatureChase>();
        chaseScript.Description = description;

        MinionCameraManager.SharedInstance.AddMinionToStack(chaseScript.SpawnCamera, username + " spawned " + description);
    }

    public void ReceiveData(string data)
    {
        // data types: comment, follower, raid
        string[] splitData = data.Split('^');
        if (splitData[0] == "comment")
        {
            // Do comment logic to spawn creature
            foreach (CreatureArchetype c in Creatures)
            {
                string[] names = c.Name.Split(',');
                foreach (string name in names)
                {
                    if (splitData[2].Contains(name))
                    {
                        spawnNextFrame = true;
                        nextFrameUser = splitData[1];
                        nextFrameDescription = splitData[2];
                        nextFrameCreature = c;
                        return;
                    }
                }
            }
        }
        else if (splitData[0] == "follower")
        {
            // Do comment logic to create boss
        }
        else if (splitData[0] == "raid")
        {
            // Do comment logic to spawn raid minions
        }
    }
}
