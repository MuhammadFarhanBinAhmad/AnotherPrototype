using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public List<Material> enemyMaterials;

    public Animator animator;


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

        enemyMaterials = GetComponent<MeshRenderer>().materials.ToList();
        if (gameObject.GetComponentInChildren<SkinnedMeshRenderer>() != null )
        {
            enemyMaterials = GetComponentInChildren<SkinnedMeshRenderer>().materials.ToList();
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
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
        //animator.SetFloat("Remaining Distance", 0);
        enemyForceDetector.CheckElement();
        this.objectGrabPoint = objectGrabPoint;
        rb.useGravity = false;
        playerFist.anim.SetTrigger("Grab");
        animator.SetFloat("Speed", 0);
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

            foreach (Material material in enemyMaterials) // if enemy is opaque, make translucent
            {
                Color enemyColour = material.color;
                enemyColour.a = Mathf.Clamp(enemyColour.a, 0.5f, 1f);
                enemyColour.a -= Time.deltaTime;
                material.color = enemyColour;
            }
        }
        else
        {
            foreach (Material material in enemyMaterials) // if enemy is translucent, make opaque
            {
                Color enemyColour = material.color;
                enemyColour.a = Mathf.Clamp(enemyColour.a, 0.5f, 1f);
                enemyColour.a += Time.deltaTime;
                material.color = enemyColour;
            }
        }

        if (isThrow == true)
        {
            rb.MovePosition(enemyYeeter.transform.position);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == ("Wall"))
        {
            playerFist.canThrow = false;
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == ("Wall"))
        {
            playerFist.canThrow = true;
        }
    }
}
