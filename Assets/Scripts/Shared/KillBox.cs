using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    public GameManager gameManager;

    public void Start()
    {
        gameManager = gameObject.transform.parent.GetComponent<GameManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag == "Player")
        {
            gameManager.RestartScene();
        }

        if (other.tag != "EnemyYeeter")
        {
            Destroy(other.gameObject);
        }
    }
}
