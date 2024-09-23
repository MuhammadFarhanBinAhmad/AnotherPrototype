using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    public float speed;
    public GameObject player;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.GetComponent<PlayerHealth>() !=null)
    //    {
    //        transform.position = Vector3.MoveTowards(transform.position, other.transform.position, speed * Time.deltaTime);
    //    }
    //}
}
