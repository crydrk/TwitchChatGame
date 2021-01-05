using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBase
{
    public float FlexTime = 3f;
    public Transform ProjectileSource;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        canMove = false;
        Invoke("EndFlex", FlexTime);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void EndFlex()
    {
        canMove = true;
    }

    public override void Attack()
    {
        if (GameplayManager.SharedInstance.Paused) return;

        GameObject projectile = ObjectPooler.SharedInstance.GetPooledObject(1);
        if (projectile != null)
        {
            projectile.transform.position = ProjectileSource.position;
            ProjectileSource.LookAt(PlayerManager.SharedInstance.Player.transform.position);
            projectile.transform.rotation = ProjectileSource.rotation;
            projectile.SetActive(true);
        }
    }
}
