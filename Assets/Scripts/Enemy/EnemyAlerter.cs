using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlerter : MonoBehaviour
{
    public RoomManager roomManager;

    // Start is called before the first frame update
    void Start()
    {
        roomManager = gameObject.transform.parent.gameObject.GetComponent<RoomManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<KickDoor>() != null)
        {
            roomManager.UnlockEnemyAI();
        }
    }
}
