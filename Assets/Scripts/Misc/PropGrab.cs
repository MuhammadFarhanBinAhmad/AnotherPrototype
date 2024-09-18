using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropGrab : MonoBehaviour
{
    private Rigidbody rb;
    private Transform objectGrabPoint;
    private float lerpSpeed = 10f;

    private Transform cameraTransform;
    public GameObject player;
    public PlayerFist playerFist;

    private bool isFlying = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        player = GameObject.FindGameObjectWithTag("Player");
        playerFist = GameObject.FindGameObjectWithTag("LeftFist").GetComponent<PlayerFist>();
    }

    public void Grab(Transform objectGrabPoint)
    {
        this.objectGrabPoint = objectGrabPoint;
        rb.useGravity = false;
        //rb.freezeRotation = true;
        //gameObject.GetComponent<Collider>().isTrigger = true;
        playerFist.anim.SetTrigger("Grab");
    }

    private void FixedUpdate()
    {
        if (objectGrabPoint != null)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPoint.position, Time.deltaTime * lerpSpeed); //lerp unused
            //rb.MovePosition(newPosition);
            rb.MovePosition(objectGrabPoint.position);
            transform.forward = player.transform.forward;
        }

        //if (rb.velocity.magnitude <= 0.5f && isFlying == true)
        //{
        //    gameObject.GetComponent<Collider>().isTrigger = false;
        //    isFlying = false;
        //}
    }

    public void Throw(float throwForce)
    {
        this.objectGrabPoint = null;
        rb.useGravity = true;
        //rb.freezeRotation = false;

        Vector3 dir = (transform.position - player.transform.position).normalized;
        //gameObject.GetComponent<Rigidbody>().AddForce(dir * throwForce, ForceMode.Impulse);
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * 30f;
        //gameObject.GetComponent<Collider>().isTrigger = true;
        isFlying = true;
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
