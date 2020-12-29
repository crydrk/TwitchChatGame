using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CreatureChase : MonoBehaviour
{


    GameObject player;
    NavMeshAgent agent;

    public string EnemyName = "Mole";
    public float AttackPower = 1f;
    public float AttackRate = 1f;
    public float Speed = 3.5f;
    public float Health = 50f;
    public Transform TextParent;
    public Transform SpawnCamera;

    public float HealthLerpSpeed = 1f;
    private float currentHealth;

    public Transform HatAttach;
    public Transform HendLeftAttach;
    public Transform HandRightAttach;
    public GameObject DamagePrefab;
    public GameObject DeathPrefab;
    public TextMeshProUGUI NameText;
    public Slider HealthSlider;

    public string Description = "";
    
    void Start()
    {
        ProcessCreation(Description);

        player = GameObject.FindWithTag("Player");

        NameText.text = EnemyName;

        currentHealth = Health;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;

        InvokeRepeating("Attack", AttackRate, AttackRate);
    }
    
    void Update()
    {
        // Follow the player
        agent.SetDestination(player.transform.position);

        // Lerp the health bar nicely
        HealthSlider.value = Mathf.Lerp(HealthSlider.value, currentHealth / Health, Time.deltaTime * HealthLerpSpeed);

        // Check to see if the enemy died
        if (currentHealth <= 0f)
        {
            GameObject deathExplosion = (GameObject)Instantiate(DeathPrefab);
            deathExplosion.transform.position = transform.position;
            Destroy(gameObject);
        }

        // Aim the text at the main camera at all times
        TextParent.LookAt(2 * TextParent.position - Camera.main.transform.position);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            WeaponController_Base weapon = other.gameObject.GetComponent<WeaponController_Base>();
            currentHealth -= weapon.Damage;
            GameObject damageExplosion = (GameObject)Instantiate(DamagePrefab);
            damageExplosion.transform.position = transform.position;
        }
    }

    public void Attack()
    {
        RaycastHit hit;
        

        if (Physics.Raycast(transform.position, PlayerManager.SharedInstance.Player.transform.position - transform.position, out hit, 3f))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                PlayerManager.SharedInstance.TakeDamage(AttackPower);
            }
        }
    }
}
