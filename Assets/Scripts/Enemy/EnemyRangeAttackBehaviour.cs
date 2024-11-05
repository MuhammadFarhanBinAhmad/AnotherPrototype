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
    private float defaultSpeed;
    [SerializeField] private EnemyMovement EnemyMovement;


    public WeaponType e_weaponType;

    public GameObject e_Projectile;
    public Transform e_SpawnPos;
    public float e_RateOfAttack;
    float nexttime_ToFire;

    public float shootDurationMax; // time enemy shoots for
    private float shootDurationCurrent;
    public float breakDuration; // time of each break
    private bool canBreak = true;
    private AudioSource audioSource;
    public AudioClip shoot;

    //public bool e_GrenadeEnemy;
    //public GameObject e_Grenade;
    //public float e_ThrowForce;

    public float e_ProjectileSpeed;
    public int e_ProjectileDamage;

    public bool isShocked;
    public float e_ShockTime;
    public float e_ShockTimeLeft;
    private float e_ShockMultiplier = 1f;

    public bool isLobotomised = false;
    public GameObject shootTarget;
    public Animator animator;


    private void Start()
    {
        ReassignMesh();
        m_Agent = GetComponent<NavMeshAgent>();
        EnemyMovement = GetComponent<EnemyMovement>();
        defaultSpeed = m_Agent.speed;
        if (gameObject.transform.parent != null)
        {
            roomManager = gameObject.transform.parent.gameObject.GetComponent<RoomManager>();
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;

        retreatPoint = player.transform.Find("EnemyWaypoints/EnemyRetreatPoint");

        shootDurationCurrent = shootDurationMax;
        audioSource = GetComponent<AudioSource>();
        animator = EnemyMovement.animator;
    }

    public void Update()
    {
        if (EnemyMovement.enabled == true)
        {
            switch (p_Mode)
            {
                case MODE.PASSIVE:
                    {
                        m_Agent.speed = defaultSpeed;
                        break;
                    }
                case MODE.ENGAGE:
                    {
                        animator.SetBool("Attacking", true);
                        transform.LookAt(player.position);
                        m_Agent.speed = defaultSpeed;
                        m_Agent.SetDestination(player.position);
                        AttackPlayer();
                        break;
                    }
                case MODE.DISENGAGE:
                    {
                        animator.SetBool("Attacking", false);
                        transform.LookAt(player.position);
                        m_Agent.speed = defaultSpeed * 1.5f;
                        m_Agent.SetDestination(retreatPoint.position);
                        break;
                    }
            }
        }

        if (isAttacking == true && isLobotomised == false)
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
        else
        {
            p_Mode = MODE.PASSIVE;
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

                GameObject p = Instantiate(e_Projectile, e_SpawnPos.position, e_SpawnPos.rotation);
                e_Projectile.GetComponent<EnemyProjectile>().SetProjectileStats(e_ProjectileSpeed, e_ProjectileDamage);
                audioSource.PlayOneShot(shoot);
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

    void ReassignMesh() {
        ArmMeshContainer amc = GameObject.FindObjectOfType<ArmMeshContainer>();
        MaterialStore materialReassign = null;
        switch(e_weaponType.p_ProjectileElement) {
            case "Freeze":
                materialReassign = amc.freeze;
                break;
            case "Burn":
                materialReassign = amc.burn;
                break;
            case "Stun":
                materialReassign = amc.stun;
                break;
            case "Shock":
                materialReassign = amc.shock;
                break;
            default:
                break;
        }
        if (materialReassign) {
            foreach(Transform i in transform) {
                if(i.tag == "Model") {
                    foreach(SkinnedMeshRenderer j in GetComponentsInChildren<SkinnedMeshRenderer>()) {
                        if (j.material.name == "arm_type (Instance)") {
                            j.material = materialReassign.arm;
                        }
                        if (j.material.name == "arm_type_joint (Instance)") {
                            j.material = materialReassign.armJoint;
                        }
                    }
                }
            }
        }
    }
}
