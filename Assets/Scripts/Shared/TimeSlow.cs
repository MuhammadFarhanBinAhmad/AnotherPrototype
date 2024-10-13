using System.IO;
using UnityEngine;

public class TimeSlow : MonoBehaviour
{
    private float slowdownDuration = 1f;
    [SerializeField]
    private GameManager gameManager;


    void Update()
    {
        if (gameManager.GetComponent<GameManager>().gamePaused == false)
        {
            Time.timeScale += (1f / slowdownDuration) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        }
    }

    public void DoSlowMotion(float slowFactor, float slowDuration)
    {
        slowdownDuration = slowDuration;
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
}
