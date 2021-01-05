using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    private GameObject player;
    public NavMeshAgent agent;
    public Animator Anim;

    public string EnemyName = "Mole";
    [HideInInspector]
    public string PlayerName = "Game";
    public float AttackPower = 1f;
    public float AttackRate = 1f;
    public float Speed = 3.5f;
    public float Health = 50f;
    public Transform TextParent;

    public float HealthLerpSpeed = 1f;
    protected float currentHealth;
    public SkinnedMeshRenderer MeshObject;
    public float DamageDisplayDuration = 0.25f;
    private float damageTimer;

    public GameObject DamagePrefab;
    public GameObject DeathPrefab;
    public TextMeshProUGUI NameText;
    public Slider HealthSlider;

    protected bool canMove = true;
    public bool PauseAnimImmune = false;
    public bool PauseMinionImmune = false;
    public bool IsMinion = false;

    private Transform attackTarget;

    public Dictionary<GameObject, Landmark> FoundLandmarks = new Dictionary<GameObject, Landmark>();

    protected virtual void Start()
    {
        player = PlayerManager.SharedInstance.Player;

        // Set the initial target to the player's transform
        SetAttackTarget(player.transform);

        InvokeRepeating("Attack", AttackRate, AttackRate);

        currentHealth = Health;

        // NavMesh initialization
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;

        // If it is a minion, start with immunity to pause since it is spawned during a cutscene
        // Also, return so that the name assignment lines are skipped
        if (IsMinion)
        {
            StartCoroutine(MakePauseAnimImmune(10f));
            StartCoroutine(MakePauseMinionImmune(10f));
            return;
        }

        NameText.text = EnemyName;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Follow the target
        agent.SetDestination(attackTarget.position);

        // Manage damage timers and resetting
        if (damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0f && MeshObject != null)
            {
                MeshObject.material.color = Color.white;
            }
        }

        // Check to see if the enemy died
        if (currentHealth <= 0f)
        {
            GameObject deathExplosion = (GameObject)Instantiate(DeathPrefab);
            deathExplosion.transform.position = transform.position;
            Destroy(gameObject);
        }

        // Only move if it can move
        if (canMove)
        {
            agent.speed = Speed;
        }
        if ((!canMove || GameplayManager.SharedInstance.Paused) && !PauseMinionImmune)
        {
            agent.speed = 0f;
        }

        // Pause animation if game is paused
        if (GameplayManager.SharedInstance.Paused && !PauseAnimImmune)
        {
            Anim.speed = 0f;
        }
        else
        {
            if (Anim.speed == 0f)
            {
                Anim.speed = 1f;
            }
        }

        // Minion doesn't do health stuff so back out if unneeded
        if (IsMinion) return;

        // Lerp the health bar nicely
        HealthSlider.value = Mathf.Lerp(HealthSlider.value, currentHealth / Health, Time.deltaTime * HealthLerpSpeed);

        // Aim the text at the main camera at all times
        TextParent.LookAt(2 * TextParent.position - Camera.main.transform.position);
               
    }

    public IEnumerator MakePauseAnimImmune(float duration)
    {
        PauseAnimImmune = true;

        yield return new WaitForSeconds(duration);

        PauseAnimImmune = false;
    }

    public IEnumerator MakePauseMinionImmune(float duration)
    {
        PauseMinionImmune = true;

        yield return new WaitForSeconds(duration);

        PauseMinionImmune = false;
    }

    public virtual void Attack()
    {
        if (GameplayManager.SharedInstance.Paused) return;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, attackTarget.position - transform.position, out hit, 1f))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                PlayerManager.SharedInstance.TakeDamage(AttackPower);
            }

            // Build up a dictionary of landmarks the enemy has encountered so we don't have to GetComponent every attack
            if (hit.transform.gameObject.tag == "Landmark")
            {
                if (!FoundLandmarks.ContainsKey(hit.transform.gameObject))
                {
                    FoundLandmarks[hit.transform.gameObject] = hit.transform.gameObject.GetComponent<Landmark>();
                }

                bool isLandmarkStillAlive = FoundLandmarks[hit.transform.gameObject].TakeDamage(AttackPower);
                if (!isLandmarkStillAlive)
                {
                    SetAttackTarget(player.transform);
                }
            }
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            WeaponController_Base weapon = other.gameObject.GetComponent<WeaponController_Base>();
            TakeDamage(weapon.Damage);
        }
    }

    protected void TakeDamage(float amount)
    {
        currentHealth -= amount;
        GameObject damageExplosion = (GameObject)Instantiate(DamagePrefab);
        damageExplosion.transform.position = transform.position;
        if (MeshObject != null)
        {
            MeshObject.material.color = Color.red;
        }
        damageTimer = DamageDisplayDuration;
    }

    public void SetAttackTarget(Transform newTarget)
    {
        if (newTarget)
        {
            attackTarget = newTarget;
        }
    }
}
