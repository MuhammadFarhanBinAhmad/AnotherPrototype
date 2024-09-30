using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public Image e_HealthUI;
    EnemyHealth e_Health;

    public Image e_StatusUI;
    EnemyStatus e_Status;

    public Color stunColour = new Color(175f, 255f, 0f, 255f);
    public Color burnColour = new Color(255f, 150f, 0f, 255f);
    public Color shockColour = new Color(200f, 0f, 255f, 255f);
    public Color freezeColour = new Color (0f, 255f, 255f, 255f);

    public Transform cam;

    private void Start()
    {
        e_Health = transform.parent.GetComponent<EnemyHealth>();
        e_Health.e_EnemyUI = this;

        e_Status = transform.parent.GetComponent<EnemyStatus>();
        e_Status.e_EnemyUI = this;

        cam = Camera.main.transform;
    }
    public void UpdateEnemyHealth()
    {
        e_HealthUI.fillAmount = (float)((float)e_Health.health / (float)e_Health.maxHealth);
    }

    public void UpdateEnemyStatus(string currentElementType)
    {
        e_StatusUI.fillAmount = e_Status.stacks / e_Status.maxStacks;

        if (currentElementType == null)
        {
            return;
        }
        else if (currentElementType.Contains("Stun"))
        {
            e_StatusUI.color = stunColour;
        }
        else if (currentElementType.Contains("Burn"))
        {
            e_StatusUI.color = burnColour;
        }
        else if (currentElementType.Contains("Shock"))
        {
            e_StatusUI.color = shockColour;
        }
        else if (currentElementType.Contains("Freeze"))
        {
            e_StatusUI.color = freezeColour;
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
