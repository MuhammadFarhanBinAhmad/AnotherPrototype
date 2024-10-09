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
        //Ka Long this one i settle with you
        // sorry bossku i was notty :(

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
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * throwForce;
        //gameObject.GetComponent<Collider>().isTrigger = true;
        isFlying = true;
    }
}
