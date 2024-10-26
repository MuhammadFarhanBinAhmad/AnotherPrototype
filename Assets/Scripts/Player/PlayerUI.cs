using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

public class PlayerUI : MonoBehaviour
{
    PlayerHealth s_PlayerHealth;
    PlayerWeaponManager s_PlayerWeaponManager;
    PlayerSkills s_PlayerSkills;

    [Header("HealthUI")]
    public Image i_Healthbar;
    public Image i_WhiteHealthbar;
    private float lerpSpeed = 0.05f;

    [Header("WeaponUI")]
    public TextMeshProUGUI t_WeaponName;
    public TextMeshProUGUI t_CurrAmmoLeft;
    public TextMeshProUGUI t_WeaponElement;
    public TextMeshProUGUI t_CurrGrenadeRemaining;
    public bool blankAmmo = true;

    [Header("UIText")]
    public Image i_SlowDownImage;
    public TextMeshProUGUI t_KickDoor;
    public TextMeshProUGUI t_PickUpWeapon;

    [Header("FollowBehaviour")]
    public GameObject HUDCanvas;
    public Transform cam;
    //public Vector3 startPos;
    //public Vector3 endPos;
    public Transform anchor;


    [Header("Colours")]
    public Color stunColour;
    public Color burnColour;
    public Color shockColour;
    public Color freezeColour;

    private void Awake()
    {
        s_PlayerHealth = FindObjectOfType<PlayerHealth>();
        s_PlayerWeaponManager = FindObjectOfType<PlayerWeaponManager>();
        s_PlayerSkills = FindObjectOfType<PlayerSkills>();
        cam = Camera.main.transform;
        //startPos = transform.position;
        //endPos = anchor.position;
    }
    public void UpdateHealthUI()
    {
        i_Healthbar.fillAmount = (float)(float)s_PlayerHealth.p_Health/(float)(s_PlayerHealth.p_MaxHealth);
    }
    public void PickUpWeaponUI(string name)
    {
        if (name != null)
        t_PickUpWeapon.text = "Press 'E' to pick up " + name + '?';
        else
        t_PickUpWeapon.text = null;

    }
    public void UpdateWeaponUI()
    {
        t_WeaponName.text = s_PlayerWeaponManager.p_WeaponName;
        if (blankAmmo == true)
        {
            t_CurrAmmoLeft.text = "";
        }
        else
        {
            t_CurrAmmoLeft.text = "" + s_PlayerWeaponManager.p_TotalAmmo;
        }

        t_WeaponElement.text = s_PlayerWeaponManager.p_ProjectileElement;

        if (s_PlayerWeaponManager.WeaponEquipped == true)
        {
            UpdateTextColour();
        }
    }

    public void UpdateTextColour()
    {
        if (s_PlayerWeaponManager.p_ProjectileElement.Contains("Stun"))
        {
            t_WeaponElement.color = stunColour;
        }
        if (s_PlayerWeaponManager.p_ProjectileElement.Contains("Shock"))
        {
            t_WeaponElement.color = shockColour;
        }
        if (s_PlayerWeaponManager.p_ProjectileElement.Contains("Freeze"))
        {
            t_WeaponElement.color = freezeColour;
        }
        if (s_PlayerWeaponManager.p_ProjectileElement.Contains("Burn"))
        {
            t_WeaponElement.color = burnColour;
        }
    }


    public void KickDoorUI(string text)
    {
        t_KickDoor.text = text;
    }

    public void Update()
    {
        if (anchor != null)
        {
            transform.position = Vector3.MoveTowards(anchor.transform.position, anchor.position, 0.1f);
        }
        //transform.position = Vector3.Lerp(startPos, endPos, lerpSpeed);
        gameObject.transform.LookAt(transform.position + cam.forward);
    }

    private void LateUpdate()
    {
        if (i_Healthbar.fillAmount != i_WhiteHealthbar.fillAmount)
        {
            i_WhiteHealthbar.fillAmount = Mathf.Lerp(i_WhiteHealthbar.fillAmount, i_Healthbar.fillAmount, lerpSpeed);
        }
    }
}
