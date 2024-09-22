using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunMine : MonoBehaviour
{
    public GameObject stunEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyMovement>() != null)
        {
            other.GetComponent<EnemyMovement>().StunEnemy();

            var effect = Instantiate(stunEffect, gameObject.transform.position, gameObject.transform.rotation);
            effect.transform.parent = other.transform;

            Destroy(gameObject);
        }
    }
}
