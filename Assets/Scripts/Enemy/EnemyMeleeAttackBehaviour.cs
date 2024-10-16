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
    public Transform retreatPoint;
    public bool isAttacking = false;
    public bool isLeader = false;
    public GameObject roomLeader;
    private Vector3 flankVector;
    private float flankAngle;
    private float movementSpeed;


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


    private void Start()
    {
        if (!ExplosiveEnemy)
            animator = GetComponent<Animator>();

        if (animator == null) {
            animator = GetComponentInChildren<Animator>();
        }

        m_Agent = GetComponent<NavMeshAgent>();
        movementSpeed = m_Agent.speed;
        roomManager = gameObject.transform.parent.gameObject.GetComponent<RoomManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        retreatPoint = player.transform.Find("EnemyRetreatPoint").transform;
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
                    m_Agent.SetDestination(player.position);
                    AttackPlayer();
                    break;
                }
            case MODE.FLANK:
                {
                    CalculateFlankAngle();
                    Quaternion myRotation = Quaternion.AngleAxis(flankAngle, Vector3.forward);
                    Vector3 playerPos = player.transform.position;
                    flankVector = myRotation * playerPos;
                    m_Agent.SetDestination(flankVector);
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
            if (roomManager.roomEnemies.Count == 1)
            {
                p_Mode = MODE.DISENGAGE;
                StartCoroutine(DisengageTimer());
            }
            else if (isLeader == true)
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
                break;
            case 2:
                flankAngle = 35f;
                break;
            case 3:
                flankAngle = -25f;
                break;
            case 4:
                flankAngle = -35f;
                break;
            default:
                flankAngle = 0f;
                break;
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(flankVector, 0.5f);
    }
}
