using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    Rigidbody m_RigidBody;
    public float p_Speed;
    public int p_Damage;
    public bool p_IsPiercing;

    public string p_ElementType;
    public float p_ElementStack;

    public float p_TimeBeforeSelfDestruct;

    public GameObject HitShotEffect;

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        Invoke("DestroySelf", p_TimeBeforeSelfDestruct);
    }

    private void FixedUpdate()
    {
        m_RigidBody.velocity =  transform.forward * p_Speed;
    }

    public void SetProjectileStats(float speed, int damage, float destroyTime, bool piercing)
    {
        p_Speed = speed;
        p_Damage = damage;
        p_IsPiercing = piercing;
        p_TimeBeforeSelfDestruct = destroyTime;
    }

    public void SetProjectileElements(string elementType, float elementStackOnHit)
    {
        p_ElementType = elementType;
        p_ElementStack = elementStackOnHit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            if (other.GetComponent<KickDoor>() != null) // if hit door
            {
                if (other.GetComponent<KickDoor>().isKick != true) // if door has NOT been kicked down
                {
                    GameObject hitShot = Instantiate(HitShotEffect, transform.position, transform.rotation);
                    hitShot.gameObject.GetComponent<HitAudio>().PlayHitTerrainSound();
                    Destroy(gameObject);
                }
            }
            else
            {
                GameObject hitShot2 = Instantiate(HitShotEffect, transform.position, transform.rotation);
                hitShot2.gameObject.GetComponent<HitAudio>().PlayHitTerrainSound();
                Destroy(gameObject);
            }
        }

        if (other.GetComponent<EnemyStatus>() != null)
        {
            other.GetComponent<EnemyStatus>().TakeStacks(p_ElementStack);
            other.GetComponent<EnemyStatus>().AssignElement(p_ElementType);
        }

        if (other.GetComponent<EnemyHealth>() != null)
        {
            GameObject hitShot = Instantiate(HitShotEffect, transform.position, transform.rotation);
            other.GetComponent<EnemyHealth>().TakeDamage(p_Damage);
            hitShot.gameObject.GetComponent<HitAudio>().PlayHitEnemySound();
            if (p_IsPiercing == false)
            {
                Destroy(gameObject);
            }
        }

        if (other.GetComponent<PropObjects>() != null)
        {
            GameObject hitShot = Instantiate(HitShotEffect, transform.position, transform.rotation);
            other.GetComponent<PropObjects>().TakeDamage(p_Damage);
            hitShot.gameObject.GetComponent<HitAudio>().PlayHitTerrainSound();
            Destroy(gameObject);
        }
    }


    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
