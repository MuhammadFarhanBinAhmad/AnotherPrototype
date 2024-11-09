using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Guncase : MonoBehaviour
{
    public Animator guncaseAnimator;
    public Animator fistAnimator;
    public Animator weaponAnimator;
    public GameObject gunArm;
    public GameObject armArm; //weapon manager
    public GameObject weaponHUD;

    public bool inRange = false;
    public bool guncaseOpened = false;
    public bool armGrabbed = false;

    public TextMeshProUGUI pickupText;

    // Start is called before the first frame update
    void Start()
    {
        armArm.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange == true && armGrabbed == false)
        {
            if (guncaseOpened == false)
            {
                pickupText.text = "(E) Open Guncase";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    guncaseOpened = true;
                    guncaseAnimator.SetTrigger("Open");
                }
            }
            else
            {
                pickupText.text = "(E) Equip Gunarm";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    armGrabbed = true;
                    fistAnimator.SetTrigger("Equip");
                    StartCoroutine(DestroyCaseArm());
                    StartCoroutine(InstantiateArmArm());
                }
            }
        }
        else
        {
            pickupText.text = " ";
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            inRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            inRange = false;
        }
    }


        public IEnumerator DestroyCaseArm()
    {
        yield return new WaitForSeconds(0.25f);
        armArm.SetActive(true);
        Destroy(gunArm);
    }

    public IEnumerator InstantiateArmArm()
    {
        yield return new WaitForSeconds(1.5f);
        weaponAnimator.enabled = false;
        weaponHUD.SetActive(true);
    }
}
