using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public PlayerSkills playerSkills;

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
            Destroy(transform.parent.gameObject);
        }
    }
}
