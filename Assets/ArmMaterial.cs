using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmMaterial : MonoBehaviour
{
    [SerializeField] ArmMeshContainer amc;
    // Start is called before the first frame update
    void Start()
    {
        amc = FindObjectOfType<ArmMeshContainer>();
    }

    // Update is called once per frame
    public void ChangeMaterial(String material) {
        MaterialStore thisMat = null;
        switch(material) {
            case "Stun":
                thisMat = amc.stun;
                break;
            case "Shock":
                thisMat = amc.shock;
                break;
            case "Freeze":
                thisMat = amc.freeze;
                break;
            case "Burn":
                thisMat = amc.burn;
                break;
            default:
                break;
        }
        if(thisMat != null) {
            foreach(MeshRenderer mr in GetComponentsInChildren<MeshRenderer>()) {
                if(mr.gameObject.tag == "Arm") {
                    mr.material = thisMat.arm;
                }
                else if(mr.gameObject.tag == "ArmJoint") {
                    mr.material = thisMat.armJoint;
                }
            }
        } else {
            Debug.Log("thisMat was null!");
        }
    }
}
