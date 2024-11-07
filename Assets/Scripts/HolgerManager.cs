using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


public class HolgerManager : MonoBehaviour
{
    public FadeAlpha fadeBlackAlpha;
    public float fadeBlackSpeed;

    public GameObject enemyHolger;
    public GameObject enemySpawnpoint;

    // Start is called before the first frame update
    void Start()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        
        if (activeScene.name == "Holger Scene")
        {
            fadeBlackAlpha.FadeOut(fadeBlackSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            QuitGame();
        }
        if (Input.GetKey(KeyCode.R))
        {
            RespawnHolger();
        }
    }

    public void RespawnHolger()
    {
        Instantiate(enemyHolger, enemySpawnpoint.transform.position, enemySpawnpoint.transform.rotation);
    }

    public  void QuitGame()
    {
        Application.Quit();
    }
}
