using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFist : MonoBehaviour
{
    public int damage;
    public float punchCooldownMax = 1f;
    public float punchCooldownCurrent;

    public int knockbackForce;
    public bool isGrabbing = false; // prop is being grabbed in hand


    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        punchCooldownCurrent = punchCooldownMax;
    }

    // Update is called once per frame
    void Update()
    {
        punchCooldownCurrent += Time.deltaTime;
        if (punchCooldownCurrent >= punchCooldownMax)
        {
            punchCooldownCurrent = punchCooldownMax;
        }

        if (Input.GetMouseButtonDown(0) && punchCooldownCurrent >= punchCooldownMax)
        {
            Punch();
        }
        if (Input.GetKeyDown(KeyCode.F) && isGrabbing == false)
        {
            Grab();
        }
        if (Input.GetKeyDown(KeyCode.F) && isGrabbing == true)
        {
            Throw();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyHealth>() != null)
        {
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
    }

    public void Punch()
    {
        punchCooldownCurrent = 0;
        if (isGrabbing == true) // throw prop
        {
            Throw();
        }
        else // punch (nothing grabbed in hand)
        {
            anim.SetTrigger("Punch");
        }
        //StartCoroutine(ResetPunch());
    }

    public void Grab()
    {
        isGrabbing = true;
        anim.SetTrigger("Grab");
    }

    public void Throw()
    {
        isGrabbing = false;
        anim.SetTrigger("Throw");
    }

    //public IEnumerator ResetPunch()
    //{
    //    yield return new WaitForSeconds(0.25f);
    //    anim.SetBool("Punch", false);
    //}
}
