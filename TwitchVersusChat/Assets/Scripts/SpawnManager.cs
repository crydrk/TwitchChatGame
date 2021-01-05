using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum BossOrigin
{
    Follow,
    Raid
}

[System.Serializable]
public class CreatureArchetype
{
    public string Name;
    public GameObject Prefab;
}

[System.Serializable]
public class BossArchetype
{
    public string Name;
    public GameObject Prefab;
    public GameObject Minion;
}

[System.Serializable]
public class BossStackable
{
    public string Name;
    public BossArchetype BossType;
    public BossOrigin Origin;
    public int NumRaiders;
    public string Message;

    public BossStackable(string n, BossArchetype a, BossOrigin origin, int numRaiders = 0)
    {
        Name = n;
        BossType = a;
        Origin = origin;
        NumRaiders = numRaiders;

        if (origin == BossOrigin.Follow)
        {
            Message = "BOSS INCOMING!!\n" + Name + " has followed!";
        }
        else if (origin == BossOrigin.Raid)
        {
            Message = "BOSS INCOMING!!\n" + Name + " has raided the channel\n and brought " + numRaiders.ToString() + " minions to fight!";
        }
    }
}

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager SharedInstance;

    public List<Transform> CreatureSpawnLocations = new List<Transform>();
    public Transform BossSpawnLocation;
    public List<Transform> MinionSpawnLocations = new List<Transform>();
    public List<CreatureArchetype> Creatures = new List<CreatureArchetype>();
    public List<BossArchetype> Bosses = new List<BossArchetype>();

    public float MinTimeBetweenBosses = 10f;
    private float bossTimer = 0f;
    private List<BossStackable> BossStack = new List<BossStackable>();

    public float PreBossSpawnPause = 5f;
    public float BossSpawnPause = 5f;

    public Camera BossCam;
    public GameObject BossUI;
    public TextMeshProUGUI BossText;

    private void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("TwitchCallbacks", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnRandomCreature();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            AddBoss("Steakosaurus_rex", BossOrigin.Follow);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            AddBoss("Steakosaurus_rex", BossOrigin.Raid, 10);
        }

        // Spawn bosses with a delay in between
        if (BossStack.Count > 0 && bossTimer <= 0f)
        {
            SpawnBoss(BossStack[0]);
            BossStack.RemoveAt(0);
            bossTimer = MinTimeBetweenBosses;
        }
        if (bossTimer > 0f && !GameplayManager.SharedInstance.Paused)
        {
            bossTimer -= Time.deltaTime;
        }
    }

    void TwitchCallbacks()
    {
        TwitchClient.SharedInstance.client.OnMessageReceived += MessageReceived;
        TwitchClient.SharedInstance.client.OnRaidNotification += Raid;
        TwitchClient.SharedInstance.FollowerService.OnNewFollowersDetected += NewFollower;
    }

    void SpawnCreature(string username, string description, CreatureArchetype creatureType)
    {
        GameObject spawnedCreature = (GameObject)Instantiate(creatureType.Prefab);
        spawnedCreature.transform.position = CreatureSpawnLocations[Random.Range(0, CreatureSpawnLocations.Count)].position;
        CreatureChase chaseScript = spawnedCreature.GetComponent<CreatureChase>();
        chaseScript.Description = description;
        chaseScript.PlayerName = username;

        MinionCameraManager.SharedInstance.AddMinionToStack(chaseScript.SpawnCamera, username + " spawned " + description);
    }

    void SpawnMinion(GameObject prefab, Transform spawnLocation)
    {
        GameObject spawnedMinion = (GameObject)Instantiate(prefab);
        spawnedMinion.transform.position = spawnLocation.position;
        CreatureChase chaseScript = spawnedMinion.GetComponent<CreatureChase>();
    }

    void AddBoss(string username, BossOrigin origin, int numRaiders = 0)
    {
        BossStack.Add(new BossStackable(username, Bosses[0], origin, numRaiders));
    }

    void SpawnBoss(BossStackable boss)
    {
        bool completedPausing = GameplayManager.SharedInstance.PauseGameplay(PreBossSpawnPause + BossSpawnPause);
        if (!completedPausing)
        {
            Debug.LogWarning("Failed to complete pausing");
            return;
        }

        StartCoroutine(ManageCameraAndSpawn(PreBossSpawnPause, BossSpawnPause, boss));
    }

    private IEnumerator ManageCameraAndSpawn(float cameraHoldDuration, float introDuration, BossStackable boss)
    {
        BossUI.SetActive(true);
        BossText.text = boss.Message;

        yield return new WaitForSeconds(cameraHoldDuration);

        if (boss.Origin == BossOrigin.Raid)
        {
            StartCoroutine(SpawnMinions(boss.BossType.Minion, boss.NumRaiders, 0.5f));
        }

        BossUI.SetActive(false);

        BossCam.enabled = true;

        // Do all the stuff to spawn it
        GameObject spawnedBoss = (GameObject)Instantiate(boss.BossType.Prefab);
        spawnedBoss.transform.position = BossSpawnLocation.position;
        spawnedBoss.transform.rotation = BossSpawnLocation.rotation;
        Boss bossScript = spawnedBoss.GetComponent<Boss>();
        bossScript.EnemyName = boss.BossType.Name;
        StartCoroutine(bossScript.MakePauseAnimImmune(10f));

        yield return new WaitForSeconds(introDuration);

        BossCam.enabled = false;
    }

    private void SpawnRandomCreature()
    {
        //string[] exampleNames = { "Steakosaurus_rex", "Vykmiggs", "chuichuichu", "stinkoman240", "STANKOSAURUS", "Doomasaur96", "Perkins", "Tigerboots" };
        string[] exampleNames = {"Steakosaurus_rex"};
        string[] exampleCreatures = { "mole", "steakosaurus", "cactus" };
        string[] exampleHats = { "army", "astronaut", "ball", "beach", "beer", "bike", "cactus", "captain", "chef", "cowboy", "crown", "detective", "dinosaur",
            "diving", "elephant", "elf", "fire", "french", "fruit", "gladiator", "graduation", "hard", "kids", "jester", "knight", "leprechaun", "fedora",
            "toad", "party", "pharaoh", "pirate", "police", "pope", "safari", "santa", "shark", "sombrero", "umbrella", "july", "unicorn", "witch",
            "top", "viking"};

        string userName = exampleNames[Random.Range(0, exampleNames.Length)];
        string creatureType = exampleCreatures[Random.Range(0, exampleCreatures.Length)];
        string description = "a " + creatureType + " wearing a " + exampleHats[Random.Range(0, exampleHats.Length)] + " hat";

        // Do comment logic to spawn creature
        foreach (CreatureArchetype c in Creatures)
        {
            string[] names = c.Name.Split(',');
            foreach (string name in names)
            {
                if (description.Contains(name))
                {
                    SpawnCreature(userName, description, c);
                    return;
                }
            }
        }
    }

    private void NewFollower(object sender, TwitchLib.Api.Services.Events.FollowerService.OnNewFollowersDetectedArgs e)
    {
        Debug.Log("New follower!");
        foreach (TwitchLib.Api.Interfaces.IFollow follower in e.NewFollowers)
        {
            AddBoss(follower.User.DisplayName.ToString(), BossOrigin.Follow);
        }
    }

    private void Raid(object sender, TwitchLib.Client.Events.OnRaidNotificationArgs e)
    {
        string raiderName = e.RaidNotificaiton.DisplayName;
        string numRaidersString = e.RaidNotificaiton.MsgParamViewerCount;
        int numRaiders = int.Parse(numRaidersString);

        AddBoss(raiderName, BossOrigin.Raid, numRaiders);
    }

    private IEnumerator SpawnMinions(GameObject prefab, int numToSpawn, float timeBetweenSpawns)
    {
        for (int i = 0; i < numToSpawn; i++)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            SpawnMinion(prefab, MinionSpawnLocations[i%2]);
        }
    }

    private void MessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
    {
        string description = e.ChatMessage.Message;
        string userName = e.ChatMessage.DisplayName;
        string userId = e.ChatMessage.UserId;

        // Do comment logic to spawn creature
        foreach (CreatureArchetype c in Creatures)
        {
            string[] names = c.Name.Split(',');
            foreach (string name in names)
            {
                if (description.Contains(name))
                {
                    SpawnCreature(userName, description, c);
                    return;
                }
            }
        }
    }
}
