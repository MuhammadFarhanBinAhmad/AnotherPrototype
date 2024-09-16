using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFistCollider : MonoBehaviour
{
    public bool canBigFist = false;
    public int damage;
    public float knockbackForce;
    public GameObject HitShotEffect;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<EnemyHealth>() != null)
        {
            if (canBigFist == true)
            {
                other.GetComponent<EnemyHealth>().TakeDamage(damage);
                Instantiate(HitShotEffect, other.transform.position, other.transform.rotation);
                other.gameObject.transform.position = Vector3.MoveTowards(other.transform.position, player.transform.position, -knockbackForce);
                canBigFist = false;
            }
        }
    }
}
