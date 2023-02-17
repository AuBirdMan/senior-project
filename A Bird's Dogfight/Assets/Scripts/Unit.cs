using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{

    public string unitName;

    public int wingattack;
    public int drillpeck;

    public int damage;

    public int maxHP;
    public int currentHP;

    public float criticalChance = 10.0f;
    
    public bool TakeDamage(int dmg, bool criticalHit) {
        {
            int random = UnityEngine.Random.Range(0, 101);

            if (random < criticalChance)

            {
                currentHP -= dmg*2;

                if(currentHP <= 0)
                    return true;
                else
                    return false;
            } else {
                currentHP -= dmg;

                if(currentHP <= 0)
                    return true;
                else
                    return false;
            }
        }
    }

    public bool TakeExtraDamage(int dmg, bool criticalHit) {
        {
            currentHP -= dmg*2;

            if(currentHP <= 0)
                return true;
            else
                return false;
        }
    }
}
