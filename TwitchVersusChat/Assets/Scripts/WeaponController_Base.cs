using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController_Base : MonoBehaviour
{

    public float LaunchSpeed = 5f;
    public float Lifetime = 5f;
    public GameObject DeathPrefab;
    public float Damage = 10f;
    //private Rigidbody RBody;

    protected float deathTimer;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        //RBody = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable()
    {
        //RBody.AddForce(transform.forward * LaunchSpeed, ForceMode.Impulse);

        deathTimer = Lifetime;
    }

    protected virtual void SelfDestruct()
    {
        GameObject explosion = (GameObject)Instantiate(DeathPrefab);
        explosion.transform.position = transform.position;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        deathTimer -= Time.deltaTime;

        if (deathTimer <= 0f)
        {
            SelfDestruct();
        }
    }
}
