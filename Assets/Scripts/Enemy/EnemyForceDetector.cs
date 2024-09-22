using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyForceDetector : MonoBehaviour
{
    public float impactForceThreshold = 10f; // A threshold to determine if the collision was strong enough.
    public float knockbackForce;
    public int stunDamage;
    public int freezeDamage;

    public EnemyStatus enemyStatus;
    public bool isStunned = false;
    public bool isBurnt = false;
    public bool isShocked = false;
    public bool isFrozen = false;

    public bool hasCollided = false;

    public GameObject stunMine;
    private Vector3 mineOffset1 = new Vector3(4f, -0.75f, 0f);
    private Vector3 mineOffset2 = new Vector3(-4f, -0.75f, 0f);
    private Vector3 mineOffset3 = new Vector3(0f, -0.75f, 4f);
    private Vector3 mineOffset4 = new Vector3(0f, -0.75f, -4f);
    public bool minesSpawned = false;
    public GameObject explosion;
    public bool canExplode = false;
    public GameObject shockAura;
    private Vector3 auraOffset = new Vector3(0f, -0.75f, 0f);
    private bool auraPresent = false;
    public GameObject iceTrail;
    private Vector3 iceOffset = new Vector3(0f, -0.75f, 0f);
    public int maxIce = 6;
    public int currentIce = 0;

    public EnemyGrab enemyGrab;
    public GameObject enemyYeeter;


    private void Start()
    {
        enemyStatus = gameObject.GetComponent<EnemyStatus>();
        enemyGrab = gameObject.GetComponent<EnemyGrab>();
        enemyYeeter = GameObject.FindGameObjectWithTag("EnemyYeeter");
    }

    private void Update()
    {
        if (enemyGrab.isThrow == true)
        {
            if (isStunned == true)
            {
                StartCoroutine(StunPotato());
            }
        }

        if (enemyGrab.isThrow == true)
        {
            if (isBurnt == true)
            {
                StartCoroutine(HotPotato());
            }
        }

        if (enemyGrab.isThrow == true)
        {
            if (isShocked == true)
            {
                StartCoroutine(ShockPotato());
            }
        }
        if (auraPresent == true)
        {
            shockAura.transform.position = gameObject.transform.position;
        }

        if (enemyGrab.isThrow == true)
        {
            if (isFrozen == true)
            {
                InvokeRepeating("SpawnIce", 0.25f, 1f);
            }
        }
    }

    public void CheckElement()
    {
        if (enemyStatus.isStunned == true)
        {
            isStunned = true;
        }
        if (enemyStatus.isBurnt == true)
        {
            isBurnt = true;
        }
        if (enemyStatus.isShocked == true)
        {
            isShocked = true;
        }
        if (enemyStatus.isFrozen == true)
        {
            isFrozen = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Get the relative velocity between the two objects
        Vector3 relativeVelocity = collision.relativeVelocity;

        // Calculate the force based on the mass of the object and the relative velocity
        Rigidbody rb = collision.rigidbody;

        if (rb != null)
        {
            // Force = mass * acceleration (here, velocity change)
            float impactForce = rb.mass * relativeVelocity.magnitude;

            if (enemyGrab.isThrow == true)
            {
                if (collision.transform.GetComponent<EnemyHealth>() != null)
                {
                    hasCollided = true;

                    if (isStunned == true)
                    {
                        Instantiate(stunMine, gameObject.transform.position + mineOffset1, Quaternion.Euler(new Vector3(0, 0, 0)));
                        Instantiate(stunMine, gameObject.transform.position + mineOffset2, Quaternion.Euler(new Vector3(0, 0, 0)));
                        Instantiate(stunMine, gameObject.transform.position + mineOffset3, Quaternion.Euler(new Vector3(0, 0, 0)));
                        Instantiate(stunMine, gameObject.transform.position + mineOffset4, Quaternion.Euler(new Vector3(0, 0, 0)));

                        if (collision.transform.GetComponent<EnemyStatus>() != null)
                        {
                            collision.transform.GetComponent<EnemyStatus>().Stunned();
                        }
                        
                        isStunned = false;
                    }

                    if (isBurnt == true)
                    {
                        Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);

                        if (collision.transform.GetComponent<EnemyStatus>() != null)
                        {
                            collision.transform.GetComponent<EnemyStatus>().Burnt();
                        }
                        isBurnt = false;
                    }

                    if (isShocked == true)
                    {
                        Instantiate(shockAura, gameObject.transform.position + auraOffset, gameObject.transform.rotation);
                        auraPresent = true;

                        if (collision.transform.GetComponent<EnemyStatus>() != null)
                        {
                            collision.transform.GetComponent<EnemyStatus>().Shocked();
                        }

                        isShocked = false;
                    }

                    if (isFrozen == true)
                    {
                        collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(freezeDamage);

                        if (collision.transform.GetComponent<EnemyStatus>() != null)
                        {
                            collision.transform.GetComponent<EnemyStatus>().Frozen();
                        }

                        isFrozen = false;
                        CancelInvoke("SpawnIce");
                    }
                }
                else
                {
                    StartCoroutine(ResetStatus());
                }
            }
        }
    }

    public IEnumerator StunPotato()
    {
        yield return new WaitForSeconds(1.5f);
        if (hasCollided == false)
        {
            if (minesSpawned == false)
            {
                Instantiate(stunMine, gameObject.transform.position + mineOffset1, Quaternion.Euler(new Vector3(0, 0, 0)));
                Instantiate(stunMine, gameObject.transform.position + mineOffset2, Quaternion.Euler(new Vector3(0, 0, 0)));
                Instantiate(stunMine, gameObject.transform.position + mineOffset3, Quaternion.Euler(new Vector3(0, 0, 0)));
                Instantiate(stunMine, gameObject.transform.position + mineOffset4, Quaternion.Euler(new Vector3(0, 0, 0)));
                minesSpawned = true;
            }
        }
        StartCoroutine(ResetStatus());
    }
    public IEnumerator HotPotato()
    {
        yield return new WaitForSeconds(1.5f);
        if (hasCollided == false)
        {
            Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
        }
        StartCoroutine(ResetStatus());
    }
    public IEnumerator ShockPotato()
    {
        yield return new WaitForSeconds(1.5f);
        if (hasCollided == false)
        {
            Instantiate(shockAura, gameObject.transform.position + auraOffset, Quaternion.Euler(new Vector3(0, 0, 0)));
        }
        auraPresent = true;
        StartCoroutine(ResetStatus());
    }

    public void SpawnIce()
    {
        if (hasCollided == false)
        {
            Instantiate(iceTrail, transform.position + iceOffset, Quaternion.Euler(new Vector3(0, 0, 0)));
            currentIce += 1;
            if (currentIce >= maxIce)
            {
                CancelInvoke("SpawnIce");
            }
        }
    }

    public IEnumerator ResetStatus()
    {
        yield return new WaitForSeconds(5);
        isStunned = false;
        isBurnt = false;
        isShocked = false;
        isFrozen = false;

        auraPresent = false;
        hasCollided = false;
        minesSpawned = false;
        CancelInvoke();
    }
}
