using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
   
    public Text nameText;
    public Slider hpSlider;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
         if (hp <= 0)
        {
            hpSlider.value = 0;
            Vector3 newScale = hpSlider.transform.localScale;
            newScale.x = 0f;
            hpSlider.transform.localScale = newScale;
        }
    }

}
