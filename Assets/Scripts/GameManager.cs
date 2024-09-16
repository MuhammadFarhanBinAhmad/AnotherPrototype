using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.P))
        {
            PauseGame();
        }
        if (Input.GetKey(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void PauseGame()
    {
        if (pauseMenu.activeSelf == false) // pause game
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenu.SetActive(false); // resume game
            Time.timeScale = 1.0f;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("WeaponGameplayScene"); // replace with initial scene name from level manager
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public  void QuitGame()
    {
        Application.Quit();
    }
}
