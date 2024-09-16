using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFist : MonoBehaviour
{
    public int damage;
    public float punchCooldownMax = 1f;
    public float punchCooldownCurrent;

    public GameObject player;
    public float knockbackForce;
    public GameObject HitShotEffect;

    public bool isGrabbing = false; // prop is being grabbed in hand
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private LayerMask pickUpLayerMask;
    [SerializeField]
    private Transform objectGrabPoint;
    public float grabDistance;
    private PropGrab propGrab;
    public float throwForce;

    public Animator anim;

    public bool canBigFist = false;
    public BigFistCollider bigFistCollider;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        punchCooldownCurrent = punchCooldownMax;

        cameraTransform = Camera.main.transform;
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
            if (canBigFist == true)
            {
                BigFist();
            }
            else
            {
                Punch();
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (propGrab == null) // not carrying an object
            {
                Grab();
            }
            else
            {
                Throw(throwForce);
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && isGrabbing == true)
        {
            Throw(throwForce);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyHealth>() != null)
        {
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
            Instantiate(HitShotEffect, other.transform.position, other.transform.rotation);
            other.gameObject.transform.position = Vector3.MoveTowards(other.transform.position, player.transform.position, -knockbackForce);
        }
    }

    public void Punch()
    {
        punchCooldownCurrent = 0;
        if (propGrab != null) // carrying an object
        {
            Throw(throwForce);
        }
        else // punch (nothing grabbed in hand)
        {
            anim.SetTrigger("Punch");
        }
    }

    public void Grab()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit raycastHit, grabDistance, pickUpLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out propGrab))
            {
                propGrab.Grab(objectGrabPoint);
            }
        }
    }

    public void Throw(float throwForce)
    {
        anim.SetTrigger("Throw");

        propGrab.Throw(throwForce);
        propGrab = null;
    }

    public void BigFist()
    {
        anim.SetTrigger("BigFist");

        bigFistCollider.canBigFist = true;
        canBigFist = false;
    }
}
