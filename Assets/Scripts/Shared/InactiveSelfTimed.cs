using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveSelfTimed : MonoBehaviour
{
    public float TimeBeforeInactive;

    private void OnEnable()
    {
        Invoke("InactiveSelf", TimeBeforeInactive);
    }

    void InactiveSelf()
    {
        gameObject.SetActive(false);
    }
}
