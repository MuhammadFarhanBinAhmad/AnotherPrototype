using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    Rigidbody m_RigidBody;
    public float p_Speed;
    public int p_Damage;
    public bool p_IsPiercing;

    public float p_TimeBeforeSelfDestruct;

    public GameObject HitShotEffect;

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        //GetComponent<AudioSource>().Play();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Instantiate(HitShotEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        if (other.GetComponent<EnemyHealth>() != null)
        {
            Instantiate(HitShotEffect, transform.position, transform.rotation);
            other.GetComponent<EnemyHealth>().TakeDamage(p_Damage);
            if (p_IsPiercing == false)
            {
                Destroy(gameObject);
            }
        }

        if (other.GetComponent<PropObjects>() != null)
        {
            Instantiate(HitShotEffect, transform.position, transform.rotation);
            other.GetComponent<PropObjects>().TakeDamage(p_Damage);
            Destroy(gameObject);
        }
    }
    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
