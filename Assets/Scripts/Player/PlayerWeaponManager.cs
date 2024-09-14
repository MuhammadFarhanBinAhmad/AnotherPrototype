using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    PlayerUI s_PlayerUI;

    public string p_WeaponName;

    [Header("FireRate")]
    public float p_WeaponFireRate;

    [Header("Ammo")]
    public int p_TotalAmmo;

    [Header("ProjectileStats")]
    public float p_BulletSpeed;
    public float p_BulletMaxDamage;
    public float p_BulletMinDamage;
    public float p_TimeBeforeSelfDestruct;
    public AudioClip p_WeaponSoundClip;

    [Header("SlowDownStats")]
    public bool isSlowDown;
    public float maxSlowDownTime;
    public float currSlowDownTime;
    public float slowDownTimeScale;

    public GameObject p_Projectile;
    public Transform p_Spawnpos;

    public bool isAuto;

    float nexttime_ToFire;

    public GameObject knifeObject;
    public GameObject currWeaponObject;
    public GameObject WeaponObject;
    public bool WeaponEquipped;

    public Weapon pickupWeapon;

    private void Start()
    {
        s_PlayerUI = FindObjectOfType<PlayerUI>();
        s_PlayerUI.UpdateWeaponUI();

        currSlowDownTime = maxSlowDownTime;
    }
    public void ChangeWeapon(Weapon wt)
    {
        if (WeaponEquipped)
        {
            GameObject w = Instantiate(WeaponObject, p_Spawnpos.position, p_Spawnpos.rotation);
            Weapon weapon = w.GetComponent<Weapon>();
            weapon.p_WeaponName = p_WeaponName;
            weapon.p_WeaponFireRate = p_WeaponFireRate;
            weapon.p_TotalAmmo = p_TotalAmmo;
            weapon.p_BulletSpeed = p_BulletSpeed;
            weapon.p_BulletMaxDamage = p_BulletMaxDamage;
            weapon.p_BulletMinDamage = p_BulletMinDamage;
            weapon.p_TimeBeforeSelfDestruct = p_TimeBeforeSelfDestruct;
            weapon.p_WeaponSoundClip = p_WeaponSoundClip;
            weapon.isAuto = isAuto;
        }

        p_WeaponName = wt.p_WeaponName;
        p_WeaponFireRate = wt.p_WeaponFireRate;
        p_TotalAmmo = wt.p_TotalAmmo;
        p_BulletSpeed = wt.p_BulletSpeed;
        p_BulletMaxDamage = wt.p_BulletMaxDamage;
        p_BulletMinDamage = wt.p_BulletMinDamage;
        p_TimeBeforeSelfDestruct = wt.p_TimeBeforeSelfDestruct;
        p_WeaponSoundClip = wt.p_WeaponSoundClip;
        isAuto = wt.isAuto;
        Destroy(wt.gameObject);
        //knifeObject.SetActive(false);
        currWeaponObject.SetActive(true);

        s_PlayerUI.PickUpWeaponUI(null);

        WeaponEquipped = true;

        s_PlayerUI.UpdateWeaponUI();
    }
    public void ClearWeapon()
    {
        p_WeaponName = null;
        p_WeaponFireRate = 0;
        p_TotalAmmo = 0;
        p_BulletSpeed = 0;
        p_BulletMaxDamage = 0;
        p_BulletMinDamage = 0;
        p_TimeBeforeSelfDestruct = 0;
        p_WeaponSoundClip = null;
        isAuto = false;
        WeaponEquipped = false;
        s_PlayerUI.UpdateWeaponUI();
    }
    private void Update()
    {

        if (isAuto)
        {
            if (Input.GetMouseButtonDown(1) && Time.time >= nexttime_ToFire && p_TotalAmmo > 0)
            {
                nexttime_ToFire = Time.time + 1f / p_WeaponFireRate;
                Shoot();
                p_TotalAmmo--;
                s_PlayerUI.UpdateWeaponUI();
            }
        }
        else if (Input.GetMouseButtonDown(1) && Time.time >= nexttime_ToFire && p_TotalAmmo > 0)
        {
            nexttime_ToFire = Time.time + 1f / p_WeaponFireRate;
            Shoot();
            p_TotalAmmo--;
            s_PlayerUI.UpdateWeaponUI();

        }
        if (Input.GetKeyDown(KeyCode.Q) && WeaponEquipped)
        {
            ThrowWeapon();
        }
        if (pickupWeapon !=null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            ChangeWeapon(pickupWeapon);
        }
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    if (!isSlowDown && currSlowDownTime > 0)
        //        StartSlowDownTime();
        //    else
        //        StopSlowDownTime();
        //}
        if (isSlowDown)
        {
            ActivateSlowDownEffect();
        }
    }

    public void Shoot()
    {
        GameObject p = Instantiate(p_Projectile, p_Spawnpos.position, p_Spawnpos.rotation);
        int damage = (int)(Random.Range(p_BulletMinDamage, p_BulletMaxDamage));
        p_Projectile.GetComponent<PlayerProjectile>().SetProjectileStats(p_BulletSpeed, damage);
    }
    public void ThrowWeapon()
    {
        GameObject w = Instantiate(WeaponObject,p_Spawnpos.position, p_Spawnpos.rotation);
        Weapon weapon =  w.GetComponent<Weapon>();
        weapon.p_WeaponName = p_WeaponName;
        weapon.p_WeaponFireRate = p_WeaponFireRate;
        weapon.p_TotalAmmo = p_TotalAmmo;
        weapon.p_BulletSpeed = p_BulletSpeed;
        weapon.p_BulletMaxDamage = p_BulletMaxDamage;
        weapon.p_BulletMinDamage = p_BulletMinDamage;
        weapon.p_TimeBeforeSelfDestruct = p_TimeBeforeSelfDestruct;
        weapon.p_WeaponSoundClip = p_WeaponSoundClip;
        weapon.isAuto = isAuto;

        Rigidbody w_rb = w.GetComponent<Rigidbody>();
        w_rb.velocity = transform.forward * 30f;
        ClearWeapon();

        knifeObject.SetActive(true);
        currWeaponObject.SetActive(false);
    }
    public void StartSlowDownTime()
    {
        isSlowDown = true;
        Time.timeScale = slowDownTimeScale;
    }

    public void ActivateSlowDownEffect()
    {
        if (currSlowDownTime > 0)
        {
            currSlowDownTime -= Time.deltaTime*2f;
        }
        else
        {
            StopSlowDownTime();
        }
        s_PlayerUI.SlowDownUI();
    }
    public void StopSlowDownTime()
    {
        isSlowDown = false;
        Time.timeScale = 1;
    }
}
