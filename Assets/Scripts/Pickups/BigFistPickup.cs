using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFistPickup : MonoBehaviour
{
    public bool canBigFist = false;
    public GameObject pickupAudio;

    private void OnTriggerEnter(Collider other)
    {
        canBigFist = true;

        if (other.GetComponent<PlayerSkills>() != null)
        {
            other.GetComponent<PlayerSkills>().BigFist(canBigFist);
            if (pickupAudio != null)
            {
                Instantiate(pickupAudio, gameObject.transform.position, gameObject.transform.rotation);
            }
            Destroy(transform.parent.gameObject);
        }
    }
}
