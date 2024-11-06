using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCrosshairCollider : MonoBehaviour
{
    public GameObject grabCrosshair;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider collision)
    {
        if (collision.transform.GetComponent<PropGrab>() != null)
        {
            grabCrosshair.SetActive(true);
        }
        if (collision.transform.GetComponent<EnemyGrab>() != null)
        {
            if (collision.transform.GetComponent<EnemyGrab>().canGrab == true)
            {
                grabCrosshair.SetActive(true);
            }
            else
            {
                grabCrosshair.SetActive(false);
            }
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.transform.GetComponent<PropGrab>() != null)
        {
            grabCrosshair.SetActive(false);
        }
        if (collision.transform.GetComponent<EnemyGrab>() != null)
        {
            grabCrosshair.SetActive(false);
        }
    }
}
