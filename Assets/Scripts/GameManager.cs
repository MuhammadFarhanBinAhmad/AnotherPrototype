using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool gamePaused = false;
    public GameObject mainMenu;
    public GameObject controlsPanel;
    public GameObject creditsPanel;

    public Animator animator;
    public FadeAlpha fadeBlackAlpha;
    public float fadeBlackSpeed;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                go.AddComponent<GameManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(false);
        }
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
        animator = gameObject.GetComponent<Animator>();


        Scene activeScene = SceneManager.GetActiveScene();
        
        if (activeScene.name == "Level 1")
        {
            fadeBlackAlpha.FadeOut(fadeBlackSpeed);
        }
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
            //RestartGame();
            RestartScene();
        }

        if (Input.GetKey(KeyCode.F1))
        {
            SceneManager.LoadScene("Level 1");
        }
        if (Input.GetKey(KeyCode.F2))
        {
            SceneManager.LoadScene("Level 2");
        }
        if (Input.GetKey(KeyCode.F3))
        {
            SceneManager.LoadScene("Level 3");
        }
        if (Input.GetKey(KeyCode.F4))
        {
            LoadArtGym();
        }
    }

    public void PauseGame()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        if (activeScene.name != "Main Menu")
        {
            if (pauseMenu.activeSelf == false) // pause game
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                gamePaused = true;
            }
            else
            {
                pauseMenu.SetActive(false); // resume game
                Time.timeScale = 1.0f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                gamePaused = false;
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Level 1"); // replace with initial scene name from level manager
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        if (activeScene.name == "Main Menu")
        {
            if (controlsPanel != null)
            {
                controlsPanel.SetActive(false);
            }
            if (creditsPanel != null)
            {
                creditsPanel.SetActive(false);
            }
        }
        else
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    public void DisplayControls()
    {
        if (controlsPanel.activeSelf == false)
        {
            controlsPanel.SetActive(true);
        }
        else
        {
            controlsPanel.SetActive(false);
        }
        creditsPanel.SetActive(false);
    }

    public void DisplayCredits()
    {
        if (creditsPanel.activeSelf == false)
        {
            creditsPanel.SetActive(true);
        }
        else
        {
            creditsPanel.SetActive(false);
        }
        controlsPanel.SetActive(false);
    }

    public void LoadLevel1()
    {
        fadeBlackAlpha.FadeIn(fadeBlackSpeed);
        StartCoroutine(Level1Delay());
    }
    public IEnumerator Level1Delay()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Level 1");
    }

    public void LoadArtGym()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        if (activeScene.name == "Art Gym")
        {
            SceneManager.LoadScene("Level 1");
        }
        else
        {
            SceneManager.LoadScene("Art Gym");
        }
    }

    public  void QuitGame()
    {
        Application.Quit();
    }
}
