using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "My assets/Gameplay/Player Stat", fileName = "New Player Stat")]
public class PlayerStatSO : ScriptableObject
{
    public int maxHP = 0;
    public int maxTotalPoint = 0;

    public int startingAttack = 0;
    public int startingMove = 0;
    public int startingShield = 0;

    void Start()
    {
        if (startingAttack + startingMove + startingShield > maxTotalPoint)
        {
            throw new System.Exception("["  + this.name + "] Total starting point exceeds max total point");
        }
    }
}
