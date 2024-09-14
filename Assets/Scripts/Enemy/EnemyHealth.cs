using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int AmountOfExpDrop;
    public int AmountOfRoundDrop;

    public GameObject DeathSoundObject;
    public GameObject ExpDrop;
    public GameObject WeaponPrefab;

    public EnemyUI e_EnemyUI;

    private void Start()
    {
        health = maxHealth;
    }
    public void TakeDamage(int dmg)
    {
        health -= dmg;
        e_EnemyUI.UpdateEnemyUI();
        if (health <= 0)
        {
            Instantiate(DeathSoundObject);
            for (int i = 0; i < AmountOfExpDrop; i++)
            {
                GameObject exp = Instantiate(ExpDrop,transform.position,transform.rotation);
                Rigidbody rb = exp.GetComponent<Rigidbody>();
                Vector3 explosionDirection = Random.insideUnitSphere.normalized;
                if (rb!=null)
                {
                    rb.AddForce(explosionDirection * .5f, ForceMode.Impulse);
                }
            }
            if (GetComponent<EnemyRangeAttackBehaviour>() != null)
            {   
                if (!GetComponent<EnemyRangeAttackBehaviour>().e_GrenadeEnemy)
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

            FindObjectOfType<Flash>().Flashing();
            FindObjectOfType<CameraShake>().Shake(.1f, .75f, .75f);
            Destroy(gameObject);
        }
    }
}
