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

        transform.Translate(Vector3.forward * LaunchSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == 8)
        {
            SelfDestruct();
        }
    }
}
