using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    PlayerUI s_PlayerUI;

    public int p_Health;
    public int p_MaxHealth;

    private bool godMode = false;
    private int o_Health;


    private void Start()
    {
        s_PlayerUI = FindObjectOfType<PlayerUI>();
        p_Health = p_MaxHealth;

        o_Health = p_Health;
    }
    public void TakeDamage(int dmg)
    {
        FindObjectOfType<CameraShake>().Shake(.2f, .1f, .1f);
        {
            if (p_Health > 0)
            {
                p_Health -= dmg;
            }
            else if (p_Health <= 0)
            {
                Die();
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
    public void Die()
    {
        //Destroy(gameObject);
    }

    public void Update()
    {
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
