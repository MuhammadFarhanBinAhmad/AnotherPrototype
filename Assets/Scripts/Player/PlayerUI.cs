using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerUI : MonoBehaviour
{

    PlayerHealth s_PlayerHealth;
    PlayerWeaponManager s_PlayerWeaponManager;
    PlayerSkills s_PlayerSkills;

    [Header("HealthUI")]
    public Image i_Healthbar;

    [Header("WeaponUI")]
    public TextMeshProUGUI t_CurrAmmoLeft;
    public TextMeshProUGUI t_WeaponName;
    public TextMeshProUGUI t_CurrGrenadeRemaining;
    [Header("EXPUI")]
    public Image i_EXP;
    public TextMeshProUGUI t_EXP;
    public TextMeshProUGUI t_CurrentLevel;

    [Header("SlowDownUI")]
    public Image i_SlowDownImage;

    [Header("KickDoorUI")]
    public TextMeshProUGUI t_KickDoor;

    public TextMeshProUGUI t_PickUpWeapon;

    private void Awake()
    {
        s_PlayerHealth = FindObjectOfType<PlayerHealth>();
        s_PlayerWeaponManager = FindObjectOfType<PlayerWeaponManager>();
        s_PlayerSkills = FindObjectOfType<PlayerSkills>();
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
        t_CurrAmmoLeft.text = "Ammo:" + s_PlayerWeaponManager.p_TotalAmmo; 
    }
    public void SlowDownUI()
    {
        i_SlowDownImage.fillAmount = s_PlayerWeaponManager.currSlowDownTime / s_PlayerWeaponManager.maxSlowDownTime;
    }

    public void KickDoorUI(string text)
    {
        t_KickDoor.text = text;
    }
    public void UpdateGrenadeUI()
    {
/*        t_CurrGrenadeRemaining.text = "x" + s_PlayerWeaponManager.p_GrenadeRemaining;
*/    }
    public void UpdateEXP()
    {
        t_EXP.text = s_PlayerSkills.totalExpPoint.ToString() + '/' + s_PlayerSkills.nextPointToLevelUp.ToString();
        i_EXP.fillAmount = (float)s_PlayerSkills.totalExpPoint / (float)s_PlayerSkills.nextPointToLevelUp;
        print((float)s_PlayerSkills.totalExpPoint / (float)s_PlayerSkills.nextPointToLevelUp);
        t_CurrentLevel.text = "Level: " + s_PlayerSkills.currentLevel.ToString();
    }
}
