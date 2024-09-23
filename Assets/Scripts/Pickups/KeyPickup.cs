using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public PlayerSkills playerSkills;
    public GameObject pickupAudio;

    // Start is called before the first frame update
    void Start()
    {
        playerSkills = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSkills>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerSkills>() != null)
        {
            playerSkills.AddKey(1);
            if (pickupAudio != null)
            {
                Instantiate(pickupAudio, gameObject.transform.position, gameObject.transform.rotation);
            }
            Destroy(transform.parent.gameObject);
        }
    }
}
