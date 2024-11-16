using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Enemy Actions", menuName = "Enemy Action", order = 0)]
public class EnemyActions : ScriptableObject
{
    public AnimationClip idle;
    public AnimationClip stun;
    public AnimationClip damaged;
}
