using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelPickup : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject pickupAudio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() != null)
        {
            Scene activeScene = SceneManager.GetActiveScene();

            if (activeScene.name == "Level 1")
            {
                winPanel.SetActive(true);
                //SceneManager.LoadScene("Level 2");
            }
            else if (activeScene.name == "Level 2")
            {
                winPanel.SetActive(true);
                //SceneManager.LoadScene("Level 3");
            }


            if (pickupAudio != null)
            {
                Instantiate(pickupAudio, gameObject.transform.position, gameObject.transform.rotation);
            }
            Destroy(transform.parent.gameObject);
        }
    }
}
