using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAudio : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] hitTerrainSounds;
    public AudioClip[] hitEnemySounds;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayHitTerrainSound()
    {
        AudioClip hitSound = hitTerrainSounds[Random.Range(0, hitTerrainSounds.Length)];
        //audioSource.pitch = (Random.Range(0.2f, 0.4f));
        audioSource.PlayOneShot(hitSound);
    }
    public void PlayHitEnemySound()
    {
        AudioClip hitSound = hitEnemySounds[Random.Range(0, hitEnemySounds.Length)];
        //audioSource.pitch = (Random.Range(0.2f, 0.4f));
        audioSource.PlayOneShot(hitSound);
    }
}
