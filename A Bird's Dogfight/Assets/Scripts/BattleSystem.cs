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
    public GameObject Shield;
    public GameObject Aura;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public TMP_Text dialogueText;
    public TMP_Text dialogueText2;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    GameObject playerGO;
    GameObject enemyGO;
    public GameObject PlayerHit;
    public GameObject EnemyHit;

    public BattleState state;

    bool defend;
    bool playerDefendedLastTurn;

    bool buff;

    public Button defendButton;

    public AudioClip wingflareAudioClip;
    public AudioClip peckAudioClip;
    public AudioClip barrierAudioClip;
    public AudioClip biteAudioClip;
    public AudioClip shieldhitAudioClip;
    public AudioClip powerupAudioClip;
    public AudioClip headbuttAudioClip;
    AudioSource wingflareSound;
    AudioSource peckSound;
    AudioSource barrierSound;
    AudioSource biteSound;
    AudioSource shieldhitSound;
    AudioSource powerupSound;
    AudioSource headbuttSound;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());

        defendButton.onClick.AddListener(OnDefend);

        wingflareSound = GameObject.Find("wingflaresound").GetComponent<AudioSource>();
        peckSound = GameObject.Find("pecksound").GetComponent<AudioSource>();
        barrierSound = GameObject.Find("barriersound").GetComponent<AudioSource>();
        biteSound = GameObject.Find("bitesound").GetComponent<AudioSource>();
        shieldhitSound = GameObject.Find("shieldhitsound").GetComponent<AudioSource>();
        powerupSound = GameObject.Find("powerupsound").GetComponent<AudioSource>();
    }

    IEnumerator SetupBattle()
    {
        playerGO = Instantiate(playerPrefab);
        playerUnit = playerGO.GetComponent<Unit>();

        enemyGO = Instantiate(enemyPrefab);
        enemyUnit = enemyGO.GetComponent<Unit>();

        PlayerHit = GameObject.Find("PlayerHit");
        EnemyHit = GameObject.Find("EnemyHit");

        playerUnit.criticalChance = 20;
        enemyUnit.criticalChance = 20;

        //playerUnit.missChance = 20;
        enemyUnit.missChance = 20;

        playerUnit.wingAttackAccuracy = 100;
        playerUnit.drillPeckAccuracy = 80;

        dialogueText.text = "A dogfight has begun!"; //enemyUnit.unitName
        dialogueText2.text = "Critical Hit Check =";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        CombatButtons.SetActive(false);
        PlayerArrow.SetActive(false);
        EnemyArrow.SetActive(false);

        defend = false;
        playerDefendedLastTurn = false;
        buff = false;

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

   IEnumerator PlayerAttack()
{
    CombatButtons.SetActive(false);
    playerDefendedLastTurn = false;
    
    Debug.Log("Player critical chance: " + playerUnit.criticalChance);

    // Calculate critical hit chance
    float playerCriticalChance = playerUnit.criticalChance / 100f;

    // Generate random number for critical hit chance
    float randomValue = UnityEngine.Random.value;

    // Generate random number for miss hit chance
    float randomValue2 = UnityEngine.Random.value;

    // Determine whether attack is a critical hit
    bool isCriticalHit = randomValue <= playerCriticalChance;

    dialogueText2.text = "Critical Hit Check = " + UnityEngine.Random.value * 100 + " <= 20";
    Debug.Log("Critical Hit Check = " + UnityEngine.Random.value * 100 + " <= 20");

    Debug.Log("isCriticalHit: " + isCriticalHit);

    playerGO.GetComponent<Animator>().SetTrigger("isWingAttack");

    AudioSource wingflareSound = GetComponent<AudioSource>();
    wingflareSound.clip = wingflareAudioClip;
    wingflareSound.Play();

    PlayerHit.GetComponent<Animator>().SetTrigger("isWing");

    yield return new WaitForSeconds(1f);
    playerGO.GetComponent<Animator>().SetTrigger("isIdle");
    PlayerHit.GetComponent<Animator>().SetTrigger("isNotWing");

    // Determine whether attack is a miss hit
    bool isMissHit = randomValue2 <= (100 - playerUnit.wingAttackAccuracy);

        if(isMissHit ==true) {
            {
                    dialogueText.text = "You missed!";
                    Aura.SetActive(false);
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
            Aura.SetActive(false);
            buff = false;
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
        playerDefendedLastTurn = false;
        
        Debug.Log("Player critical chance: " + playerUnit.criticalChance);

        // Calculate critical hit chance
        float playerCriticalChance = playerUnit.criticalChance / 100f;

        // Generate random number for critical hit chance
        float randomValue = UnityEngine.Random.value;

        // Generate random number for miss hit chance
        float randomValue2 = UnityEngine.Random.value;

        // Determine whether attack is a critical hit
        bool isCriticalHit = randomValue <= playerCriticalChance;

        AudioSource peckSound = GetComponent<AudioSource>();
        peckSound.clip = peckAudioClip;
        peckSound.Play();

        playerGO.GetComponent<Animator>().SetTrigger("isDrillPeck");
        PlayerHit.GetComponent<Animator>().SetTrigger("isHit");
        yield return new WaitForSeconds(1f);
        PlayerHit.GetComponent<Animator>().SetTrigger("isNotHit");
        playerGO.GetComponent<Animator>().SetTrigger("isIdle");

        // Determine whether attack is a miss hit
        bool isMissHit = randomValue2 <= (100 - playerUnit.drillPeckAccuracy) / 100f;

        Debug.Log("isCriticalHit: " + isCriticalHit);

            if(isMissHit ==true) {
                    dialogueText.text = "You missed!";
                    Aura.SetActive(false);
                    state = BattleState.ENEMYTURN;
                    enemyHUD.SetHP(enemyUnit.currentHP);
                    yield return new WaitForSeconds(1f);
                    StartCoroutine(EnemyTurn());
            }else if(buff == true || isCriticalHit == true)
            {
                dialogueText.text = isCriticalHit ? "Critical hit! Your beak drills into enemy!" :  "Your sharpened beak drills into enemy!";
                
                bool isDead = enemyUnit.TakeExtraDamage(playerUnit.drillpeck, isCriticalHit, isMissHit);

                enemyHUD.SetHP(enemyUnit.currentHP);
                Aura.SetActive(false);
                buff = false;
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
        if(playerDefendedLastTurn == false)
        {
            CombatButtons.SetActive(false);
            Shield.SetActive(true);
            
            enemyHUD.SetHP(enemyUnit.currentHP);
            dialogueText.text = "You defend and heal up!";

            AudioSource barrierSound = GetComponent<AudioSource>();
            barrierSound.clip = barrierAudioClip;
            barrierSound.Play();

            playerUnit.Heal(8);
            playerHUD.SetHP(playerUnit.currentHP);
            
            defend = true;
            playerDefendedLastTurn = true;

            state = BattleState.ENEMYTURN;
            enemyHUD.SetHP(enemyUnit.currentHP);
            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyTurn());
        } else {
            dialogueText.text = "Your barrier is recharging!";
            yield return new WaitForSeconds(2f);
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator PlayerBuff()
    {
        CombatButtons.SetActive(false);
        Aura.SetActive(true);

        AudioSource powerupSound = GetComponent<AudioSource>();
        powerupSound.clip = powerupAudioClip;
        powerupSound.Play();
        
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "You power up your next attack!";

        buff = true;
        playerDefendedLastTurn = false;
        
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

        // Determine which attack
        float randomValue3 = UnityEngine.Random.value;
        int ranmdomNumber = (int)(randomValue3 * 10) + 1;

        // Determine whether attack is a critical hit
        bool isCriticalHit = randomValue <= enemyCriticalChance;

        
        if (ranmdomNumber <= 5) {
            enemyGO.GetComponent<Animator>().SetTrigger("isBite");

            AudioSource biteSound = GetComponent<AudioSource>();
            biteSound.clip = biteAudioClip;
            biteSound.Play();

            EnemyHit.GetComponent<Animator>().SetTrigger("isBiten");
            yield return new WaitForSeconds(1f);
            enemyGO.GetComponent<Animator>().SetTrigger("isSit");
            EnemyHit.GetComponent<Animator>().SetTrigger("isNotBiten");

            // Determine whether attack is a miss hit
            bool isMissHit = randomValue2 <= enemyMissChance;

            Debug.Log("isCriticalHit: " + isCriticalHit);

            if(isMissHit == true) {
                
                        dialogueText.text = "Pina missed!";
                        yield return new WaitForSeconds(2f);
                        defend = false;

                        state = BattleState.PLAYERTURN;
                        PlayerTurn();
                }else if(defend == true)
            {
                dialogueText.text = enemyUnit.unitName + " couldn't get through your barrier!";

                AudioSource shieldhitSound = GetComponent<AudioSource>();
                shieldhitSound.clip = shieldhitAudioClip;
                shieldhitSound.Play();

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
        }else if (ranmdomNumber >= 6){
            enemyGO.GetComponent<Animator>().SetTrigger("isHeadbutt");

            AudioSource headbuttSound = GetComponent<AudioSource>();
            headbuttSound.clip = headbuttAudioClip;
            headbuttSound.Play();

            EnemyHit.GetComponent<Animator>().SetTrigger("isHead");
            yield return new WaitForSeconds(1f);
            enemyGO.GetComponent<Animator>().SetTrigger("isSit");
            EnemyHit.GetComponent<Animator>().SetTrigger("isNotHead");

            // Determine whether attack is a miss hit
            bool isMissHit = randomValue2 <= enemyMissChance;

            Debug.Log("isCriticalHit: " + isCriticalHit);

            if(isMissHit == true) {
                
                        dialogueText.text = "Pina missed!";
                        yield return new WaitForSeconds(2f);
                        defend = false;

                        state = BattleState.PLAYERTURN;
                        PlayerTurn();
                }else if(defend == true)
            {
                dialogueText.text = enemyUnit.unitName + " couldn't get through your barrier!";

                AudioSource shieldhitSound = GetComponent<AudioSource>();
                shieldhitSound.clip = shieldhitAudioClip;
                shieldhitSound.Play();

                yield return new WaitForSeconds(1f);

                defend = false;

                yield return new WaitForSeconds(1f);

                state = BattleState.PLAYERTURN;
                PlayerTurn();
            } else if (isCriticalHit == true)
            {
                    yield return new WaitForSeconds(1f);

                    bool isDead = playerUnit.TakeExtraDamage(enemyUnit.headbutt, isCriticalHit, isMissHit);

                    dialogueText.text = "Critical Hit! Pina slams her head right into you harder!";

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

                    bool isDead = playerUnit.TakeDamage(enemyUnit.headbutt, isCriticalHit, isMissHit);

                    dialogueText.text = enemyUnit.unitName + " slams her head into you!";

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
