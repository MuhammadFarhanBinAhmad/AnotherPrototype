using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttackBehaviour : MonoBehaviour
{
    public WeaponType e_weaponType;

    public float e_RateOfAttack;
    float nexttime_ToFire;

    [Header("ExplosiveEnemy")]
    public bool ExplosiveEnemy;
    public GameObject Explosion;

    [Header("MeleeEnemy")]
    Animator animator;
    bool Attacking;

    public bool isShocked;
    public float e_ShockTime;
    public float e_ShockTimeLeft;
    private float e_ShockMultiplier = 1f;


    private void Start()
    {
        if (!ExplosiveEnemy)
            animator = GetComponent<Animator>();
    }
    public void AttackPlayer()
    {
        if (isShocked == false)
        {
            if (ExplosiveEnemy)
            {
                Instantiate(Explosion, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else
            {
                if (Time.time >= nexttime_ToFire)
                {
                    nexttime_ToFire = Time.time + 1f / e_RateOfAttack;
                    animator.SetTrigger("Attacking");
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
