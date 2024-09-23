using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject WeaponPrefab;
    public WeaponType e_weaponType;
    public GameObject pickupAudio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() != null)
        {
            GameObject weapondrop = Instantiate(WeaponPrefab, transform.position, transform.rotation);
            WeaponPrefab = null;
            weapondrop.GetComponent<Weapon>().SetWeapon(e_weaponType);
            Rigidbody weapondrop_rb = weapondrop.GetComponent<Rigidbody>();
            Vector3 AmmoDirection = Random.insideUnitSphere.normalized;
            if (weapondrop_rb != null)
            {
                weapondrop_rb.AddForce(AmmoDirection * .5f, ForceMode.Impulse);
            }

            if (pickupAudio != null)
            {
                Instantiate(pickupAudio, gameObject.transform.position, gameObject.transform.rotation);
            }
            Destroy(transform.parent.gameObject);
        }
    }
}
