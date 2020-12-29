using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{
    public string Name;
    public float FireRate = 0.5f;
    public GameObject Projectile;
}

public class FireControl : MonoBehaviour
{
    public Weapon CurrentWeapon;
    public List<Weapon> TestWeapons = new List<Weapon>();
    public Transform WeaponSource;
    public Transform BombSource;

    private float weaponCooldown;

    private void Start()
    {
        RegisterWeapon(TestWeapons[0]);
    }

    void Update()
    {
        if (Input.GetAxis("Fire1") > 0f)
        {
            if (weaponCooldown <= 0.0f)
            {
                GameObject projectile = ObjectPooler.SharedInstance.GetPooledObject();
                if (projectile != null)
                {
                    projectile.transform.position = WeaponSource.position;
                    WeaponSource.LookAt(AimControl.SharedInstance.Reticle.transform.position);
                    projectile.transform.rotation = WeaponSource.rotation;
                    projectile.SetActive(true);
                }
                weaponCooldown = CurrentWeapon.FireRate;
            }
        }

        if (weaponCooldown > 0.0f)
        {
            weaponCooldown -= Time.deltaTime;
        }
    }

    private void RegisterWeapon(Weapon weapon)
    {
        CurrentWeapon = weapon;
        weaponCooldown = weapon.FireRate;
    }
}
