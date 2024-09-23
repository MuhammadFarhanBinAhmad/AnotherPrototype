using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockAura : MonoBehaviour
{
    public GameObject shockEffect;
    private bool canSpawnEffect = true;

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<EnemyHealth>() != null)
        {
            other.GetComponent<EnemyHealth>().DoShock();

            if (canSpawnEffect == true)
            {
                var effect = Instantiate(shockEffect, gameObject.transform.position, gameObject.transform.rotation);
                effect.transform.parent = other.transform;
                canSpawnEffect = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyHealth>() != null)
        {
            other.GetComponent<EnemyHealth>().DoShock();

            if (canSpawnEffect == true)
            {
                var effect = Instantiate(shockEffect, gameObject.transform.position, gameObject.transform.rotation);
                effect.transform.parent = other.transform;
                canSpawnEffect = false;
            }
        }
    }
}
