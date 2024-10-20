using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeAlpha : MonoBehaviour
{
    [SerializeField] private CanvasGroup myUIGroup;
    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;

    private float fadeSpeed = 1;

    public void Start()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        if (activeScene.name != "Main Menu" && myUIGroup.name == "BlackPanel")
        {
            myUIGroup.alpha = 1;
            FadeOut(1);
        }
    }

    public void FadeIn(float fadeInSpeed)
    {
        fadeSpeed = fadeInSpeed;
        fadeIn = true;
    }
    public void FadeOut(float fadeOutSpeed)
    {
        fadeSpeed = fadeOutSpeed;
        fadeOut = true;
    }

    private void Update()
    {
        if (fadeIn)
        {
            if (myUIGroup.alpha < 1)
            {
                myUIGroup.alpha += Time.deltaTime * fadeSpeed;
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
                myUIGroup.alpha -= Time.deltaTime * fadeSpeed;
                if (myUIGroup.alpha == 0)
                {
                    fadeOut = false;
                }
            }
        }
    }
}
