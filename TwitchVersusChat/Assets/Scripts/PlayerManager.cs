using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager SharedInstance;
    public GameObject Player;
    public SkinnedMeshRenderer MeshObject;

    public float StartingHealth;
    public float HealthLerpSpeed = 1f;
    public float DamageDisplayDuration = 0.25f;
    private float damageTimer;
    private Rigidbody rBody;

    public Slider HealthSlider;

    private float currentHealth;

    private void Awake()
    {
        SharedInstance = this;

        rBody = GetComponent<Rigidbody>();
    }
    
    void Start()
    {
        currentHealth = StartingHealth;
    }
    
    void Update()
    {
        // Lerp the health bar nicely
        HealthSlider.value = Mathf.Lerp(HealthSlider.value, currentHealth / StartingHealth, Time.deltaTime * HealthLerpSpeed);

        if (damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0f)
            {
                MeshObject.material.color = Color.white;
            }
        }

        // Stop movement if paused
        if (GameplayManager.SharedInstance.Paused && rBody.isKinematic == false)
        {
            rBody.isKinematic = true;
        }
        else if (!GameplayManager.SharedInstance.Paused && rBody.isKinematic == true)
        {
            rBody.isKinematic = false;
        }
    }

    public void TakeDamage(float amount)
    {
        // Take damage, but only if it hasn't been damaged in DamageDisplayDuration
        if (damageTimer <= 0f)
        {
            currentHealth -= amount;
            MeshObject.material.color = Color.red;
            damageTimer = DamageDisplayDuration;
            rBody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
        
    }

    public void GainHealth(float amount)
    {
        currentHealth += amount;
        MeshObject.material.color = Color.green;
        damageTimer = DamageDisplayDuration;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ProjectileBad")
        {
            WeaponController_Base w = other.GetComponent<WeaponController_Base>();
            TakeDamage(w.Damage);
        }
    }
}
