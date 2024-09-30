using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLister : MonoBehaviour
{
    public RoomManager roomManager;

    // Start is called before the first frame update
    void Start()
    {
        roomManager = gameObject.transform.parent.gameObject.GetComponent<RoomManager>();

        if (roomManager != null)
        {
            roomManager.AddDoor(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
