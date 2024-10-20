using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> roomEnemies = new List<GameObject>();
    public List<GameObject> roomDoors = new List<GameObject>();

    public int doorCount;

    public int leaderCount = 0;
    public Transform player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        roomEnemies.RemoveAll(item => item == null);

        if (roomEnemies.Count <= roomDoors.Count)
        {
            roomEnemies.ForEach(item => item.GetComponent<EnemyLister>().DropKey());
        }

        foreach (GameObject enemy in roomEnemies)
        {
            if (enemy.gameObject.GetComponent<EnemyMeleeAttackBehaviour>() != null)
            {
                if (leaderCount == 0)
                {
                    enemy.GetComponent<EnemyMeleeAttackBehaviour>().isLeader = true; // assign a new leader
                    foreach (GameObject enemyy in roomEnemies)
                    {
                        if (enemyy.GetComponent<EnemyMeleeAttackBehaviour>() != null) // tell all enemies who is the leader
                        {
                            enemyy.GetComponent<EnemyMeleeAttackBehaviour>().roomLeader = enemy;
                        }
                        if (enemyy.GetComponent<EnemyRangeAttackBehaviour>() != null) // tell all enemies who is the leader
                        {
                            enemyy.GetComponent<EnemyRangeAttackBehaviour>().roomLeader = enemy;
                        }
                    }
                    leaderCount += 1;
                }
            }
        }
    }

    public void AddEnemy(GameObject enemy)
    {
        roomEnemies.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy) //NOT USED
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

    public void UnlockEnemyAI()
    {
        foreach (GameObject enemy in roomEnemies)
        {
            enemy.GetComponent<EnemyMovement>().enabled = true;
            enemy.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
