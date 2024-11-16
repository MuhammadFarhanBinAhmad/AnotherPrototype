using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int AmountOfExpDrop;
    public int AmountOfRoundDrop;
    public bool dropKey = false;

    public GameObject DeathSoundObject;
    public GameObject DoorKey;
    public GameObject WeaponPrefab;

    public EnemyUI e_EnemyUI;

    public int burnTicks;
    private int burnCount;
    public int burnDamage;
    public int shockTicks;
    private int shockCount;
    public int shockDamage;

    public GameObject burnTickEffect;
    public GameObject shockTickEffect;

    public PlayerFist playerFist; // to reset grab status if enemy burn to death while held
    public RoomManager roomManager; // to reset leader status on death
    public bool assignFist = true; // false if specialscene

    [SerializeField]
    public Animator crosshairAnim;

    private void Start()
    {
        health = maxHealth;

        if (assignFist == true)
        {
            playerFist = GameObject.FindGameObjectWithTag("LeftFist").GetComponent<PlayerFist>();
        }
        if (gameObject.transform.parent != null)
        {
            roomManager = gameObject.transform.parent.gameObject.GetComponent<RoomManager>();
        }
        if (crosshairAnim == null) {
            crosshairAnim = GameObject.Find("CrosshairDefault").GetComponentInChildren<Animator>();
        }
    }
    public void TakeDamage(int dmg)
    {
        health -= dmg;
        e_EnemyUI.UpdateEnemyHealth();

        crosshairAnim.SetTrigger("Pulse");


        if (playerFist != null)
        {
            transform.LookAt(playerFist.transform.position);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeBurnDamage()
    {
        InvokeRepeating("DoBurn", 0.5f, 0.5f);
    }

    public void DoBurn()
    {
        TakeDamage(burnDamage);
        if (burnTickEffect != null)
        {
            Instantiate(burnTickEffect, gameObject.transform.position, gameObject.transform.rotation);
        }
        burnCount += 1;
        if (burnCount >= burnTicks)
        {
            CancelInvoke("DoBurn");
        }
    }

    public void TakeShockDamage()
    {
        InvokeRepeating("DoShock", 0.5f, 1f);
    }

    public void DoShock()
    {
        TakeDamage(shockDamage);
        if (shockTickEffect != null)
        {
            Instantiate(shockTickEffect, gameObject.transform.position, gameObject.transform.rotation);
        }
        shockCount += 1;
        if (shockCount >= shockTicks)
        {
            CancelInvoke("DoShock");
        }
    }

    public void Die()
    {
        Instantiate(DeathSoundObject);
        if (playerFist != null)
        {
            playerFist.GrabDeath();
        }

        if (GetComponent<EnemyRangeAttackBehaviour>() != null)
        {
            if (WeaponPrefab != null)
            {
                GameObject weapondrop = Instantiate(WeaponPrefab, transform.position, transform.rotation);
                WeaponPrefab = null;
                weapondrop.GetComponent<Weapon>().SetWeapon(GetComponent<EnemyRangeAttackBehaviour>().e_weaponType);
                Rigidbody weapondrop_rb = weapondrop.GetComponent<Rigidbody>();
                Vector3 AmmoDirection = Random.insideUnitSphere.normalized;
                if (weapondrop_rb != null)
                {
                    weapondrop_rb.AddForce(AmmoDirection * .5f, ForceMode.Impulse);
                }
            }
        }
        if (GetComponent<EnemyMeleeAttackBehaviour>() != null)
        {
            if (WeaponPrefab != null)
            {
                GameObject weapondrop = Instantiate(WeaponPrefab, transform.position, transform.rotation);
                WeaponPrefab = null;
                weapondrop.GetComponent<Weapon>().SetWeapon(GetComponent<EnemyMeleeAttackBehaviour>().e_weaponType);
                Rigidbody weapondrop_rb = weapondrop.GetComponent<Rigidbody>();
                Vector3 AmmoDirection = Random.insideUnitSphere.normalized;
                if (weapondrop_rb != null)
                {
                    weapondrop_rb.AddForce(AmmoDirection * .5f, ForceMode.Impulse);
                }
            }

            if (GetComponent<EnemyMeleeAttackBehaviour>().isLeader == true)
            {
                roomManager.leaderCount -= 1;
            }
        }

        if (dropKey == true)
        {
            Instantiate(DoorKey, transform.position, transform.rotation);
            dropKey = false;
        }

        //FindObjectOfType<Flash>().Flashing();
        FindObjectOfType<CameraShake>().Shake(.1f, .75f, .75f);
        Destroy(gameObject);
    }
}
