using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> roomEnemies = new List<GameObject>();
    public List<GameObject> roomDoors = new List<GameObject>();

    public int doorCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        roomEnemies.RemoveAll(item => item == null);

        if (roomEnemies.Count <= roomDoors.Count)
        {
            roomEnemies.ForEach(item => item.GetComponent<EnemyLister>().DropKey());
        }
    }

    public void AddEnemy(GameObject enemy)
    {
        roomEnemies.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        roomEnemies.Remove(enemy);
    }

    public void AddDoor(GameObject door)
    {
        roomDoors.Add(door);
        doorCount = roomDoors.Count;
    }
    public void RemoveDoor(GameObject door)
    {
        roomDoors.Remove(door);
    }
}
