using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;
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
            LoadArtGym();
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
        SceneManager.LoadScene("Level 1"); // replace with initial scene name from level manager
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadArtGym()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        if (activeScene.name == "ArtGym")
        {
            SceneManager.LoadScene("Level 1");
        }
        else
        {
            SceneManager.LoadScene("ArtGym");
        }
    }

    public  void QuitGame()
    {
        Application.Quit();
    }
}
