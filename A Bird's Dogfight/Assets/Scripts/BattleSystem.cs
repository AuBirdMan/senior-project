using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, PLAYERATTACKED}

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public GameObject CombatButtons;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public TMP_Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    bool defend;

    bool buff;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = "A dogfight has begun!"; //enemyUnit.unitName

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        CombatButtons.SetActive(false);

        defend = false;
        buff = false;

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

   IEnumerator PlayerAttack()
{
    // Calculate whether the player's attack is a critical hit
    bool isCriticalHit = UnityEngine.Random.value <= playerUnit.criticalChance;

        if(buff == true)
        {
            bool isDead = enemyUnit.TakeExtraDamage(playerUnit.wingattack, isCriticalHit);

            enemyHUD.SetHP(enemyUnit.currentHP);
            dialogueText.text = "Your strengthened wings smash right into the enemy!";
            
            if(isDead)
            {
                state = BattleState.WON;
                enemyHUD.SetHP(enemyUnit.currentHP = 0);
                EndBattle();
            } else
                {
                    state = BattleState.ENEMYTURN;
                    enemyHUD.SetHP(enemyUnit.currentHP);
                    yield return new WaitForSeconds(2f);
                    StartCoroutine(EnemyTurn());
                }
        } else {
            bool isDead = enemyUnit.TakeDamage(playerUnit.wingattack, isCriticalHit);

            enemyHUD.SetHP(enemyUnit.currentHP);
            dialogueText.text = isCriticalHit ? "Critical hit! Your wings smash right into the enemy!" : "Your wings smash right into the enemy!";
            
            if(isDead)
            {
                state = BattleState.WON;
                enemyHUD.SetHP(enemyUnit.currentHP = 0);
                EndBattle();
            } else
                {
                    state = BattleState.ENEMYTURN;
                    enemyHUD.SetHP(enemyUnit.currentHP);
                    yield return new WaitForSeconds(2f);
                    StartCoroutine(EnemyTurn());
                }
        }
}

    IEnumerator PlayerAttack2()
    {
        // Calculate whether the player's attack is a critical hit
        bool isCriticalHit = UnityEngine.Random.value <= playerUnit.criticalChance;

            if(buff == true)
            {
                bool isDead = enemyUnit.TakeExtraDamage(playerUnit.wingattack, isCriticalHit);

                enemyHUD.SetHP(enemyUnit.currentHP);
                dialogueText.text = "Your sharpened beak drills into enemy!";
                
                if(isDead)
                {
                    state = BattleState.WON;
                    enemyHUD.SetHP(enemyUnit.currentHP = 0);
                    EndBattle();
                } else
                    {
                        state = BattleState.ENEMYTURN;
                        enemyHUD.SetHP(enemyUnit.currentHP);
                        yield return new WaitForSeconds(2f);
                        StartCoroutine(EnemyTurn());
                    }
            } else {
                bool isDead = enemyUnit.TakeDamage(playerUnit.wingattack, isCriticalHit);

                enemyHUD.SetHP(enemyUnit.currentHP);
                dialogueText.text = isCriticalHit ? "Critical hit! Your beak drills into enemy!" : "Your beak drills into enemy!";
                
                if(isDead)
                {
                    state = BattleState.WON;
                    enemyHUD.SetHP(enemyUnit.currentHP = 0);
                    EndBattle();
                } else
                    {
                        state = BattleState.ENEMYTURN;
                        enemyHUD.SetHP(enemyUnit.currentHP);
                        yield return new WaitForSeconds(2f);
                        StartCoroutine(EnemyTurn());
                    }
            }
    }

    IEnumerator PlayerDefend()
    {
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "You defend!";
        
        defend = true;

        state = BattleState.ENEMYTURN;
        enemyHUD.SetHP(enemyUnit.currentHP);
        yield return new WaitForSeconds(2f);
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerBuff()
    {
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "You power up your next attack!";

        buff = true;
        
        state = BattleState.ENEMYTURN;
        enemyHUD.SetHP(enemyUnit.currentHP);
        yield return new WaitForSeconds(2f);
        StartCoroutine(EnemyTurn());
    }

    void EndBattle()
    {
        if(state == BattleState.WON)
            {
                dialogueText.text = "You've defeated Pina!";
            } else if (state == BattleState.LOST)
                {
                    dialogueText.text = "You were devoured.";
                }
    }

    IEnumerator EnemyTurn()
    {
        CombatButtons.SetActive(false);

         bool isCriticalHit = UnityEngine.Random.value <= enemyUnit.criticalChance;

        if(defend == true)
        {
            dialogueText.text = enemyUnit.unitName + " couldn't get through your barrier!";

            yield return new WaitForSeconds(1f);

            defend = false;

            yield return new WaitForSeconds(1f);

            state = BattleState.PLAYERTURN;
            PlayerTurn();
        } else {

            yield return new WaitForSeconds(1f);

            bool isDead = playerUnit.TakeDamage(enemyUnit.damage, isCriticalHit);

            dialogueText.text = isCriticalHit ? "Critical Hit! Pina digs her fangs into you!" : enemyUnit.unitName + " digs her fangs into you!";

            playerHUD.SetHP(playerUnit.currentHP);

            defend = false;

            yield return new WaitForSeconds(1f);

            if(isDead)
            {
                state = BattleState.LOST;
                EndBattle();
            }else
                {
                    state = BattleState.PLAYERTURN;
                    PlayerTurn();
                }
    }
        }

    void PlayerTurn()
    {
        CombatButtons.SetActive(true);
        dialogueText.text = "Choose an action:";
    }

    public void OnWingAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        return;

        StartCoroutine(PlayerAttack());
    }

    public void OnDrillPeckButton()
    {
        if (state != BattleState.PLAYERTURN)
        return;

        StartCoroutine(PlayerAttack2());
    }

    public void OnDefend()
    {
        if (state != BattleState.PLAYERTURN)
        return;

        StartCoroutine(PlayerDefend());
    }

    public void OnBuff()
    {
        if (state != BattleState.PLAYERTURN)
        return;

        StartCoroutine(PlayerBuff());
    }
}
