using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController_Fireball : WeaponController_Base
{

    protected override void OnEnable()
    {
        deathTimer = Lifetime;
    }

    protected override void Update()
    {
        base.Update();

        if (!GameplayManager.SharedInstance.Paused)
        {
            transform.Translate(Vector3.forward * LaunchSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        int groundLM = LayerMask.NameToLayer("Ground");
        int enemyLM = LayerMask.NameToLayer("Enemy");

        if (collider.gameObject.layer == groundLM || collider.gameObject.layer == enemyLM)
        {
            SelfDestruct();
        }
    }
}
