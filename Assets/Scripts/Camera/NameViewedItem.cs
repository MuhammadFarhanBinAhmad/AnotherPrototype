using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class NameViewedItem : MonoBehaviour
{
    public Transform cameraTransform;
    public float viewDistance = 10f;
    public string objectName;
    public PlayerUI playerUI;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit raycastHit, viewDistance))
        {
            objectName = raycastHit.collider.gameObject.name;
            playerUI.t_CurrAmmoLeft.text = objectName;
        }

    }
}
