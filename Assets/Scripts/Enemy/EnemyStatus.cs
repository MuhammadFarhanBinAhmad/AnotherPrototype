using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [Header ("Stacks")]
    public float stacks;
    public float maxStacks;

    public float startingDecay;
    public float currentDecay;

    private string newElementType;
    private string currentElementType;

    [Header("Checks")]
    public bool isStunned = false;
    public bool isBurnt = false;
    public bool isShocked = false;
    public bool isFrozen = false;
    private float recoveryTime;
    public float stunRecovery;
    public float burnRecovery;
    public float shockRecovery;
    public float freezeRecovery;
    private bool firstBullet = true;

    [Header("Effects")]
    private bool canSpawnEffect = true;
    public GameObject stunEffect;
    public GameObject burnEffect;
    public GameObject shockEffect;
    public GameObject freezeEffect;

    [Header("Audio")]
    public AudioSource audioSource;

    public AudioClip stunAudio;
    public AudioClip burnAudio;
    public AudioClip shockAudio;
    public AudioClip freezeAudio;

    [Header("Conditions")]
    public EnemyUI e_EnemyUI;
    public EnemyGrab enemyGrab;

    public EnemyHealth enemyHealth;
    public EnemyMovement enemyMovement;
    public EnemyMeleeAttackBehaviour enemyMeleeAttackBehaviour;
    public EnemyRangeAttackBehaviour enemyRangeAttackBehaviour;


    // Start is called before the first frame update
    void Start()
    {
        ResetStacks();
        enemyGrab = gameObject.GetComponent<EnemyGrab>();
        enemyHealth = gameObject.GetComponent<EnemyHealth>();
        enemyMovement = gameObject.GetComponent<EnemyMovement>();
        enemyMeleeAttackBehaviour = gameObject.GetComponent<EnemyMeleeAttackBehaviour>();
        enemyRangeAttackBehaviour = gameObject.GetComponent<EnemyRangeAttackBehaviour>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void TakeStacks(float stackAmount)
    {
        stacks += stackAmount;
        currentDecay = startingDecay;
    }

    // Update is called once per frame
    void Update()
    {
        if (stacks <= 0)
        {
            stacks = 0;
        }
        if (stacks >= maxStacks)
        {
            stacks = maxStacks;
            EnemyVulnerable(currentElementType);
        }

        if (stacks >= 1)
        {
            currentDecay -= Time.deltaTime;
            if (currentDecay <= 0)
            {
                stacks = Mathf.Lerp(stacks, 0, Time.deltaTime * 1f);
            }
        }
        e_EnemyUI.UpdateEnemyStatus(currentElementType);
    }

    public void AssignElement(string elementType)
    {
        currentElementType = elementType;
        StartCoroutine(CheckElement(elementType));
    }

    public IEnumerator CheckElement(string elementType)
    {
        yield return new WaitForSeconds(0.5f);
        newElementType = elementType;

        if (newElementType != currentElementType) // if new element is different
        {
            ResetStacks();
            currentElementType = newElementType;
        }
    }

    public void ResetStacks()
    {
        isStunned = false;
        isBurnt = false;
        isShocked = false;
        isFrozen = false;
        stacks = 0;
        currentDecay = startingDecay;
    }

    public void EnemyVulnerable(string element)
    {
        if (element.Contains("Stun"))
        {
            Stunned();
        }
        if (element.Contains("Burn"))
        {
            Burnt();
        }
        if (element.Contains("Shock"))
        {
            Shocked();
        }
        if (element.Contains("Freeze"))
        {
            Frozen();
        }
    }

    public void Stunned()
    {
        isStunned = true;
        enemyGrab.canGrab = true;
        enemyMovement.StunEnemy();

        if (stunAudio != null && canSpawnEffect == true)
        {
            audioSource.PlayOneShot(stunAudio);
        }
        if (stunEffect != null && canSpawnEffect == true)
        {
            var effect = Instantiate(stunEffect, gameObject.transform.position, gameObject.transform.rotation);
            effect.transform.parent = gameObject.transform;
            canSpawnEffect = false;
        }

        recoveryTime = stunRecovery;
        StartCoroutine(ResetState());
    }
    public void Burnt()
    {
        isBurnt = true;
        enemyGrab.canGrab = true;
        enemyHealth.TakeBurnDamage();

        if (burnAudio != null)
        {
            audioSource.PlayOneShot(burnAudio);
        }
        if (burnEffect != null && canSpawnEffect == true)
        {
            var effect = Instantiate(burnEffect, gameObject.transform.position, gameObject.transform.rotation);
            effect.transform.parent = gameObject.transform;
            canSpawnEffect = false;
        }

        recoveryTime = burnRecovery;
        StartCoroutine(ResetState());
    }
    public void Shocked()
    {
        isShocked = true;
        enemyGrab.canGrab = true;
        if (enemyMeleeAttackBehaviour != null)
        {
            enemyMeleeAttackBehaviour.ShockEnemy();
        }
        if (enemyRangeAttackBehaviour != null)
        {
            enemyRangeAttackBehaviour.ShockEnemy();
        }

        if (shockAudio != null)
        {
            audioSource.PlayOneShot(shockAudio);
        }
        if (shockEffect != null && canSpawnEffect == true)
        {
            var effect = Instantiate(shockEffect, gameObject.transform.position, gameObject.transform.rotation);
            effect.transform.parent = gameObject.transform;
            canSpawnEffect = false;
        }

        recoveryTime = shockRecovery;
        StartCoroutine(ResetState());
    }
    public void Frozen()
    {
        isFrozen = true;
        enemyGrab.canGrab = true;
        enemyMovement.FreezeEnemy();

        if (freezeAudio != null)
        {
            audioSource.PlayOneShot(freezeAudio);
        }
        if (freezeEffect != null && canSpawnEffect == true)
        {
            var effect = Instantiate(freezeEffect, gameObject.transform.position, gameObject.transform.rotation);
            effect.transform.parent = gameObject.transform;
            canSpawnEffect = false;
        }

        recoveryTime = freezeRecovery;
        StartCoroutine(ResetState());
    }

    public IEnumerator ResetState()
    {
        yield return new WaitForSeconds(recoveryTime);

        isStunned = false;
        isBurnt = false;
        isShocked = false;
        isFrozen = false;
        enemyGrab.canGrab = false;

        currentDecay = startingDecay;
        firstBullet = true;
        ResetStacks();
    }
}
