using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Elemental Materials", menuName = "ScriptableObjects/Materials", order = 1)]
public class MaterialStore : ScriptableObject
{
    public Material arm;
    public Material armJoint;
}
