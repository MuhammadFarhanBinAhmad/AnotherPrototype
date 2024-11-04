using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public enum MODE
    {
        PATROL,
        ATTACKING
    }

    EnemyRangeAttackBehaviour rangeAttackBehaviour;
    EnemyMeleeAttackBehaviour meleeAttackBehaviour;

    NavMeshAgent m_Agent;
    public MODE p_Mode;

    [Header("MovementStats")]
    public float s_MovementSpeed;
    public Animator animator;

    [Header("DetectionZone")]
    public Transform coneTip;  // The position of the tip of the cone (vertex)
    public float coneAngle;  // The angle at the tip of the cone (in degrees)
    public float coneLength;  // The length of the cone
    public float m_PatrolRadius;
    public Transform m_CenterPoint;

    public Transform m_Target;
    private AudioSource audioSource;
    public bool canDetect = true;
    public AudioClip detectedPlayer;

    [Header("Effects")]
    public bool isStunned;
    public float e_StunTime;
    public float e_StunTimeLeft;
    public bool isFrozen;
    public float e_FreezeTime;
    public float e_FreezeTimeLeft;


    private void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        rangeAttackBehaviour = GetComponent<EnemyRangeAttackBehaviour>();
        meleeAttackBehaviour = GetComponent<EnemyMeleeAttackBehaviour>();

        m_Agent.speed = s_MovementSpeed;
        //m_CenterPoint = gameObject.transform.parent; // there might be multiple centerpoints in 1 room
        if (m_CenterPoint == null)
        {
            m_CenterPoint = GameObject.FindGameObjectWithTag("Player").transform;
        }

        foreach(Animator i in GetComponentsInChildren<Animator>())
        {
            if(i.gameObject.tag == "Model") {
                animator = i;
            }
        }
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        DetectionCone();
        switch (p_Mode)
        {
            case MODE.PATROL:
                {
                    if (m_Agent.remainingDistance <= m_Agent.stoppingDistance && s_MovementSpeed != 0)
                    {
                        Vector3 point;
                        if (RandomPoint(m_CenterPoint.position, m_PatrolRadius, out point))
                        {
                            m_Agent.SetDestination(point);
                        }
                    }

                    if (meleeAttackBehaviour != null)
                    {
                        meleeAttackBehaviour.isAttacking = false;
                    }
                    else if (rangeAttackBehaviour != null)
                    {
                        rangeAttackBehaviour.isAttacking = false;
                    }
                    break;
                }
                case MODE.ATTACKING:
                {
                    if (!isStunned)
                    {
                        //m_Agent.ResetPath();
                        if (meleeAttackBehaviour != null)
                        {
                            meleeAttackBehaviour.isAttacking = true;
                        }
                        else if (rangeAttackBehaviour != null)
                        {
                            rangeAttackBehaviour.isAttacking = true;
                        }
                    }
                    break;
                }
        }
        if (isStunned)
        {
            m_Agent.speed = 0;
            CountStunTimer();
        }
        if (isFrozen)
        {
            m_Agent.speed = s_MovementSpeed / 2;
            CountFreezeTimer();
        }
        if (animator != null)
        {
            animator.SetFloat("Remaining Distance", m_Agent.remainingDistance);
            animator.SetFloat("Speed", m_Agent.speed);
        }
    }
    public void StunEnemy()
    {
        isStunned = true;
        e_StunTimeLeft = e_StunTime;
        //m_Agent.speed = 0;
    }
    void CountStunTimer()
    {
        if (e_StunTimeLeft > 0)
        {
            e_StunTimeLeft -= Time.deltaTime;
        }
        else
        {
            isStunned = false;
            m_Agent.speed = s_MovementSpeed;
        }
    }
    public void FreezeEnemy()
    {
        isFrozen = true;
        e_FreezeTimeLeft = e_FreezeTime;
        //m_Agent.speed = s_MovementSpeed / 2;
    }
    void CountFreezeTimer()
    {
        if (e_FreezeTimeLeft > 0)
        {
            e_FreezeTimeLeft -= Time.deltaTime;
        }
        else
        {
            isFrozen = false;
            m_Agent.speed = s_MovementSpeed;
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }
    void DetectionCone()
    {
        Collider[] colliders = Physics.OverlapSphere(coneTip.position, coneLength);
        foreach (Collider collider in colliders)
        {
            Vector3 directionToCollider = (collider.transform.position - coneTip.position).normalized;
            float angleToCollider = Vector3.Angle(transform.forward, directionToCollider);

            if (angleToCollider < coneAngle / 2)
            {
                float distanceToCollider = Vector3.Distance(coneTip.position, collider.transform.position);
                if (distanceToCollider <= coneLength)
                {
                    if (collider.GetComponent<PlayerHealth>() != null)
                    {
                        Vector3 playerpos = collider.transform.position;
                        float dist = Vector3.Distance(playerpos, transform.position);
                        transform.LookAt(collider.transform);
                        p_Mode = MODE.ATTACKING;
                        m_Target = collider.transform ;
                        if (canDetect == true && detectedPlayer != null)
                        {
                            canDetect = false;
                            audioSource.PlayOneShot(detectedPlayer);
                        }
                    }
                }
                else
                {
                    StartCoroutine(MustHaveBeenTheWind(collider));
                }
            }
        }
    }

    IEnumerator MustHaveBeenTheWind(Collider collider)
    {
        yield return new WaitForSeconds(6);
        p_Mode = MODE.PATROL;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 forward = transform.forward * coneLength;
        Gizmos.DrawRay(coneTip.position, forward);

        // Draw the outline of the cone
        Vector3 right = Quaternion.Euler(0, coneAngle / 2, 0) * forward;
        Gizmos.DrawRay(coneTip.position, right);

        Vector3 left = Quaternion.Euler(0, -coneAngle / 2, 0) * forward;
        Gizmos.DrawRay(coneTip.position, left);
    }
}

