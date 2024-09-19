using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFistPickup : MonoBehaviour
{
    public bool canBigFist = false;

    private void OnTriggerEnter(Collider other)
    {
        canBigFist = true;

        if (other.GetComponent<PlayerSkills>() != null)
        {
            other.GetComponent<PlayerSkills>().BigFist(canBigFist);
            Destroy(transform.parent.gameObject);
        }
    }
}
