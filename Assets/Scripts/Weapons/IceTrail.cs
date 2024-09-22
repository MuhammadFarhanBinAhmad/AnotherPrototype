using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTrail : MonoBehaviour
{
    public GameObject freezeEffect;

    public void Start()
    {
        StartCoroutine(Activate());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyMovement>() != null)
        {
            other.GetComponent<EnemyMovement>().FreezeEnemy();

            var effect = Instantiate(freezeEffect, gameObject.transform.position, gameObject.transform.rotation);
            effect.transform.parent = other.transform;

            Destroy(gameObject);
        }
    }

    public IEnumerator Activate()
    {
        yield return new WaitForSeconds(0.25f);
        gameObject.GetComponent<Collider>().enabled = true;
    }
}
