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
    public GameObject optionsPanel;
    public GameObject keybindsPanel;
    public GameObject creditsPanel;

    public Animator animator;
    public FadeAlpha fadeBlackAlpha;
    public float fadeBlackSpeed;

    public Animator menuAnimator;
    public GameObject buttonChirp;

    public static GameManager instance { get; private set; }


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than 1 Game Manager in scene!");
        }
        instance = this;

        //DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
        if (keybindsPanel != null)
        {
            keybindsPanel.SetActive(false);
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
            optionsPanel.SetActive(false);
            keybindsPanel.SetActive(false);
            creditsPanel.SetActive(false);

            if (pauseMenu.activeSelf == false) // pause game
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                gamePaused = true;
                PlayButtonChirp();
            }
            else
            {
                pauseMenu.SetActive(false); // resume game
                Time.timeScale = 1.0f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                gamePaused = false;
                PlayButtonChirp();
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
            if (optionsPanel != null)
            {
                optionsPanel.SetActive(false);
            }
            if (keybindsPanel != null)
            {
                keybindsPanel.SetActive(false);
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

    #region Display Panels
    public void DisplayOptions()
    {
        if (optionsPanel.activeSelf == false)
        {
            optionsPanel.SetActive(true);
        }
        else
        {
            optionsPanel.SetActive(false);
        }
        keybindsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }
    public void DisplayKeybinds()
    {
        if (keybindsPanel.activeSelf == false)
        {
            keybindsPanel.SetActive(true);
        }
        else
        {
            keybindsPanel.SetActive(false);
        }
        optionsPanel.SetActive(false);
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
        optionsPanel.SetActive(false);
        keybindsPanel.SetActive(false);
    }
    #endregion

    public void LoadLevel1()
    {
        fadeBlackAlpha.FadeIn(fadeBlackSpeed);
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
        if (keybindsPanel != null)
        {
            keybindsPanel.SetActive(false);
        }
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
        StartCoroutine(Level1Delay());
    }
    public IEnumerator Level1Delay()
    {
        menuAnimator.SetTrigger("Play");
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

    public void PlayButtonChirp()
    {
        Instantiate(buttonChirp, transform.position, transform.rotation);
    }

    public  void QuitGame()
    {
        Application.Quit();
    }
}
