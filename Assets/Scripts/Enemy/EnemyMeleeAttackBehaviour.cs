using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemyMeleeAttackBehaviour : MonoBehaviour
{
    public enum MODE
    {
        PASSIVE,
        ENGAGE,
        FLANK,
        DISENGAGE
    }

    public RoomManager roomManager;
    public NavMeshAgent m_Agent;
    public MODE p_Mode;
    public Transform player;
    public bool isAttacking = false;
    public bool isLeader = false;
    public GameObject roomLeader;
    private Vector3 flankVector;
    private float flankAngle;
    private float defaultSpeed;
    public EnemyMovement EnemyMovement;

    public Transform flankPoint;
    private bool canSetFlank = true;
    private Transform flankPoint1;
    private Transform flankPoint2;
    private Transform flankPoint3;
    private Transform flankPoint4;
    private Transform retreatPoint;


    public WeaponType e_weaponType;

    public float e_RateOfAttack;
    float nexttime_ToFire;

    [Header("ExplosiveEnemy")]
    public bool ExplosiveEnemy;
    public GameObject Explosion;

    [Header("MeleeEnemy")]
    [SerializeField] Animator animator;
    [SerializeField] bool Attacking;

    public bool isShocked;
    public float e_ShockTime;
    public float e_ShockTimeLeft;
    private float e_ShockMultiplier = 1f;

    public bool isLobotomised = false;


    private void Start()
    {
        if (!ExplosiveEnemy)
            animator = GetComponent<Animator>();

        if (animator == null) {
            animator = GetComponentInChildren<Animator>();
        }

        m_Agent = GetComponent<NavMeshAgent>();
        EnemyMovement = GetComponent<EnemyMovement>();
        defaultSpeed = m_Agent.speed;
        if (gameObject.transform.parent != null)
        {
            roomManager = gameObject.transform.parent.gameObject.GetComponent<RoomManager>();
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;
        AssignPoints();
    }

    public void AssignPoints()
    {
        flankPoint1 = player.transform.Find("EnemyWaypoints/EnemyFlankPoint1");
        flankPoint2 = player.transform.Find("EnemyWaypoints/EnemyFlankPoint2");
        flankPoint3 = player.transform.Find("EnemyWaypoints/EnemyFlankPoint3");
        flankPoint4 = player.transform.Find("EnemyWaypoints/EnemyFlankPoint4");
        retreatPoint = player.transform.Find("EnemyWaypoints/EnemyRetreatPoint");
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
                        transform.LookAt(player.position);
                        m_Agent.speed = defaultSpeed * 2.5f;
                        m_Agent.SetDestination(player.position);
                        AttackPlayer();
                        break;
                    }
                case MODE.FLANK:
                    {
                        transform.LookAt(player.position);
                        m_Agent.speed = defaultSpeed * 1.5f;
                        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
                        if (distance <= 4)
                        {
                            m_Agent.SetDestination(retreatPoint.position);
                        }
                        else
                        {
                            if (canSetFlank == true)
                            {
                                CalculateFlankAngle();
                            }
                            //Quaternion myRotation = Quaternion.AngleAxis(flankAngle, Vector3.forward);
                            //Vector3 playerPos = player.transform.position;
                            //flankVector = myRotation * playerPos;
                            //m_Agent.SetDestination(flankVector);
                            m_Agent.SetDestination(flankPoint.position);
                            if (m_Agent.remainingDistance <= m_Agent.stoppingDistance * 2) // find new flank
                            {
                                canSetFlank = true;
                            }
                        }

                        AttackPlayer();
                        break;
                    }
                case MODE.DISENGAGE:
                    {
                        transform.LookAt(player.position);
                        m_Agent.speed = defaultSpeed * 1.5f;
                        m_Agent.SetDestination(retreatPoint.position);
                        AttackPlayer();
                        break;
                    }
            }
        }

        if (isAttacking == true && isLobotomised == false)
        {
            if (roomManager != null && roomManager.roomEnemies.Count == 1)
            {
                p_Mode = MODE.DISENGAGE;
                StartCoroutine(DisengageTimer());
            }
            else if (roomManager == null || isLeader == true)
            {
                p_Mode = MODE.ENGAGE;
            }
            else
            {
                p_Mode = MODE.FLANK;
            }
        }

        if (isShocked)
        {
            CountShockTimer();
        }
    }

    public void AttackPlayer()
    {
        if (m_Agent.remainingDistance <= m_Agent.stoppingDistance)
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
                        animator.SetTrigger("Attacking");
                        nexttime_ToFire = Time.time + 1f / e_RateOfAttack;
                    }
                }
            }
        }
    }

    public IEnumerator DisengageTimer()
    {
        yield return new WaitForSeconds(2);
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

    public void CalculateFlankAngle()
    {
        int angleSelection = Random.Range(1, 5);

        switch (angleSelection)
        {
            case 1:
                flankAngle = 25f;
                flankPoint = flankPoint1;
                break;
            case 2:
                flankAngle = 35f;
                flankPoint = flankPoint2;
                break;
            case 3:
                flankAngle = -25f;
                flankPoint = flankPoint3;
                break;
            case 4:
                flankAngle = -35f;
                flankPoint = flankPoint4;
                break;
            default:
                flankAngle = 0f;
                break;
        }

        canSetFlank = false;
        StartCoroutine(ResetFlank());
    }
    public IEnumerator ResetFlank()
    {
        yield return new WaitForSeconds(8);
        canSetFlank = true;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(flankVector, 0.5f);
    }
}
