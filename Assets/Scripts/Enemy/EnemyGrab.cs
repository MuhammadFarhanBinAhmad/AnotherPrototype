using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGrab : MonoBehaviour
{
    private Rigidbody rb;
    private Transform objectGrabPoint;
    private float lerpSpeed = 10f;

    private Transform cameraTransform;
    public GameObject player;
    public PlayerFist playerFist;

    private bool isGrabbing = false;
    public GameObject enemyYeeter;
    public bool isThrow = false;
    public bool canGrab = false;
    public EnemyForceDetector enemyForceDetector;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        //Ka Long this one i settle with you
        // sorry bossku i was notty :(

        player = GameObject.FindGameObjectWithTag("Player");
        playerFist = GameObject.FindGameObjectWithTag("LeftFist").GetComponent<PlayerFist>();

        enemyYeeter = GameObject.FindGameObjectWithTag("EnemyYeeter");
        enemyForceDetector = gameObject.GetComponent<EnemyForceDetector>();
    }

    public void ToggleAI()
    {
        if (gameObject.GetComponent<EnemyMovement>().enabled == true)
        {
            gameObject.GetComponent<EnemyMovement>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<EnemyMovement>().enabled = true;
        }

        if (gameObject.GetComponent<NavMeshAgent>().enabled == true)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
        }

        if (gameObject.GetComponent<Rigidbody>().isKinematic == true)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void Grab(Transform objectGrabPoint)
    {
        ToggleAI();
        enemyForceDetector.CheckElement();
        this.objectGrabPoint = objectGrabPoint;
        rb.useGravity = false;
        playerFist.anim.SetTrigger("Grab");
    }

    public void Throw(float throwForce)
    {
        enemyYeeter.transform.forward = player.transform.forward;
        enemyYeeter.GetComponent<Rigidbody>().velocity = transform.forward * throwForce;
        isThrow = true;

        this.objectGrabPoint = null;
        rb.useGravity = true;
        
        Vector3 dir = (transform.position - player.transform.position).normalized;
        StartCoroutine(Delay());
    }

    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);
        ToggleAI();
        isThrow = false;
    }

    private void Update()
    {
        if (objectGrabPoint != null) // something is being grabbed
        {
            rb.MovePosition(objectGrabPoint.position);
            transform.forward = player.transform.forward;
            enemyYeeter.GetComponent<Rigidbody>().MovePosition(objectGrabPoint.position);
            enemyYeeter.transform.forward = player.transform.forward;
        }

        if (isThrow == true)
        {
            rb.MovePosition(enemyYeeter.transform.position);
        }
    }

        //public void OnTriggerEnter(Collider other)
        //{
        //    if (other.GetComponent<EnemyMovement>() != null && isFlying == true) // if prop hits enemy in mid air
        //    {
        //        if (gameObject.GetComponent<ExplosivePropObjects>() != null) // this prop goes boom
        //        {
        //            gameObject.GetComponent<ExplosivePropObjects>().Explode();
        //            Debug.Log("Splode");
        //            isFlying = false;
        //            gameObject.GetComponent<Collider>().isTrigger = false;
        //        }
        //        else // this prop goes bonk
        //        {
        //            other.GetComponent<EnemyMovement>().StunEnemy();
        //            Debug.Log("Stun");
        //            isFlying = false;
        //            gameObject.GetComponent<Collider>().isTrigger = false;
        //        }
        //    }

        //    gameObject.GetComponent<Collider>().isTrigger = false;

        //    if (other.tag == ("Wall"))
        //    {
        //        gameObject.GetComponent<Collider>().isTrigger = false;
        //        isFlying = false;
        //        gameObject.GetComponent<Collider>().isTrigger = false;
        //    }
        //}
}
