using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public PlayerUI s_PlayerUI;
    public BarrelSmoke BarrelSmoke;
    public ParticleSystem MuzzleFlash;

    [Header("General")]
    public string p_WeaponName;
    public GameObject p_WeaponModel;
    public Material p_WeaponMaterial;
    public Transform p_Spawnpos; // spawnpoint for both weapons and projectiles
    public Transform recoilEndPos; // pos manager moves towards when shooting
    public Transform recoilStartPos;

    public Color stunColour = new Color(175f, 255f, 0f, 255f);
    public Color burnColour = new Color(255f, 150f, 0f, 255f);
    public Color shockColour = new Color(150f, 0f, 200f, 255f);
    public Color freezeColour = new Color(0f, 255f, 255f, 255f);

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

    [Header("ProjectileElement - 'Stun', 'Burn', 'Shock', 'Freeze'")]
    public string p_ProjectileElement;
    public float p_ElementStackOnHit;

    [Header("Audio")]
    public AudioClip p_GunShotAudio;
    public AudioClip p_GunPickupAudio;
    public AudioClip p_GunActionAudio; // stuff like charging up railgun

    public float initialPitch;
    public float currentPitch;
    public float maxPitch;
    public float pitchCounter;
    public float pitchCap;

    //[Header("SlowDownStats")]
    //public bool isSlowDown;
    //public float maxSlowDownTime;
    //public float currSlowDownTime;
    //public float slowDownTimeScale;

    [Header("WeaponFireMode")]
    public bool isAuto;
    public bool isCharge;

    float nexttime_ToFire;
    public float currentCharge;
    Vector3 startingPos; // starting position of weapon
    Vector3 startingRotation; // starting rotation of firepoint

    [Header("Weapon Swapping")]
    //public GameObject knifeObject;
    public GameObject initialWeapon; // weapon player spawns with (model)
    public GameObject currWeaponObject; // weapon being held
    public GameObject WeaponObject; //weapon to be thrown to the ground
    public bool WeaponEquipped;

    public Weapon pickupWeapon;

    public PlayerFist playerFist; // ref for anim

    private bool godMode = false;
    private float o_WeaponFireRate;
    private float o_BulletMinDamage;
    private float o_BulletMaxDamage;
    private int o_TotalAmmo;
    

    private void Start()
    {
        s_PlayerUI = FindObjectOfType<PlayerUI>();
        s_PlayerUI.UpdateWeaponUI();

        currentCharge = 0f;
        startingPos.x = currWeaponObject.transform.localPosition.x; // this is weap startingpos not weap controller
        startingPos.y = currWeaponObject.transform.localPosition.y;
        startingPos.z = currWeaponObject.transform.localPosition.z;

        startingRotation.x = p_Spawnpos.transform.rotation.x;
        startingRotation.y = p_Spawnpos.transform.rotation.y;
        startingRotation.z = p_Spawnpos.transform.rotation.z;

        o_TotalAmmo = p_TotalAmmo;

        initialPitch = GetComponent<AudioSource>().pitch;
        currentPitch = initialPitch;
    }

    public void ChangeWeapon(Weapon wt)
    {
        if (WeaponEquipped)
        {
            GameObject w = Instantiate(WeaponObject, p_Spawnpos.position, p_Spawnpos.rotation);
            //AssignModelColour(p_WeaponMaterial, p_ProjectileElement);
            //armMaterial.ChangeMaterial(p_ProjectileElement);
            p_WeaponModel.GetComponent<Renderer>().material = p_WeaponMaterial;
            Weapon weapon = w.GetComponent<Weapon>();
            weapon.p_WeaponName = p_WeaponName;
            weapon.p_WeaponFireRate = p_WeaponFireRate;
            weapon.p_WeaponChargeRate = p_WeaponChargeRate;
            weapon.p_WeaponChargeCap = p_WeaponChargeCap;
            weapon.p_TotalAmmo = p_TotalAmmo;
            weapon.p_ProjectileType = p_ProjectileType;
            weapon.p_BulletSpeed = p_BulletSpeed;
            weapon.p_BulletMaxDamage = p_BulletMaxDamage;
            weapon.p_BulletMinDamage = p_BulletMinDamage;
            weapon.p_BulletCount = p_BulletCount;
            weapon.p_TimeBeforeSelfDestruct = p_TimeBeforeSelfDestruct;
            weapon.p_isPiercing = p_isPiercing;
            weapon.p_ProjectileElement = p_ProjectileElement;
            weapon.p_ElementStackOnHit = p_ElementStackOnHit;
            weapon.p_WeaponModel = p_WeaponModel;
            weapon.p_WeaponMaterial = p_WeaponMaterial;
            weapon.p_GunShotAudio = p_GunShotAudio;
            weapon.p_GunPickupAudio = p_GunPickupAudio;
            weapon.p_GunActionAudio = p_GunActionAudio;
            weapon.isAuto = isAuto;
            weapon.isCharge = isCharge;
        }

        p_WeaponName = wt.p_WeaponName;  // this stuff makes new weapon take on intended properties
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
        p_ProjectileElement = wt.p_ProjectileElement;
        p_ElementStackOnHit = wt.p_ElementStackOnHit;
        p_WeaponModel = wt.p_WeaponModel;
        p_WeaponMaterial = wt.p_WeaponMaterial;
        p_GunShotAudio = wt.p_GunShotAudio;
        p_GunPickupAudio = wt.p_GunPickupAudio;
        p_GunActionAudio = wt.p_GunActionAudio;
        isAuto = wt.isAuto;
        isCharge = wt.isCharge;

        Destroy(initialWeapon);
        Destroy(wt.gameObject);
        //knifeObject.SetActive(false);
        currWeaponObject.SetActive(true);
        GameObject m = Instantiate(p_WeaponModel, currWeaponObject.transform.position, currWeaponObject.transform.rotation); // generate weapon model
        p_WeaponModel.GetComponent<Renderer>().material = p_WeaponMaterial;
        //AssignModelColour(p_WeaponMaterial, p_ProjectileElement);
        //armMaterial.ChangeMaterial(p_ProjectileElement);
        m.transform.parent = currWeaponObject.transform;
        initialWeapon = m;
        s_PlayerUI.PickUpWeaponUI(null);

        WeaponEquipped = true;

        s_PlayerUI.UpdateWeaponUI();
    }
    public void ClearWeapon()
    {
        p_WeaponName = null;
        p_WeaponFireRate = 0;
        p_WeaponChargeRate = 0;
        p_WeaponChargeCap = 0;
        p_TotalAmmo = 0;
        p_ProjectileType = null;
        p_BulletSpeed = 0;
        p_BulletMaxDamage = 0;
        p_BulletMinDamage = 0;
        p_BulletCount = 0;
        p_TimeBeforeSelfDestruct = 0;
        p_isPiercing = false;
        p_ProjectileElement = null;
        p_ElementStackOnHit = 0;
        p_WeaponModel = null;
        p_WeaponMaterial = null;
        p_GunShotAudio = null;
        p_GunPickupAudio = null;
        p_GunActionAudio = null;
        isAuto = false;
        isCharge = false;
        WeaponEquipped = false;
        s_PlayerUI.UpdateWeaponUI();
    }
    private void Update()
    {
        CheckFireMode();

        if (Input.GetKeyDown(KeyCode.Q) && WeaponEquipped)
        {
            ThrowWeapon();
        }

        if (pickupWeapon !=null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ChangeWeapon(pickupWeapon);
                playerFist.anim.SetTrigger("Swap");
                s_PlayerUI.blankAmmo = false;

                if (pickupWeapon.p_GunPickupAudio != null)
                {
                    GetComponent<AudioSource>().PlayOneShot(pickupWeapon.p_GunPickupAudio);
                }
            }
        }
        
        transform.position = Vector3.MoveTowards(transform.position, recoilStartPos.position, 0.05f);

        //if (Input.GetMouseButton(1))
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, recoilEndPos.position, 0.1f);
        //}

        #region CheatKeys

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            p_ProjectileElement = "Stun";
            s_PlayerUI.UpdateWeaponUI();
            s_PlayerUI.UpdateTextColour();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            p_ProjectileElement = "Burn";
            s_PlayerUI.UpdateWeaponUI();
            s_PlayerUI.UpdateTextColour();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            p_ProjectileElement = "Shock";
            s_PlayerUI.UpdateWeaponUI();
            s_PlayerUI.UpdateTextColour();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            p_ProjectileElement = "Freeze";
            s_PlayerUI.UpdateWeaponUI();
            s_PlayerUI.UpdateTextColour();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            p_TotalAmmo = o_TotalAmmo;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            GodMode();
        }
        #endregion

        pitchCounter -= Time.deltaTime;
        if (pitchCounter < 0)
        {
            currentPitch = initialPitch;
        }
    }

    public void CheckFireMode()
    {
        if (isAuto)
        {
            if (playerFist.isGrabbing == false)
            {
                if (Input.GetMouseButton(1) && Time.time >= nexttime_ToFire && p_TotalAmmo > 0) // automated weapon
                {
                    transform.position = Vector3.MoveTowards(transform.position, recoilEndPos.position, 0.5f);
                    nexttime_ToFire = Time.time + 1f / p_WeaponFireRate;
                    Shoot();
                    p_TotalAmmo--;
                    s_PlayerUI.UpdateWeaponUI();
                }
            }
        }

        else if (isCharge)
        {
            if (playerFist.isGrabbing == false)
            {
                if (Input.GetMouseButton(1) && p_TotalAmmo > 0) // charging weapon
                {
                    transform.position = Vector3.MoveTowards(transform.position, recoilEndPos.position, 0.5f);
                    if (p_GunShotAudio != null)
                    {
                        GetComponent<AudioSource>().PlayOneShot(p_GunActionAudio);
                    }
                    currentCharge += Time.deltaTime * p_WeaponChargeRate;
                    if (currentCharge >= p_WeaponChargeCap)
                    {
                        currentCharge = p_WeaponChargeCap + 0.1f; // +0.1 so weapon can shoot before losing charge
                    }
                }
                else
                {
                    if (playerFist.isGrabbing == false)
                    {
                        currentCharge -= Time.deltaTime * p_WeaponChargeRate; // weapon loses charge
                        if (currentCharge <= 0)
                        {
                            currentCharge = 0;
                        }
                    }
                }

                currWeaponObject.transform.localPosition = new Vector3(startingPos.x + Mathf.Sin(Time.time * currentCharge * 2f) * 0.05f, startingPos.y, startingPos.z);
                float clampX = currWeaponObject.transform.localPosition.x;
                clampX = Mathf.Clamp(transform.position.x, -2.0f, 2.0f); // clamp dont seem to work for now

                if (Input.GetMouseButtonUp(1) && currentCharge >= p_WeaponChargeCap && p_TotalAmmo > 0)
                {
                    Shoot();
                    p_TotalAmmo--;
                    s_PlayerUI.UpdateWeaponUI();
                    currentCharge = 0;
                }
            }
        }

        else if (Input.GetMouseButtonDown(1) && Time.time >= nexttime_ToFire && p_TotalAmmo > 0) // semi auto weapon
        {
            if (playerFist.isGrabbing == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, recoilEndPos.position, 0.5f);
                nexttime_ToFire = Time.time + 1f / p_WeaponFireRate;
                Shoot();
                p_TotalAmmo--;
                s_PlayerUI.UpdateWeaponUI();
            }
        }
    }

    public void Shoot()
    {
        //transform.position = Vector3.MoveTowards(transform.position, recoilEndPos.position, 0.1f);

        for (int bulletCount = p_BulletCount; bulletCount > 0; bulletCount--)
        {
            if (p_BulletCount > 1) // more than 1 bullet, randomise spread
            {
                p_Spawnpos.transform.localRotation = Quaternion.identity;
                p_Spawnpos.Rotate(new Vector3(Random.Range(-12, 12), Random.Range(-12, 12), Random.Range(-12, 12)));
                GameObject p = Instantiate(p_ProjectileType, p_Spawnpos.position, p_Spawnpos.rotation);
                int damage = (int)(Random.Range(p_BulletMinDamage, p_BulletMaxDamage));
                p_ProjectileType.GetComponent<PlayerProjectile>().SetProjectileElements(p_ProjectileElement, p_ElementStackOnHit);
                p_ProjectileType.GetComponent<PlayerProjectile>().SetProjectileStats(p_BulletSpeed, damage, p_TimeBeforeSelfDestruct, p_isPiercing);
            }
            else // 1 bullet, dead center
            {
                //p_Spawnpos.transform.localRotation = Quaternion.identity;
                GameObject p = Instantiate(p_ProjectileType, p_Spawnpos.position, p_Spawnpos.rotation);
                int damage = (int)(Random.Range(p_BulletMinDamage, p_BulletMaxDamage));
                p_ProjectileType.GetComponent<PlayerProjectile>().SetProjectileElements(p_ProjectileElement, p_ElementStackOnHit);
                p_ProjectileType.GetComponent<PlayerProjectile>().SetProjectileStats(p_BulletSpeed, damage, p_TimeBeforeSelfDestruct, p_isPiercing);

                if (BarrelSmoke != null)
                {
                    BarrelSmoke.shotFired = true;
                }
                pitchCounter = pitchCap;

                MuzzleFlash.Play();
            }
        }

        if (p_GunShotAudio != null)
        {
            if (pitchCounter >= 0)
            {
                currentPitch += 0.05f;
                if (currentPitch >= maxPitch)
                {
                    currentPitch = maxPitch;
                }
            }
            else
            {
                currentPitch = initialPitch;
            }

            var AudioSource = GetComponent<AudioSource>();
            AudioSource.pitch = currentPitch;
            AudioSource.PlayOneShot(p_GunShotAudio);
        }
    }

    public void ThrowWeapon()
    {
        GameObject w = Instantiate(WeaponObject, p_Spawnpos.position, p_Spawnpos.rotation);
        //AssignModelColour(p_WeaponMaterial, p_ProjectileElement);
        //armMaterial.ChangeMaterial(p_ProjectileElement);
        p_WeaponModel.GetComponent<Renderer>().material = p_WeaponMaterial;
        Weapon weapon = w.GetComponent<Weapon>();
        weapon.p_WeaponName = p_WeaponName;
        weapon.p_WeaponFireRate = p_WeaponFireRate;
        weapon.p_WeaponChargeRate = p_WeaponChargeRate;
        weapon.p_WeaponChargeCap = p_WeaponChargeCap;
        weapon.p_TotalAmmo = p_TotalAmmo;
        weapon.p_ProjectileType = p_ProjectileType;
        weapon.p_BulletSpeed = p_BulletSpeed;
        weapon.p_BulletMaxDamage = p_BulletMaxDamage;
        weapon.p_BulletMinDamage = p_BulletMinDamage;
        weapon.p_BulletCount = p_BulletCount;
        weapon.p_TimeBeforeSelfDestruct = p_TimeBeforeSelfDestruct;
        weapon.p_isPiercing = p_isPiercing;
        weapon.p_ProjectileElement = p_ProjectileElement;
        weapon.p_ElementStackOnHit = p_ElementStackOnHit;
        weapon.p_WeaponModel = p_WeaponModel;
        weapon.p_WeaponMaterial = p_WeaponMaterial;
        weapon.p_GunShotAudio = p_GunShotAudio;
        weapon.p_GunPickupAudio = p_GunPickupAudio;
        weapon.p_GunActionAudio = p_GunActionAudio;
        weapon.isAuto = isAuto;
        weapon.isCharge = isCharge;

        Rigidbody w_rb = w.GetComponent<Rigidbody>(); // throw weapon
        w_rb.velocity = transform.forward * 30f;
        ClearWeapon();

        if (initialWeapon != null)
        {
            Destroy(initialWeapon);
        }

        //knifeObject.SetActive(true);
        currWeaponObject.SetActive(false);
    }


    public void AssignModelColour(Material weaponMaterial, string element)
    {

        if (element == null)
        {
            return;
        }
        if (element.Contains("Stun"))
        {
            weaponMaterial.color = stunColour;
        }
        else if (element.Contains("Burn"))
        {
            weaponMaterial.color = burnColour;
        }
        else if (element.Contains("Shock"))
        {
            weaponMaterial.color = shockColour;
        }
        else if (element.Contains("Freeze"))
        {
            weaponMaterial.color = freezeColour;
        }

        
    }

    public void GodMode()
    {
        if (godMode == false)
        {
            o_WeaponFireRate = p_WeaponFireRate;
            o_BulletMinDamage = p_BulletMinDamage;
            o_BulletMaxDamage = p_BulletMaxDamage;
            o_TotalAmmo = p_TotalAmmo;
            p_WeaponFireRate = 50;
            p_BulletMinDamage = 20;
            p_BulletMaxDamage = 20;
            p_TotalAmmo = 10000;
            godMode = true;
        }
        else
        {
            p_WeaponFireRate = o_WeaponFireRate;
            p_BulletMinDamage = o_BulletMinDamage;
            p_BulletMaxDamage = o_BulletMaxDamage;
            p_TotalAmmo = o_TotalAmmo;
            godMode = false;
        }
    }

    //public void StartSlowDownTime()
    //{
    //    isSlowDown = true;
    //    Time.timeScale = slowDownTimeScale;
    //}

    //public void ActivateSlowDownEffect()
    //{
    //    if (currSlowDownTime > 0)
    //    {
    //        currSlowDownTime -= Time.deltaTime*2f;
    //    }
    //    else
    //    {
    //        StopSlowDownTime();
    //    }
    //    s_PlayerUI.SlowDownUI();
    //}
    //public void StopSlowDownTime()
    //{
    //    isSlowDown = false;
    //    Time.timeScale = 1;
    //}
}
