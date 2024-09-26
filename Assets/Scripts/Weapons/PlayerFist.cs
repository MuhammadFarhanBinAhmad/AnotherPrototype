using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFist : MonoBehaviour
{
    [Header ("Punch Properties")]
    public int damage;
    public int damageOnStunned;
    public int damageOnFrozen;
    public float punchCooldownMaxO;
    private float punchCooldownMax;
    private float punchCooldownCurrent;
    public float punchCooldownMaxF;
    public GameObject shockWave;
    public Vector3 offset = new Vector3(0f, 0.5f, 0);

    public GameObject player;
    public float knockbackForce;
    public GameObject HitShotEffect;

    [Header("Grab and Throw Properties")]
    public bool isGrabbing = false; // prop is being grabbed in hand
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private LayerMask pickUpLayerMask;
    [SerializeField]
    private Transform objectGrabPoint;
    public float grabDistance;
    public PropGrab propGrab;
    public EnemyGrab enemyGrab;
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

        punchCooldownMax = punchCooldownMaxO;
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
            if (propGrab == null && enemyGrab == null) // not carrying an object or enemy
            {
                Grab();
            }
            else
            {
                Throw(throwForce);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyHealth>() != null)
        {
            if (other.GetComponent<EnemyStatus>().isStunned == true) // enemy is afflicted by stun
            {
                other.GetComponent<EnemyHealth>().TakeDamage(damageOnStunned);
            }
            else if (other.GetComponent<EnemyStatus>().isBurnt == true) // enemy is afflicted by burn
            {
                other.GetComponent<EnemyHealth>().TakeBurnDamage();
            }
            else if (other.GetComponent<EnemyStatus>().isShocked == true) // enemy is afflicted by shock
            {
                Instantiate(shockWave, player.transform.position + offset, player.transform.rotation);
            }
            else if (other.GetComponent<EnemyStatus>().isFrozen == true) // enemy is afflicted by freeze
            {
                other.GetComponent<EnemyHealth>().TakeDamage(damageOnFrozen);
                other.GetComponent<EnemyMovement>().FreezeEnemy();
                punchCooldownMax = punchCooldownMaxF;
            }
            else
            {
                other.GetComponent<EnemyHealth>().TakeDamage(damage); // default punch
                punchCooldownMax = punchCooldownMaxO;
            }
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
        else if (enemyGrab != null) // carrying an enemy
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
                isGrabbing = true;
                propGrab.Grab(objectGrabPoint);
            }

            if (raycastHit.transform.TryGetComponent(out enemyGrab))
            {
                if (enemyGrab.canGrab == true)
                {
                    isGrabbing = true;
                    enemyGrab.Grab(objectGrabPoint);
                }
            }
        }
    }

    public void Throw(float throwForce)
    {
        anim.SetTrigger("Throw");

        if (propGrab != null)
        {
            propGrab.Throw(throwForce);
            propGrab = null;
            isGrabbing = false;
        }
        if (enemyGrab != null)
        {
            enemyGrab.Throw(throwForce);
            enemyGrab = null;
            isGrabbing = false;
        }
    }

    public void BigFist()
    {
        anim.SetTrigger("BigFist");

        bigFistCollider.canBigFist = true;
        canBigFist = false;
    }

    public void GrabDeath()
    {
        isGrabbing = false;
        anim.SetTrigger("Throw");
    }
}
