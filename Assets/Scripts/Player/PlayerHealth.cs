using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    PlayerUI s_PlayerUI;

    public int p_Health;
    public int p_MaxHealth;

    private bool godMode = false;
    private int o_Health;


    public FadeAlpha hurtFadeAlpha;
    private float hurtFlashDelay = 2f;
    private float hurtFlashCurrent;
    public float hurtFlashSpeed;
    public float hurtFlashDuration;

    private bool hurtFlashBlocker = true;

    public Animator cameraAnimator;
    public FadeAlpha deathFadeAlpha;
    public float deathFlashSpeed;
    public FadeAlpha blackFadeAlpha;
    public float blackFlashSpeed;
    public PlayerCamera playerCamera;
    public GameObject playerArms;


    private void Start()
    {
        s_PlayerUI = FindObjectOfType<PlayerUI>();
        p_Health = p_MaxHealth;

        o_Health = p_Health;
        hurtFlashCurrent = hurtFlashDelay;
    }
    public void TakeDamage(int dmg)
    {
        FindObjectOfType<CameraShake>().Shake(.2f, .1f, .1f);
        {
            if (p_Health > 0)
            {
                p_Health -= dmg;

                if (hurtFlashCurrent <= 0)
                {
                    hurtFlashCurrent = hurtFlashDelay;
                    StartCoroutine(HurtFlash());
                }
            }
            else if (p_Health <= 0)
            {
                StartCoroutine(Die());
            }
        }

        s_PlayerUI.UpdateHealthUI();
    }

    public void Heal(int health)
    {
        if (p_Health > 0 && p_Health < p_MaxHealth)
        {
            p_Health += health;
        }
        s_PlayerUI.UpdateHealthUI();
    }
    public IEnumerator HurtFlash()
    {
        hurtFadeAlpha.FadeIn(hurtFlashSpeed);
        yield return new WaitForSeconds(hurtFlashDuration);
        hurtFadeAlpha.FadeOut(hurtFlashSpeed);
    }
    public IEnumerator Die()
    {
        playerCamera.enabled = false;
        playerArms.SetActive(false);
        cameraAnimator.SetTrigger("Dead");
        deathFadeAlpha.FadeIn(deathFlashSpeed);
        yield return new WaitForSeconds(3);
        blackFadeAlpha.FadeIn(blackFlashSpeed);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Update()
    {
        hurtFlashCurrent -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.G))
        {
            GodMode();
        }
    }

    public void GodMode()
    {
        if (godMode == false)
        {
            p_Health = 10000;
            godMode = true;
        }
        else
        {
            p_Health = o_Health;
            godMode = false;
        }
    }
}
