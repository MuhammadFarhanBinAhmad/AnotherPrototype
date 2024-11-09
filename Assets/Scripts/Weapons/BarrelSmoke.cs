using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSmoke : MonoBehaviour
{
    public GameObject smokeParticles;
    public bool canSmoke;
    public float smokeCooldownMax;
    public float smokeCooldownCurrent;
    public bool shotFired;
    public bool initialShot = false;

    // Start is called before the first frame update
    void Start()
    {
        smokeCooldownCurrent = smokeCooldownMax;
        smokeParticles.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (initialShot == true)
        {
            smokeCooldownCurrent -= Time.deltaTime;
        }


        if (shotFired == true)
        {
            initialShot = true;
            shotFired = false;
            smokeParticles.SetActive(false);
            smokeCooldownCurrent = smokeCooldownMax;
        }

        if (smokeCooldownCurrent <= 0)
        {
            smokeParticles.SetActive(true);
        }
    }

    public IEnumerator PlaySmoke()
    {
        yield return new WaitForSeconds(1f);
        if (shotFired == true)
        {
            smokeParticles.SetActive(true);
        }
    }
}
