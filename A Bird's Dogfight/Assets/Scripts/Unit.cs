using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{

    public string unitName;

    public int wingattack;
    public int drillpeck;
    public int bite;
    public int headbutt;

    public int maxHP;
    public int currentHP;

    public float criticalChance;
    public float missChance;
    
    public bool TakeDamage(int dmg, bool criticalHit, bool missHit) {
        {
            currentHP -= dmg;

                if(currentHP <= 0)
                    return true;
                    else
                    return false;

        }
    }

    public bool TakeExtraDamage(int dmg, bool criticalHit, bool missHit) {
        {
            currentHP -= dmg*2;

            if(currentHP <= 0)
                return true;
            else
                return false;
        }
    }
}
