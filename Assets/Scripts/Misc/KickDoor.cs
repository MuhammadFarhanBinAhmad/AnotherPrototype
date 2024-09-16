using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickDoor : MonoBehaviour
{
    Rigidbody rb;
    public bool inrange;
    public bool isKick;
    public Animator fistAnimator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (fistAnimator == null)
        {
            fistAnimator = GameObject.FindGameObjectWithTag("LeftFist").GetComponent<Animator>();
        }
    }
    private void Update()
    {
        if (inrange)
        {
            if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && !isKick)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.velocity = transform.forward * 30f;
                isKick = true;
                fistAnimator.SetTrigger("Punch");
                StartCoroutine("SlowDownTime");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerMovement>() != null)
        {
            inrange = true;

            if (!isKick)
            FindObjectOfType<PlayerUI>().KickDoorUI("Press 'E' to breach door");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            inrange = false;
            FindObjectOfType<PlayerUI>().KickDoorUI("");
        }
    }
    IEnumerator SlowDownTime()
    {
        Time.timeScale = .3f;
        yield return new WaitForSeconds(.25f);
        Time.timeScale = 1f;
    }
}
