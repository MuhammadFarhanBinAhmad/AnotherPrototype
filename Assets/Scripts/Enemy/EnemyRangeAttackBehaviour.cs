using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyRangeAttackBehaviour : MonoBehaviour
{
    public enum MODE
    {
        PASSIVE,
        ENGAGE,
        DISENGAGE
    }

    public RoomManager roomManager;
    public NavMeshAgent m_Agent;
    public MODE p_Mode;
    public Transform player;
    public bool isAttacking = false;
    private float movementSpeed;


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

    public bool isLobotomised = false;
    public GameObject shootTarget;


    private void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        roomManager = gameObject.transform.parent.gameObject.GetComponent<RoomManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        movementSpeed = m_Agent.speed;
    }

    public void Update()
    {
        switch (p_Mode)
        {
            case MODE.PASSIVE:
                {
                    m_Agent.speed = movementSpeed;
                    break;
                }
            case MODE.ENGAGE:
                {
                    m_Agent.speed = movementSpeed * 2.5f;
                    //m_Agent.SetDestination(player.position);
                    AttackPlayer();
                    break;
                }
                //case MODE.DISENGAGE:
                //    {
                //m_Agent.speed = m_Agent.speed / 2;
                //        if (!isStunned)
                //        {
                //            m_Agent.SetDestination(m_Target.position);
                //            if (m_Agent.remainingDistance <= m_Agent.stoppingDistance)
                //            {
                //                if (rangeAttackBehaviour != null)
                //                    rangeAttackBehaviour.AttackPlayer();

                //                if (meleeAttackBehaviour != null)
                //                    meleeAttackBehaviour.AttackPlayer();
                //            }
                //        }
                //        break;
                //    }
        }
        if (isAttacking == true)
        {
            AttackPlayer();
        }

        if (isShocked)
        {
            CountShockTimer();
        }

        if (isLobotomised == true)
        {
            gameObject.transform.LookAt(shootTarget.transform.position);
            if (Time.time >= nexttime_ToFire)
            {
                nexttime_ToFire = Time.time + 1f / e_weaponType.p_WeaponFireRate * e_ShockMultiplier;
                GameObject p = Instantiate(e_Projectile, e_SpawnPos.position, e_SpawnPos.rotation);
                e_Projectile.GetComponent<EnemyProjectile>().SetProjectileStats(e_ProjectileSpeed, e_ProjectileDamage);
            }
        }
    }

    public void AttackPlayer()
    {
        m_Agent.SetDestination(player.position);
        if (isShocked == false && isLobotomised == false)
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
