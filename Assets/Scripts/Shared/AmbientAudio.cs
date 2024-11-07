using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] ambientSounds;

    public float timeInterval;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        CalculateAudioInterval();
    }

    // Update is called once per frame
    void Update()
    {
        timeInterval -= Time.deltaTime;

        if (timeInterval <= 0)
        {
            PlaySound();
            CalculateAudioInterval();
        }

    }

    public void CalculateAudioInterval()
    {
        timeInterval = Random.Range(10, 30);
    }

    void PlaySound()
    {
        AudioClip ambientSound = ambientSounds[Random.Range(0, ambientSounds.Length)];
        audioSource.pitch = (Random.Range(0.6f, 1f));
        audioSource.PlayOneShot(ambientSound);
    }
}
