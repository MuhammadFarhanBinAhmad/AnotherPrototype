using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyRangeAttackBehaviour : MonoBehaviour
{
    public WeaponType e_weaponType;

    public GameObject e_Projectile;
    public Transform e_SpawnPos;
    public float e_RateOfAttack;
    float nexttime_ToFire;

    public bool e_GrenadeEnemy;
    public GameObject e_Grenade;
    public float e_ThrowForce;

    public float e_ProjectileSpeed;
    public int e_ProjectileDamage;

    public bool isShocked;
    public float e_ShockTime;
    public float e_ShockTimeLeft;
    private float e_ShockMultiplier = 1f;


    public void AttackPlayer()
    {
        if (isShocked == false)
        {
            if (Time.time >= nexttime_ToFire)
            {
                nexttime_ToFire = Time.time + 1f / e_weaponType.p_WeaponFireRate * e_ShockMultiplier;
                if (e_GrenadeEnemy)
                {
                    GameObject grenade = Instantiate(e_Grenade, e_SpawnPos.position, e_SpawnPos.rotation);

                    // Add force to the grenade
                    Rigidbody rb = grenade.GetComponent<Rigidbody>();
                    rb.AddForce((transform.forward * e_ThrowForce), ForceMode.VelocityChange);
                }
                else
                {
                    GameObject p = Instantiate(e_Projectile, e_SpawnPos.position, e_SpawnPos.rotation);
                    e_Projectile.GetComponent<EnemyProjectile>().SetProjectileStats(e_ProjectileSpeed, e_ProjectileDamage);
                }
            }
        }
    }

    public void Update()
    {
        if (isShocked)
        {
            CountShockTimer();
        }
    }

    public void ShockEnemy()
    {
        isShocked = true;
        e_ShockTimeLeft = e_ShockTime;
        e_ShockMultiplier = 0.25f;
    }
    void CountShockTimer()
    {
        if (e_ShockTimeLeft > 0)
        {
            e_ShockTimeLeft -= Time.deltaTime;
        }
        else
        {
            isShocked = false;
            e_ShockMultiplier = 1f;
        }
    }
}
