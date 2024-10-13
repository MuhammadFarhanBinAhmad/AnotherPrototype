using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public List<Material> propMaterials;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        //Ka Long this one i settle with you
        // sorry bossku i was notty :(

        player = GameObject.FindGameObjectWithTag("Player");
        playerFist = GameObject.FindGameObjectWithTag("LeftFist").GetComponent<PlayerFist>();

        propMaterials = GetComponent<MeshRenderer>().materials.ToList();
        if (gameObject.GetComponentInChildren<SkinnedMeshRenderer>() != null)
        {
            propMaterials = GetComponentInChildren<SkinnedMeshRenderer>().materials.ToList();
        }
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

            foreach (Material material in propMaterials) // if prop is opaque, make translucent
            {
                Color propColour = material.color;
                propColour.a = Mathf.Clamp(propColour.a, 0.5f, 1f);
                propColour.a -= Time.deltaTime;
                material.color = propColour;
            }
        }
        else
        {
            foreach (Material material in propMaterials)  // if prop is translucent, make opaque
            {
                Color propColour = material.color;
                propColour.a = Mathf.Clamp(propColour.a, 0.5f, 1f);
                propColour.a += Time.deltaTime;
                material.color = propColour;
            }
        }
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
