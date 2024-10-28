using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

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
    public bool damageDealt = false;
    public GameObject HitShotEffect;
    public TimeSlow timeSlow;

    public bool canDestroy = false;

    private AudioSource audioSource;
    public AudioClip doorBreach;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        fistAnimator = GameObject.FindGameObjectWithTag("LeftFist").GetComponent<Animator>();
        playerSkills = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSkills>();
        timeSlow = GameObject.Find("GameManager").GetComponent<TimeSlow>();
        audioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (inrange)
        {
            if (bypassDoor == true)
            {
                KickDoorDown();
            }

            else if (playerSkills.keyCount >= 1)
            {
                KickDoorDown();
            }
        }

        if (canDestroy == true)
        {
            gameObject.transform.localScale -= new Vector3(.1f, .1f, .1f) * Time.deltaTime;
            StartCoroutine(DestroyDoor());
        }
    }

    public void KickDoorDown()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && !isKick)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = transform.forward * 80f;
            float originalVol = audioSource.volume;
            audioSource.volume = 1;
            audioSource.PlayOneShot(doorBreach);
            audioSource.volume = originalVol;

            isKick = true;

            if (bypassDoor != true)
            {
                playerSkills.RemoveKey(1);
            }

            if (fistAnimator != null)
            {
                fistAnimator.SetTrigger("Punch");
            }
            timeSlow.DoSlowMotion(0.1f, 2f);

            //StartCoroutine(ShrinkDoor());
        }
    }

    private void OnTriggerEnter(Collider other) // player kick range
    {
        if(other.GetComponent<PlayerMovement>() != null)
        {
            inrange = true;

            if (!isKick)
            {
                if (playerSkills.keyCount >= 1 || bypassDoor == true)
                {
                    FindObjectOfType<PlayerUI>().KickDoorUI("(E) Breach Door");
                }
                else
                {
                    FindObjectOfType<PlayerUI>().KickDoorUI("Clear Enemies!");
                }
            }
        }
    }
    private void OnTriggerExit(Collider other) // player kick range
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            inrange = false;
            FindObjectOfType<PlayerUI>().KickDoorUI("");
        }
    }

    private void OnCollisionEnter(Collision other) // door flying to hit enemy
    {
        if (other.gameObject.GetComponent<EnemyHealth>() != null && isKick == true)
        {
            if (damageDealt == false)
            {
                other.gameObject.transform.position = Vector3.MoveTowards(other.transform.position, gameObject.transform.position, -kickForce);
                other.gameObject.GetComponent<EnemyMovement>().StunEnemy();
                other.gameObject.GetComponent<EnemyHealth>().TakeDamage(kickDamage);
                Instantiate(HitShotEffect, transform.position, transform.rotation);
                damageDealt = true;
            }
        }
    }

    IEnumerator SlowDownTime()
    {
        Time.timeScale = .3f;
        yield return new WaitForSeconds(.25f);
        Time.timeScale = 1f;
    }

    public IEnumerator ShrinkDoor()
    {
        yield return new WaitForSeconds(3);
        canDestroy = true;
    }

    public IEnumerator DestroyDoor()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
