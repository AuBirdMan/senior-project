using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, PLAYERATTACKED}

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public GameObject CombatButtons;
    public GameObject PlayerArrow;
    public GameObject EnemyArrow;
    public GameObject Player;
    public GameObject Shield;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public TMP_Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    GameObject playerGO;
    GameObject enemyGO;
    GameObject PlayerHit;
    GameObject EnemyHit;

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
        playerGO = Instantiate(playerPrefab);
        playerUnit = playerGO.GetComponent<Unit>();

        enemyGO = Instantiate(enemyPrefab);
        enemyUnit = enemyGO.GetComponent<Unit>();

        PlayerHit = GameObject.Find("PlayerHit");
        EnemyHit = GameObject.Find("EnemyHit");

        playerUnit.criticalChance = 30;
        enemyUnit.criticalChance = 30;

        playerUnit.missChance = 20;
        enemyUnit.missChance = 20;

        dialogueText.text = "A dogfight has begun!"; //enemyUnit.unitName

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        CombatButtons.SetActive(false);
        PlayerArrow.SetActive(false);
        EnemyArrow.SetActive(false);

        defend = false;
        buff = false;

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

   IEnumerator PlayerAttack()
{
    CombatButtons.SetActive(false);
    
    Debug.Log("Player critical chance: " + playerUnit.criticalChance);

    // Calculate critical hit chance
    float playerCriticalChance = playerUnit.criticalChance / 100f;

    // Calculate miss hit chance
    float playerMissChance = playerUnit.missChance / 100f;

    // Generate random number for critical hit chance
    float randomValue = UnityEngine.Random.value;

    // Generate random number for miss hit chance
    float randomValue2 = UnityEngine.Random.value;

    // Determine whether attack is a critical hit
    bool isCriticalHit = randomValue <= playerCriticalChance;

    Debug.Log("isCriticalHit: " + isCriticalHit);

    playerGO.GetComponent<Animator>().SetTrigger("isWingAttack");
    PlayerHit.GetComponent<Animator>().SetTrigger("isWing");
    yield return new WaitForSeconds(1f);
    playerGO.GetComponent<Animator>().SetTrigger("isIdle");
    PlayerHit.GetComponent<Animator>().SetTrigger("isNotWing");

    // Determine whether attack is a miss hit
    bool isMissHit = randomValue2 <= playerMissChance;

        if(isMissHit ==true) {
            {
                    dialogueText.text = "You missed!";
                    
                    state = BattleState.ENEMYTURN;
                    enemyHUD.SetHP(enemyUnit.currentHP);
                    yield return new WaitForSeconds(1f);
                    StartCoroutine(EnemyTurn());
            }
        }else if(buff == true || isCriticalHit == true)
        {
            dialogueText.text = isCriticalHit ? "Critical hit! Your wings smash right into the enemy!" :  "Your strengthened wings smash right into the enemy!";
            
            bool isDead = enemyUnit.TakeExtraDamage(playerUnit.wingattack, isCriticalHit, isMissHit);

            enemyHUD.SetHP(enemyUnit.currentHP);
            yield return new WaitForSeconds(1f);
            
            if(isDead)
            {
                state = BattleState.WON;
                enemyHUD.SetHP(enemyUnit.currentHP = 0);
                EndBattle();
                yield return new WaitForSeconds(2f);
                SceneManager.LoadScene("You Win!");
            } else
                {
                    state = BattleState.ENEMYTURN;
                    enemyHUD.SetHP(enemyUnit.currentHP);
                    yield return new WaitForSeconds(1f);
                    StartCoroutine(EnemyTurn());
                }
        } else {
            
            dialogueText.text = "Your wings smash right into the enemy!";
            
            bool isDead = enemyUnit.TakeDamage(playerUnit.wingattack, isCriticalHit, isMissHit);

            enemyHUD.SetHP(enemyUnit.currentHP);
            yield return new WaitForSeconds(1f);

            if(isDead)
            {
                state = BattleState.WON;
                enemyHUD.SetHP(enemyUnit.currentHP = 0);
                EndBattle();
                yield return new WaitForSeconds(2f);
                SceneManager.LoadScene("You Win!");
            } else
                {
                    state = BattleState.ENEMYTURN;
                    enemyHUD.SetHP(enemyUnit.currentHP);
                    yield return new WaitForSeconds(1f);
                    StartCoroutine(EnemyTurn());
                }
        }
}

    IEnumerator PlayerAttack2()
    {
        CombatButtons.SetActive(false);
        
        Debug.Log("Player critical chance: " + playerUnit.criticalChance);

        // Calculate critical hit chance
        float playerCriticalChance = playerUnit.criticalChance / 100f;

        // Calculate miss hit chance
        float playerMissChance = playerUnit.missChance / 100f;

        // Generate random number for critical hit chance
        float randomValue = UnityEngine.Random.value;

        // Generate random number for miss hit chance
        float randomValue2 = UnityEngine.Random.value;

        // Determine whether attack is a critical hit
        bool isCriticalHit = randomValue <= playerCriticalChance;

        playerGO.GetComponent<Animator>().SetTrigger("isDrillPeck");
        PlayerHit.GetComponent<Animator>().SetTrigger("isHit");
        yield return new WaitForSeconds(1f);
        PlayerHit.GetComponent<Animator>().SetTrigger("isNotHit");
        playerGO.GetComponent<Animator>().SetTrigger("isIdle");

        // Determine whether attack is a miss hit
        bool isMissHit = randomValue2 <= playerMissChance;

        Debug.Log("isCriticalHit: " + isCriticalHit);

            if(isMissHit ==true) {
                    dialogueText.text = "You missed!";

                    state = BattleState.ENEMYTURN;
                    enemyHUD.SetHP(enemyUnit.currentHP);
                    yield return new WaitForSeconds(1f);
                    StartCoroutine(EnemyTurn());
            }else if(buff == true || isCriticalHit == true)
            {
                dialogueText.text = isCriticalHit ? "Critical hit! Your beak drills into enemy!" :  "Your sharpened beak drills into enemy!";
                
                bool isDead = enemyUnit.TakeExtraDamage(playerUnit.drillpeck, isCriticalHit, isMissHit);

                enemyHUD.SetHP(enemyUnit.currentHP);
                yield return new WaitForSeconds(1f);

                if(isDead)
                {
                    state = BattleState.WON;
                    enemyHUD.SetHP(enemyUnit.currentHP = 0);
                    EndBattle();
                    yield return new WaitForSeconds(2f);
                    SceneManager.LoadScene("You Win!");
                } else
                    {
                        state = BattleState.ENEMYTURN;
                        enemyHUD.SetHP(enemyUnit.currentHP);
                        yield return new WaitForSeconds(1f);
                        StartCoroutine(EnemyTurn());
                    }
            } else {
                dialogueText.text = "Your beak drills into enemy!";
                bool isDead = enemyUnit.TakeDamage(playerUnit.drillpeck, isCriticalHit, isMissHit);

                enemyHUD.SetHP(enemyUnit.currentHP);
                yield return new WaitForSeconds(1f);

                if(isDead)
                {
                    state = BattleState.WON;
                    enemyHUD.SetHP(enemyUnit.currentHP = 0);
                    EndBattle();
                    yield return new WaitForSeconds(2f);
                    SceneManager.LoadScene("You Win!");
                } else
                    {
                        state = BattleState.ENEMYTURN;
                        enemyHUD.SetHP(enemyUnit.currentHP);
                        yield return new WaitForSeconds(1f);
                        StartCoroutine(EnemyTurn());
                    }
            }
}

    IEnumerator PlayerDefend()
    {
        CombatButtons.SetActive(false);
        Shield.SetActive(true);
        
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
        CombatButtons.SetActive(false);
        
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
        PlayerArrow.SetActive(false);
        EnemyArrow.SetActive(true);

        Debug.Log("Enemy critical chance: " + enemyUnit.criticalChance);

        // Calculate critical hit chance
        float enemyCriticalChance = enemyUnit.criticalChance / 100f;

        // Calculate miss hit chance
        float enemyMissChance = enemyUnit.missChance / 100f;

        // Generate random number for critical hit chance
        float randomValue = UnityEngine.Random.value;

        // Generate random number for miss hit chance
        float randomValue2 = UnityEngine.Random.value;

        // Determine whether attack is a critical hit
        bool isCriticalHit = randomValue <= enemyCriticalChance;

        enemyGO.GetComponent<Animator>().SetTrigger("isBite");
        EnemyHit.GetComponent<Animator>().SetTrigger("isBiten");
        yield return new WaitForSeconds(1f);
        enemyGO.GetComponent<Animator>().SetTrigger("isSit");
        EnemyHit.GetComponent<Animator>().SetTrigger("isNotBiten");

        // Determine whether attack is a miss hit
        bool isMissHit = randomValue2 <= enemyMissChance;

        Debug.Log("isCriticalHit: " + isCriticalHit);

        if(isMissHit ==true) {
            
                    dialogueText.text = "Pina missed!";
                    yield return new WaitForSeconds(2f);

                    state = BattleState.PLAYERTURN;
                    PlayerTurn();
            }else if(defend == true)
        {
            dialogueText.text = enemyUnit.unitName + " couldn't get through your barrier!";

            yield return new WaitForSeconds(1f);

            defend = false;

            yield return new WaitForSeconds(1f);

            state = BattleState.PLAYERTURN;
            PlayerTurn();
        } else if (isCriticalHit == true)
        {
            yield return new WaitForSeconds(1f);

            bool isDead = playerUnit.TakeExtraDamage(enemyUnit.bite, isCriticalHit, isMissHit);

            dialogueText.text = "Critical Hit! Pina digs her fangs into you deeper!";

            playerHUD.SetHP(playerUnit.currentHP);

            defend = false;

            yield return new WaitForSeconds(2f);

            if (playerUnit.currentHP <= 0) {
            playerGO.GetComponent<Animator>().SetTrigger("isDead");
            yield return new WaitForSeconds(2f);
}

            if(isDead)
            {
                state = BattleState.LOST;
                EndBattle();
                yield return new WaitForSeconds(2f);
                SceneManager.LoadScene("Game Over");
            }else
                {
                    state = BattleState.PLAYERTURN;
                    PlayerTurn();
                }
        }else {

            yield return new WaitForSeconds(1f);

            bool isDead = playerUnit.TakeDamage(enemyUnit.bite, isCriticalHit, isMissHit);

            dialogueText.text = enemyUnit.unitName + " digs her fangs into you!";

            playerHUD.SetHP(playerUnit.currentHP);

            defend = false;

            yield return new WaitForSeconds(2f);

            if(isDead)
            {
                state = BattleState.LOST;
                EndBattle();
                yield return new WaitForSeconds(2f);
                SceneManager.LoadScene("Game Over");
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
        PlayerArrow.SetActive(true);
        EnemyArrow.SetActive(false);
        Shield.SetActive(false);
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
