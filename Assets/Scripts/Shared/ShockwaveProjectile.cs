using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveProjectile : MonoBehaviour
{
    Rigidbody m_RigidBody;
    public float p_Speed;
    public int p_Damage;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Instantiate(HitShotEffect, transform.position, transform.rotation);
        }

        if (other.GetComponent<EnemyHealth>() != null)
        {
            Instantiate(HitShotEffect, transform.position, transform.rotation);
            other.GetComponent<EnemyHealth>().TakeDamage(p_Damage);
        }

        if (other.GetComponent<PropObjects>() != null)
        {
            Instantiate(HitShotEffect, transform.position, transform.rotation);
            other.GetComponent<PropObjects>().TakeDamage(p_Damage);
        }
    }
    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
