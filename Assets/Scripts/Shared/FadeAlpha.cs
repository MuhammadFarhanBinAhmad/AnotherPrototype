using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeAlpha : MonoBehaviour
{
    [SerializeField] private CanvasGroup myUIGroup;
    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;

    public void Start()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        if (activeScene.name != "Main Menu")
        {
            myUIGroup.alpha = 1;
            FadeOut();
        }
    }

    public void FadeIn()
    {
        fadeIn = true;
    }
    public void FadeOut()
    {
        fadeOut = true;
    }

    private void Update()
    {
        if (fadeIn)
        {
            if (myUIGroup.alpha < 1)
            {
                myUIGroup.alpha += Time.deltaTime;
                if (myUIGroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }

        if (fadeOut)
        {
            if (myUIGroup.alpha >= 0)
            {
                myUIGroup.alpha -= Time.deltaTime;
                if (myUIGroup.alpha == 0)
                {
                    fadeOut = false;
                }
            }
        }
    }
}
