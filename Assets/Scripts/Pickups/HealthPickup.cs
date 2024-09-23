using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public int healValue;
    public GameObject pickupAudio;

    public void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() != null)
        {
            if (playerHealth.p_Health < playerHealth.p_MaxHealth)
            {
                playerHealth.Heal(healValue);
                if (pickupAudio != null)
                {
                    Instantiate(pickupAudio, gameObject.transform.position, gameObject.transform.rotation);
                }
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
