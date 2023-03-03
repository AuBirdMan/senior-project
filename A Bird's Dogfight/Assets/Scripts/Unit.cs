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

    public int wingAttackAccuracy;
    public int drillPeckAccuracy;
    
    public bool TakeDamage(int dmg, bool isCriticalHit, bool isMissHit)
{
    currentHP -= dmg;
    currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    
    // Check if the unit is dead
    if (currentHP == 0)
    {
        StartCoroutine(FadeOutSprite());

        return true;
    }

    return false;
}

    public bool TakeExtraDamage(int dmg, bool criticalHit, bool missHit) 
    {
        currentHP -= dmg*2;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    
    // Check if the unit is dead
    if (currentHP == 0)
    {
        StartCoroutine(FadeOutSprite());

        return true;
    }

    return false;
}

    IEnumerator FadeOutSprite()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        // Fade out the sprite over 1 second
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            // Calculate the alpha value based on the elapsed time
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / 1f);

            // Set the sprite's color with the new alpha value
            Color newColor = renderer.color;
            newColor.a = alpha;
            renderer.color = newColor;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    public void Heal (int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }
}
