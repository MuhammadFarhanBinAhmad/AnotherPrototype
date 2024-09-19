using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyUI : MonoBehaviour
{
    public Image e_HealthUI;
    EnemyHealth e_Health;

    public Transform cam;

    private void Start()
    {
        e_Health = transform.parent.GetComponent<EnemyHealth>();
        e_Health.e_EnemyUI = this;

        cam = Camera.main.transform;
    }
    public void UpdateEnemyUI()
    {
        e_HealthUI.fillAmount = (float)((float)e_Health.health / (float)e_Health.maxHealth);
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
