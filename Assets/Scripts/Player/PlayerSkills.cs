using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    PlayerUI s_PlayerUI;
    public PlayerFist playerFist;

    public int keyCount = 0;

    //public int currentLevel;
    //public int totalExpPoint;
    //public int nextPointToLevelUp;


    private void Start()
    {
        s_PlayerUI = FindObjectOfType<PlayerUI>();
    }

    public void BigFist(bool canBigFist)
    {
        if (canBigFist == true)
        {
            playerFist.canBigFist = true;
        }
    }

    public void AddKey(int key)
    {
        keyCount++;
    }
    public void RemoveKey(int key)
    {
        keyCount--;
    }

    //public void AddExp(int exp)
    //{
    //    totalExpPoint += exp;
    //    if (totalExpPoint >= nextPointToLevelUp)
    //    {
    //        currentLevel++;
    //        nextPointToLevelUp *= 2;
    //        totalExpPoint = 0;
    //    }

    //    s_PlayerUI.UpdateEXP();
    //}
}
