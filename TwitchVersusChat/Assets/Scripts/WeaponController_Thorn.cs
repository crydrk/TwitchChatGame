using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController_Thorn : WeaponController_Base
{
    public float TurnSpeed = 1f;
    private Transform target;

    protected override void OnEnable()
    {
        deathTimer = Lifetime;

        target = PlayerManager.SharedInstance.Player.transform;
    }

    protected override void Update()
    {
        base.Update();

        if (!GameplayManager.SharedInstance.Paused)
        {
            Vector3 targetDirection = target.position - transform.position;
            float singleStep = TurnSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);

            transform.Translate(Vector3.forward * LaunchSpeed * Time.deltaTime);
        }
        
    }

    void OnTriggerEnter(Collider collider)
    {
        int groundLM = LayerMask.NameToLayer("Ground");
        int enemyLM = LayerMask.NameToLayer("Player");

        if (collider.gameObject.layer == groundLM || collider.gameObject.layer == enemyLM)
        {
            SelfDestruct();
        }
    }
}
