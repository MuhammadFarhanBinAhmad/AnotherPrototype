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

    private float startingDecay = 4f; // how long it takes for element to decay
    private float currentDecay;
    private bool dropStacks = false; // element hard drop after bar is filled

    private string newElementType;
    private string currentElementType;

    private bool canFlashBar = true;

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
    private bool canResetState = true;
    private bool firstBullet = true;

    [Header("Effects")]
    private bool canSpawnEffect = true;
    public GameObject stunEffect;
    public GameObject burnEffect;
    public GameObject shockEffect;
    public GameObject freezeEffect;

    [Header("Audio")]
    private AudioSource audioSource;
    private bool canPlayAudio = true;

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

    public Animator statusAnimator;


    // Start is called before the first frame update
    void Start()
    {
        //ResetStacks();
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
        if (dropStacks == true)
        {
            stacks -= Time.deltaTime * 200;
            if (stacks < 0)
            {
                stacks = 0;
                dropStacks = false;
            }
        }
        e_EnemyUI.UpdateEnemyStatus(currentElementType);
    }

    public void AssignElement(string elementType)
    {
        currentDecay = startingDecay;
        currentElementType = elementType;
        StartCoroutine(CheckElement(elementType));
    }

    public IEnumerator CheckElement(string elementType)
    {
        yield return new WaitForSeconds(0.5f);
        newElementType = elementType;

        if (newElementType != currentElementType) // if new element is different
        {
            currentElementType = newElementType;
            ResetStacks();
        }
    }

    public void ResetStacks()
    {
        isStunned = false;
        isBurnt = false;
        isShocked = false;
        isFrozen = false;
        //stacks = 0;
        dropStacks = true;
        canFlashBar = true;

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

    #region Element Status Stuff
    public void Stunned()
    {
        isStunned = true;
        enemyGrab.canGrab = true;
        enemyMovement.StunEnemy();

        if (stunAudio != null && canSpawnEffect == true)
        {
            if (canPlayAudio == true)
            {
                canPlayAudio = false;
                Invoke("ResetAudioBool", 4f);
                audioSource.PlayOneShot(stunAudio);
            }
        }
        if (stunEffect != null && canSpawnEffect == true)
        {
            var effect = Instantiate(stunEffect, gameObject.transform.position, gameObject.transform.rotation);
            effect.transform.parent = gameObject.transform;
            canSpawnEffect = false;
        }
        statusAnimator.SetTrigger("Pulse");

        recoveryTime = stunRecovery;
        if (canResetState == true)
        {
            canResetState = false;
            StartCoroutine(ResetState());
        }
    }
    public void Burnt()
    {
        isBurnt = true;
        enemyGrab.canGrab = true;
        enemyHealth.TakeBurnDamage();

        if (burnAudio != null)
        {
            if (canPlayAudio == true)
            {
                canPlayAudio = false;
                Invoke("ResetAudioBool", 4f);
                audioSource.PlayOneShot(burnAudio);
            }
        }
        if (burnEffect != null && canSpawnEffect == true)
        {
            var effect = Instantiate(burnEffect, gameObject.transform.position, gameObject.transform.rotation);
            effect.transform.parent = gameObject.transform;
            canSpawnEffect = false;
        }
        statusAnimator.SetTrigger("Pulse");

        recoveryTime = burnRecovery;
        if (canResetState == true)
        {
            canResetState = false;
            StartCoroutine(ResetState());
        }
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
            if (canPlayAudio == true)
            {
                canPlayAudio = false;
                Invoke("ResetAudioBool", 4f);
                audioSource.PlayOneShot(shockAudio);
            }
        }
        if (shockEffect != null && canSpawnEffect == true)
        {
            var effect = Instantiate(shockEffect, gameObject.transform.position, gameObject.transform.rotation);
            effect.transform.parent = gameObject.transform;
            canSpawnEffect = false;
        }
        statusAnimator.SetTrigger("Pulse");

        recoveryTime = shockRecovery;
        if (canResetState == true)
        {
            canResetState = false;
            StartCoroutine(ResetState());
        }
    }
    public void Frozen()
    {
        isFrozen = true;
        enemyGrab.canGrab = true;
        enemyMovement.FreezeEnemy();

        if (freezeAudio != null)
        {
            if (canPlayAudio == true)
            {
                canPlayAudio = false;
                Invoke("ResetAudioBool", 4f);
                audioSource.PlayOneShot(freezeAudio);
            }
        }
        if (freezeEffect != null && canSpawnEffect == true)
        {
            var effect = Instantiate(freezeEffect, gameObject.transform.position, gameObject.transform.rotation);
            effect.transform.parent = gameObject.transform;
            canSpawnEffect = false;
        }
        statusAnimator.SetTrigger("Pulse");

        recoveryTime = freezeRecovery;
        if (canResetState == true)
        {
            canResetState = false;
            StartCoroutine(ResetState());
        }
    }
    #endregion

    public IEnumerator ResetState()
    {
        //yield return new WaitForSeconds(recoveryTime - 2.25f);
        //if (canFlashBar == true)
        //{
        //    canFlashBar = false;    
        //    StartCoroutine(e_EnemyUI.FlashStatusBar());
        //}
        //yield return new WaitForSeconds(2.25f);
        yield return new WaitForSeconds(recoveryTime);

        isStunned = false;
        isBurnt = false;
        isShocked = false;
        isFrozen = false;
        enemyGrab.canGrab = false;

        canResetState = true;
        currentDecay = startingDecay;
        firstBullet = true;
        ResetStacks();
    }

    public void ResetAudioBool()
    {
        if (canPlayAudio == false)
        {
            canPlayAudio = true;
        }
    }
}
