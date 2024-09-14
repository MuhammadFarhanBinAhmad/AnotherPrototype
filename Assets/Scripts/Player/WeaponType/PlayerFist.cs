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
            Punch();
        }
        if (Input.GetKeyDown(KeyCode.F))// && isGrabbing == false)
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
            Vector3 dir = (other.transform.position - player.transform.position).normalized;
            other.gameObject.GetComponent<Rigidbody>().AddForce(dir * knockbackForce, ForceMode.Impulse); // knockback doesnt work
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
        //StartCoroutine(ResetPunch());
    }

    public void Grab()
    {
        //anim.SetTrigger("Grab");

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
}
