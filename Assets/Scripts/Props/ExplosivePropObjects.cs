using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosivePropObjects : MonoBehaviour
{
    public int health;
    public GameObject Explosion;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerProjectile>() !=null)
        {
            Explode();
        }
    }

    public void Explode()
    {
        Instantiate(Explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
