using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

public class Weapon : MonoBehaviour
{
    public WeaponType p_WeaponType;

    [Header("General")]
    public string p_WeaponName;
    public GameObject p_WeaponModel;

    [Header("ShootProperties")]
    public float p_WeaponFireRate;
    public float p_WeaponChargeRate; // only applicable for charging weapons
    public float p_WeaponChargeCap;
    public int p_BulletCount; // shooting many bullets at once

    [Header("Ammo")]
    public int p_TotalAmmo;

    [Header("ProjectileStats")]
    public GameObject p_ProjectileType;
    public float p_BulletSpeed;
    public float p_BulletMaxDamage;
    public float p_BulletMinDamage;
    public float p_TimeBeforeSelfDestruct;
    public bool p_isPiercing;

    [Header("Audio")]
    public AudioClip p_GunShotAudio;
    public AudioClip p_GunPickupAudio;
    public AudioClip p_GunActionAudio; // stuff like charging up railgun

    [Header("WeaponFireMode")]
    public bool isAuto;
    public bool isCharge;


    void Start()
    {
        Vector3 spawnOffset = new Vector3(-0.5f, -0.25f, 0f);
        GameObject m = Instantiate(p_WeaponModel, transform.position + spawnOffset, transform.rotation); // generate weapon model
        m.transform.parent = gameObject.transform;

        if (p_WeaponType != null)
        {
            p_WeaponName = p_WeaponType.p_WeaponName;
            p_WeaponFireRate = p_WeaponType.p_WeaponFireRate;
            p_WeaponChargeRate = p_WeaponType.p_WeaponChargeRate;
            p_WeaponChargeCap = p_WeaponType.p_WeaponChargeCap;
            p_TotalAmmo = p_WeaponType.p_TotalAmmo;
            p_ProjectileType = p_WeaponType.p_ProjectileType;
            p_BulletSpeed = p_WeaponType.p_BulletSpeed;
            p_BulletMaxDamage = p_WeaponType.p_BulletMaxDamage;
            p_BulletMinDamage = p_WeaponType.p_BulletMinDamage;
            p_BulletCount = p_WeaponType.p_BulletCount;
            p_TimeBeforeSelfDestruct = p_WeaponType.p_TimeBeforeSelfDestruct;
            p_isPiercing = p_WeaponType.p_isPiercing;
            p_WeaponModel = p_WeaponType.p_WeaponModel;
            p_GunShotAudio = p_WeaponType.p_GunShotAudio;
            p_GunPickupAudio = p_WeaponType.p_GunPickupAudio;
            p_GunActionAudio = p_WeaponType.p_GunActionAudio;
            isAuto = p_WeaponType.isAuto;
            isCharge = p_WeaponType.isCharge;
        }
    }
    public void SetWeapon()
    {
        p_WeaponName = p_WeaponType.p_WeaponName;
        p_WeaponFireRate = p_WeaponType.p_WeaponFireRate;
        p_WeaponChargeRate = p_WeaponType.p_WeaponChargeRate;
        p_WeaponChargeCap = p_WeaponType.p_WeaponChargeCap;
        p_TotalAmmo = p_WeaponType.p_TotalAmmo;
        p_ProjectileType = p_WeaponType.p_ProjectileType;
        p_BulletSpeed = p_WeaponType.p_BulletSpeed;
        p_BulletMaxDamage = p_WeaponType.p_BulletMaxDamage;
        p_BulletMinDamage = p_WeaponType.p_BulletMinDamage;
        p_BulletCount = p_WeaponType.p_BulletCount;
        p_TimeBeforeSelfDestruct = p_WeaponType.p_TimeBeforeSelfDestruct;
        p_isPiercing = p_WeaponType.p_isPiercing;
        p_WeaponModel = p_WeaponType.p_WeaponModel;
        p_GunShotAudio = p_WeaponType.p_GunShotAudio;
        p_GunPickupAudio = p_WeaponType.p_GunPickupAudio;
        p_GunActionAudio = p_WeaponType.p_GunActionAudio;
        isAuto = p_WeaponType.isAuto;
        isCharge = p_WeaponType.isCharge;
    }
    public void SetWeapon(WeaponType wt)
    {
        p_WeaponName = wt.p_WeaponName;
        p_WeaponFireRate = wt.p_WeaponFireRate;
        p_WeaponChargeRate = wt.p_WeaponChargeRate;
        p_WeaponChargeCap = wt.p_WeaponChargeCap;
        p_TotalAmmo = wt.p_TotalAmmo;
        p_ProjectileType = wt.p_ProjectileType;
        p_BulletSpeed = wt.p_BulletSpeed;
        p_BulletMaxDamage = wt.p_BulletMaxDamage;
        p_BulletMinDamage = wt.p_BulletMinDamage;
        p_BulletCount = wt.p_BulletCount;
        p_TimeBeforeSelfDestruct = wt.p_TimeBeforeSelfDestruct;
        p_isPiercing = wt.p_isPiercing;
        p_WeaponModel = wt.p_WeaponModel;
        p_GunShotAudio = wt.p_GunShotAudio;
        p_GunPickupAudio = wt.p_GunPickupAudio;
        p_GunActionAudio = wt.p_GunActionAudio;
        isAuto = wt.isAuto;
        isCharge = wt.isCharge;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            //other.GetComponent<PlayerMovement>().PlayerWeapon.SetActive(true);
            FindObjectOfType<PlayerWeaponManager>().pickupWeapon = this;
            FindObjectOfType<PlayerUI>().PickUpWeaponUI(p_WeaponName);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() != null)
        {
            FindObjectOfType<PlayerWeaponManager>().pickupWeapon = null;
            FindObjectOfType<PlayerUI>().PickUpWeaponUI(null);
        }
    }
}
