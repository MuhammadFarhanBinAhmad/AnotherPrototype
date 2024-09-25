using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon", order = 0)]
public class WeaponType : ScriptableObject
{
    [Header("General")]
    public string p_WeaponName;
    public GameObject p_WeaponModel;

    [Header("ShootProperties")]
    public int p_TotalAmmo;
    public float p_WeaponFireRate;
    public float p_WeaponChargeRate; // only applicable for charging weapons
    public float p_WeaponChargeCap;
    public int p_BulletCount; // shooting many bullets at once

    [Header("ProjectileStats")]
    public GameObject p_ProjectileType;
    public float p_BulletSpeed;
    public float p_BulletMaxDamage;
    public float p_BulletMinDamage;
    public float p_TimeBeforeSelfDestruct;
    public bool p_isPiercing;

    //public enum Element // your custom enumeration
    //{
    //    Stun,
    //    Burn,
    //    Shock,
    //    Freeze
    //};
    [Header("ProjectileElement - 'Stun', 'Burn', 'Shock', 'Freeze'")]
    //public Element projectileElement;
    public string p_ProjectileElement;
    public float p_ElementStackOnHit;

    [Header("Audio")]
    public AudioClip p_GunShotAudio;
    public AudioClip p_GunPickupAudio;
    public AudioClip p_GunActionAudio; // stuff like charging up railgun

    [Header("WeaponFireMode")]
    public bool isAuto;
    public bool isCharge;

    public void Start()
    {
        Debug.Log(p_ProjectileElement.ToString());
    }
}
