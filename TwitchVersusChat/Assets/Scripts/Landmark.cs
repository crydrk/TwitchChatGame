using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LandmarkType
{
    None,
    Pyramid,
    Tomb,
    Crystal,
    Egg
}

public class Landmark : MonoBehaviour
{
    public LandmarkType TypeOfLandmark;
    public List<GameObject> DamageStages = new List<GameObject>();
    public float MaxHealth = 100f;

    private int damageStage;
    private float health;
    public bool IsDestroyed = false;

    public MeshRenderer MeshObject;
    public float DamageDisplayDuration = 0.25f;
    private float damageTimer;

    void Start()
    {
        damageStage = DamageStages.Count;
        health = MaxHealth;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(5f);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(-5f);
        }

        // Manage damage timers and resetting
        if (damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0f && MeshObject != null)
            {
                MeshObject.sharedMaterial.color = Color.white;
            }
        }
    }

    public bool TakeDamage(float value)
    {
        // If landmark is ready to be damaged, damage it
        // Restricted to DamageDisplayDuration to prevent swarming of attacks
        // TODO: This is sort of unfair to the viewers, since as it stands, it wouldn't be beneficial
        // to send multiple bad guys to attack. But I also don't want it to be hard on the player.
        if (damageTimer <= 0f)
        {
            health -= value;

            if (MeshObject != null)
            {
                MeshObject.sharedMaterial.color = Color.red;
            }
            damageTimer = DamageDisplayDuration;
        }

        // Make landmark no longer attackable if it is destroyed fully
        if (health <= 0f)
        {
            DestroyLandmark();
        }

        // Update the display for damage on the landmark
        if (!IsDestroyed)
        {
            SetDamageStage();
        }
        else
        {
            // Tell the attacking monster that this target is no longer valid
            return false;
        }

        return true;

        
    }

    public void SetDamageStage()
    {
        if (health > MaxHealth * 0.9f)
        {
            damageStage = 0;
            HideAllDamageStagesExcept(damageStage);
            return;
        }
        if (health > MaxHealth * 0.75f)
        {
            damageStage = 1;
            HideAllDamageStagesExcept(damageStage);
            return;
        }
        if (health > MaxHealth * 0.5f)
        {
            damageStage = 2;
            HideAllDamageStagesExcept(damageStage);
            return;
        }
        if (health > MaxHealth * 0.25f)
        {
            damageStage = 3;
            HideAllDamageStagesExcept(damageStage);
            return;
        }
        if (health > MaxHealth * 0.1f)
        {
            damageStage = 4;
            HideAllDamageStagesExcept(damageStage);
            return;
        }
        if (health <= 0f)
        {
            damageStage = 5;
            HideAllDamageStagesExcept(damageStage);
            return;
        }
    }

    private void HideAllDamageStagesExcept(int exempt)
    {
        int count = 0;
        foreach (GameObject stage in DamageStages)
        {
            if (count != exempt)
            {
                stage.SetActive(false);
            }
            else
            {
                stage.SetActive(true);
            }
            count += 1;
        }
    }

    public void DestroyLandmark()
    {
        IsDestroyed = true;
    }
}
