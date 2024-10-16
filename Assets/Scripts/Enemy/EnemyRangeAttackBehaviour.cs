using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
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
    public Transform retreatPoint;
    public bool isAttacking = false;
    public GameObject roomLeader;
    private float movementSpeed;


    public WeaponType e_weaponType;

    public GameObject e_Projectile;
    public Transform e_SpawnPos;
    public float e_RateOfAttack;
    float nexttime_ToFire;

    public float shootDurationMax; // time enemy shoots for
    private float shootDurationCurrent;
    public float breakDuration; // time of each break
    private bool canBreak = true;

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
        movementSpeed = m_Agent.speed;
        roomManager = gameObject.transform.parent.gameObject.GetComponent<RoomManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        retreatPoint = player.transform.Find("EnemyRetreatPoint").transform;

        shootDurationCurrent = shootDurationMax;
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
                    m_Agent.speed = movementSpeed;
                    m_Agent.SetDestination(player.position);
                    AttackPlayer();
                    break;
                }
            case MODE.DISENGAGE:
                {
                    m_Agent.speed = movementSpeed * 1.5f;
                    m_Agent.SetDestination(retreatPoint.position);
                    break;
                }
        }
        if (isAttacking == true)
        {
            if (roomLeader == null)
            {
                p_Mode = MODE.DISENGAGE;
                StartCoroutine(DisengageTimer());
            }
            else
            {
                p_Mode = MODE.ENGAGE;
            }
        }
        if (shootDurationCurrent <= 0 && canBreak == true)
        {
            canBreak = false;
            StartCoroutine(ShootBreak());
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
        shootDurationCurrent -= Time.deltaTime;

        m_Agent.SetDestination(player.position);
        if (isShocked == false && isLobotomised == false)
        {
            if (Time.time >= nexttime_ToFire && shootDurationCurrent >= 0)
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

    public IEnumerator ShootBreak()
    {
        yield return new WaitForSeconds(breakDuration);
        shootDurationCurrent = shootDurationMax;
        canBreak = true;
    }

    public IEnumerator DisengageTimer()
    {
        yield return new WaitForSeconds(1);
        p_Mode = MODE.ENGAGE;
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
