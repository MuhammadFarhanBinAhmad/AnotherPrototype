using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLister : MonoBehaviour
{
    public EnemyHealth enemyHealth;
    public RoomManager roomManager;
    public bool dropKey = false;
    public GameObject roomKey;

    public EnemyMeleeAttackBehaviour attackBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        roomManager = gameObject.transform.parent.gameObject.GetComponent<RoomManager>();
        enemyHealth = GetComponent<EnemyHealth>();

        if (roomManager != null)
        {
            roomManager.AddEnemy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dropKey == true && enemyHealth.health <= 0)
        {
            //Instantiate(roomKey, gameObject.transform.position, gameObject.transform.rotation);
        }
    }

    public void DropKey()
    {
        enemyHealth.dropKey = true;
        //dropKey = true;
    }
}
