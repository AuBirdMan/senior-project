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

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.wingattack);

        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "Your wings smash right into the enemy!";

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

    IEnumerator PlayerAttack2()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.drillpeck);

        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "Your beak drills into enemy!";
        
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
        
        dialogueText.text = enemyUnit.unitName + " digs her fangs into you!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);

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


}
