using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager SharedInstance;
    public GameObject Player;

    public float StartingHealth;
    public float HealthLerpSpeed = 1f;

    public Slider HealthSlider;

    private float currentHealth;

    private void Awake()
    {
        SharedInstance = this;
    }
    
    void Start()
    {
        currentHealth = StartingHealth;
    }
    
    void Update()
    {
        // Lerp the health bar nicely
        HealthSlider.value = Mathf.Lerp(HealthSlider.value, currentHealth / StartingHealth, Time.deltaTime * HealthLerpSpeed);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
    }

    public void GainHealth(float amount)
    {

    }
}
