using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickDoor : MonoBehaviour
{
    Rigidbody rb;
    public bool inrange;
    public bool isKick;
    public Animator fistAnimator;
    public PlayerSkills playerSkills;

    public bool bypassDoor = false;

    public float kickForce;
    public int kickDamage;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        fistAnimator = GameObject.FindGameObjectWithTag("LeftFist").GetComponent<Animator>();
        playerSkills = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSkills>();
    }
    private void Update()
    {
        if (inrange)
        {
            if (bypassDoor == true)
            {
                if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && !isKick)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.velocity = transform.forward * 30f;
                    isKick = true;
                    if (fistAnimator != null)
                    {
                        fistAnimator.SetTrigger("Punch");
                    }
                    StartCoroutine("SlowDownTime");
                }
            }

            else if (playerSkills.keyCount > 0)
            {
                if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && !isKick)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.velocity = transform.forward * 30f;
                    isKick = true;
                    if (fistAnimator != null)
                    {
                        fistAnimator.SetTrigger("Punch");
                    }
                    playerSkills.RemoveKey(1);
                    StartCoroutine("SlowDownTime");
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerMovement>() != null)
        {
            inrange = true;

            if (!isKick)
            {
                if (playerSkills.keyCount >= 1 || bypassDoor == true)
                {
                    FindObjectOfType<PlayerUI>().KickDoorUI("Press 'E' to breach door");
                }
                else
                {
                    FindObjectOfType<PlayerUI>().KickDoorUI("Clear enemies for entry!");
                }
            }
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<EnemyHealth>() != null && isKick == true)
        {
            other.gameObject.transform.position = Vector3.MoveTowards(other.transform.position, gameObject.transform.position, -kickForce);
            other.gameObject.GetComponent<EnemyMovement>().StunEnemy();
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(kickDamage);
            isKick = false;
        }
    }
}
