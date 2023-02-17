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

    public int criticalChance;
    
    public bool TakeDamage(int dmg, bool criticalHit) {
        {
            Debug.Log("criticalChance: " + criticalChance);
            int random = UnityEngine.Random.Range(0, 101);
            Debug.Log("Random number: " + random);

            if (random < criticalChance)

            {
                currentHP -= dmg*2;

                Debug.Log("Damage dealt: " + dmg);

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
