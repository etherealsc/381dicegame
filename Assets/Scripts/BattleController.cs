using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class BattleController : MonoBehaviour
{
    public List<GameObject> playerDice = new List<GameObject>();
    public List<GameObject> enemyDice = new List<GameObject>();
    public float throwForce;

    private void Awake()
    {
        pDicePrefab = GameManager.instance.playerDicePrefab;
        eDicePrefab = GameManager.instance.enemyDicePrefab;
    }
    private void Start()
    {
        ResetBoard();
        battleState = BattleState.WAITING;
        roundCounter = 0;
    }

    public GameObject pDicePrefab;
    public GameObject eDicePrefab;
    public List<Transform> playerDicePlacement = new List<Transform>();
    public List<Transform> enemyDicePlacement = new List<Transform>();
    public void ResetBoard()
    {
        //foreach (var item in playerDice)
        //{
        //    Destroy(item);
        //}
        //foreach (var item in enemyDice)
        //{
        //    Destroy(item);
        //}
        foreach (var item in deadDice)
        {
            Destroy(item);
        }

        playerDice.Clear();
        enemyDice.Clear();
        deadDice.Clear();

        //if (roundCounter != 12)
        //{
            for (int i = 0; i < 5; i++)
            {
                var playerDicePrefab = Instantiate(pDicePrefab, playerDicePlacement[i], false);
                playerDice.Add(playerDicePrefab);
                playerDicePrefab.GetComponent<Dice>().icon = playerHandIcons[i];
                playerHandIcons[i].gameObject.GetComponent<DiceIcon>().playerDice = playerDicePrefab;
                playerHandIcons[i].gameObject.GetComponent<DiceIcon>().Assign();

                var enemyDicePrefab = Instantiate(eDicePrefab, enemyDicePlacement[i]);
                enemyDice.Add(enemyDicePrefab);
                enemyDicePrefab.tag = "EnemyDice";
                enemyDicePrefab.GetComponent<Dice>().icon = enemyHandIcons[i];

            }
        //}

        arenaPanel.GetComponent<SpriteRenderer>().sprite = arenas[Random.Range(0, arenas.Count)];
    }
    public GameObject arenaPanel;
    public List<Sprite> arenas = new List<Sprite>();


        public void RollDice()
    {
        for (int i = 0; i < playerDice.Count; i++)
        {
            //if the die isn't Locked
            if (!playerDice[i].GetComponent<Dice>().locked)
            {
                var rb = playerDice[i].GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * throwForce);
                rb.AddTorque(Random.Range(-2f, 2f), ForceMode2D.Impulse);
            }
        }
    }

    public void EnemyRoll()
    {
        for (int i = 0; i < enemyDice.Count; i++)
        {
            var rb = enemyDice[i].GetComponent<Rigidbody2D>();
            enemyDice[i].GetComponent<Dice>().locked = false;
            rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * throwForce);
            rb.AddTorque(Random.Range(-2f, 2f), ForceMode2D.Impulse);
        }

        int cheat = Random.Range(0, 100000);
        Debug.Log(cheat);
        if (cheat <= 2300)
        {
            int dice = Random.Range(0, 6);
            for (int i = 0; i < enemyDice.Count; i++)
            {
                enemyDice[i].GetComponent<Dice>().cheatDiceValue = dice;
            }
        }
        else if (cheat <= 15000)
        {
            int dice = Random.Range(0, 6);
            int dice2 = Random.Range(0, 6);

            if (dice != dice2)
            {
                enemyDice[0].GetComponent<Dice>().cheatDiceValue = dice2;
                enemyDice[1].GetComponent<Dice>().cheatDiceValue = dice;
                enemyDice[2].GetComponent<Dice>().cheatDiceValue = dice;
                enemyDice[3].GetComponent<Dice>().cheatDiceValue = dice;
                enemyDice[4].GetComponent<Dice>().cheatDiceValue = dice;

            }
        }
        else if (cheat <= 37500)
        {
            int dice = Random.Range(0, 6);
            int dice2 = Random.Range(0, 6);

            if (dice != dice2)
            {
                enemyDice[0].GetComponent<Dice>().cheatDiceValue = dice2;
                enemyDice[1].GetComponent<Dice>().cheatDiceValue = dice2;
                enemyDice[2].GetComponent<Dice>().cheatDiceValue = dice;
                enemyDice[3].GetComponent<Dice>().cheatDiceValue = dice;
                enemyDice[4].GetComponent<Dice>().cheatDiceValue = dice;

            }

        }
        else if (cheat <= 50000)
        {
            int dice = Random.Range(0, 6);

            enemyDice[0].GetComponent<Dice>().cheatDiceValue = dice;
            enemyDice[1].GetComponent<Dice>().cheatDiceValue = 2;
            enemyDice[2].GetComponent<Dice>().cheatDiceValue = 4;
            enemyDice[3].GetComponent<Dice>().cheatDiceValue = 3;
            enemyDice[4].GetComponent<Dice>().cheatDiceValue = dice;
        }
        else
        {
            enemyDice[0].GetComponent<Dice>().cheatDiceValue = -1;
            enemyDice[1].GetComponent<Dice>().cheatDiceValue = -1;
            enemyDice[2].GetComponent<Dice>().cheatDiceValue = -1;
            enemyDice[3].GetComponent<Dice>().cheatDiceValue = -1;
            enemyDice[4].GetComponent<Dice>().cheatDiceValue = -1;
        }
            //add code to make the AI cheat
        }
    



    public enum BattleState
    {
        FIRSTROLL,
        SECONDROLL,
        THIRDROLL,
        WAITING,
        FIRSTLOCK,
        SECONDLOCK,
        PREBATTLE,
        BATTLING,
        POSTCOMBAT
    }

    public BattleState battleState;


    public GameObject rollButton;
    public GameObject roll2Button;
    public GameObject roll3Button;

    public Toggle toggle1;
    public Toggle toggle2;
    public Toggle toggle3;
    public Toggle toggle4;
    public Toggle toggle5;

    public GameObject playerScorePanel;
    public GameObject enemyScorePanel;

    public List<GameObject> survivingPlayerDice = new List<GameObject>();
    public List<GameObject> survivingEnemyDice = new List<GameObject>();
    public List<GameObject> deadDice = new List<GameObject>();


    public List<Image> playerHandIcons = new List<Image>();
    public List<Image> enemyHandIcons = new List<Image>();

    public Transform dicePurgatory;
    void Update()
    {
        switch(battleState)
        {
            case BattleState.WAITING:
                rollButton.SetActive(true);
                roll2Button.SetActive(false);
                roll3Button.SetActive(false);
                toggle1.interactable = false;
                toggle2.interactable = false;
                toggle3.interactable = false;
                toggle4.interactable = false;
                toggle5.interactable = false;
                playerScorePanel.SetActive(false);
                enemyScorePanel.SetActive(false);
                rollButton.GetComponent<Button>().interactable = true;
                UnlockEverything();

                break;
            case BattleState.FIRSTROLL:

                rollButton.SetActive(false);
                if(DiceNotMoving())
                {
                    battleState = BattleState.FIRSTLOCK;
                }


                break;
            case (BattleState.SECONDROLL):
                roll2Button.SetActive(false);

                if (DiceNotMoving())
                {
                    battleState = BattleState.SECONDLOCK;
                }
                break;
            case (BattleState.THIRDROLL):
                roll3Button.SetActive(false);

                if (DiceNotMoving())
                {
                    //play animation for moving the scoreboards onscreen
                    CalculateScores();
                    playerScorePanel.SetActive(true);
                    enemyScorePanel.SetActive(true);
                    battleState = BattleState.PREBATTLE;
                }
                break;

            case (BattleState.FIRSTLOCK):
                toggle1.interactable = true;
                toggle2.interactable = true;
                toggle3.interactable = true;
                toggle4.interactable = true;
                toggle5.interactable = true;
                roll2Button.SetActive(true);
                break;
            case (BattleState.SECONDLOCK):
                toggle1.interactable = true;
                toggle2.interactable = true;
                toggle3.interactable = true;
                toggle4.interactable = true;
                toggle5.interactable = true;
                roll3Button.SetActive(true);

                break;
            case (BattleState.PREBATTLE):
                toggle1.interactable = false;
                toggle2.interactable = false;
                toggle3.interactable = false;
                toggle4.interactable = false;
                toggle5.interactable = false;
                toggle1.isOn = false;
                toggle2.isOn = false;
                toggle3.isOn = false;
                toggle4.isOn = false;
                toggle5.isOn = false;
                UnlockEverything();

                Debug.Log("We preppin");

                break;

            case (BattleState.BATTLING):
                Debug.Log("We fightin");
                playerScorePanel.SetActive(false);
                enemyScorePanel.SetActive(false);
                roundTimerNumber -= Time.deltaTime;
                roundTimer.text = Mathf.CeilToInt(roundTimerNumber).ToString();
                if (playerDice.Count == 0)
                {
                    //add function to save living enemy dice for final battle
                    for (int i = 0; i < enemyDice.Count; i++)
                    {
                        survivingEnemyDice.Add(enemyDice[i]);
                        enemyDice[i].GetComponent<Dice>().diceClass = Dice.Class.NONE;
                        enemyDice[i].transform.position = dicePurgatory.transform.position;
                        enemyDice[i].GetComponent<Dice>().icon = null;
                    }

                    battleState = BattleState.POSTCOMBAT;
                }

                if (enemyDice.Count == 0)
                {
                    //add function to save living player dice for final battle
                    for (int i = 0; i < playerDice.Count; i++)
                    {
                        survivingPlayerDice.Add(playerDice[i]);
                        playerDice[i].GetComponent<Dice>().diceClass = Dice.Class.NONE;
                        playerDice[i].transform.position = dicePurgatory.transform.position;
                        playerDice[i].GetComponent<Dice>().icon = null;

                    }

                    battleState = BattleState.POSTCOMBAT;
                }

                if(roundTimerNumber <= 0f)
                {
                    for (int i = 0; i < enemyDice.Count; i++)
                    {
                        survivingEnemyDice.Add(enemyDice[i]);
                        enemyDice[i].GetComponent<Dice>().diceClass = Dice.Class.NONE;
                        enemyDice[i].transform.position = dicePurgatory.transform.position;
                        enemyDice[i].GetComponent<Dice>().icon = null;

                    }
                    for (int i = 0; i < playerDice.Count; i++)
                    {
                        survivingPlayerDice.Add(playerDice[i]);
                        playerDice[i].GetComponent<Dice>().diceClass = Dice.Class.NONE;
                        playerDice[i].transform.position = dicePurgatory.transform.position;
                        playerDice[i].GetComponent<Dice>().icon = null;

                    }
                    battleState = BattleState.POSTCOMBAT;

                }
                break;

            case (BattleState.POSTCOMBAT):
                roundTimer.text = "";
                if(roundCounter == 14)
                {
                    playerScorePanel.SetActive(true);
                    enemyScorePanel.SetActive(true);
                }
                else
                {
                    ResetBoard();
                    roundCounter += 1;
                    if (roundCounter == 13)
                    {
                        FinalShowdown();

                    }
                    else
                    {
                        battleState = BattleState.WAITING;
                    }
                }
                



                break;
        }
    }
    public int roundCounter;
    public Button mainMenuButton;
    public void FinalShowdown()
    {
        for (int i = 0; i < survivingPlayerDice.Count; i++)
        {
            survivingPlayerDice[i].transform.position = diceHeaven.transform.position;
            survivingPlayerDice[i].GetComponent<Dice>().diceClass = survivingPlayerDice[i].GetComponent<Dice>().originalClass;
            playerDice.Add(survivingPlayerDice[i]);
        }
        for (int i = 0; i < survivingEnemyDice.Count; i++)
        {
            survivingEnemyDice[i].transform.position = diceHell.transform.position;
            survivingEnemyDice[i].GetComponent<Dice>().diceClass = survivingEnemyDice[i].GetComponent<Dice>().originalClass;
            enemyDice.Add(survivingEnemyDice[i]);
        }
        roundTimerNumber = 999f;
        battleState = BattleState.BATTLING;
    }

    public Transform diceHeaven;
    public Transform diceHell;

    public TextMeshProUGUI roundTimer;
    public float roundTimerNumber;
    //public bool AllPlayersDead()
    //{
    //    for (int i = 0; i < playerDice.Count; i++)
    //    {
    //        if(playerDice[i].GetComponent<Dice>().hp > 0)
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    //public bool AllEnemiesDead()
    //{
    //    for (int i = 0; i < enemyDice.Count; i++)
    //    {
    //        if (enemyDice[i].GetComponent<Dice>().hp > 0)
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    public void UnlockEverything()
    {
        for (int i = 0; i < playerDice.Count; i++)
        {
            playerDice[i].GetComponent<Dice>().locked = false;
        }
        for (int i = 0; i < enemyDice.Count; i++)
        {
            enemyDice[i].GetComponent<Dice>().locked = false;
        }
    }

    public void LockEverything()
    {
        for (int i = 0; i < playerDice.Count; i++)
        {
            playerDice[i].GetComponent<Dice>().locked = true;
        }
        for (int i = 0; i < enemyDice.Count; i++)
        {
            enemyDice[i].GetComponent<Dice>().locked = true;
        }
    }

    public float delayTimer;
    public void FirstRoll()
    {
        RollDice();
        EnemyRoll();
        toggle1.interactable = false;
        toggle2.interactable = false;
        toggle3.interactable = false;
        toggle4.interactable = false;
        toggle5.interactable = false;
        delayTimer = 0.5f;
        battleState = BattleState.FIRSTROLL;
    }

    public void SecondRoll()
    {
        RollDice();
        toggle1.interactable = false;
        toggle2.interactable = false;
        toggle3.interactable = false;
        toggle4.interactable = false;
        toggle5.interactable = false;
        delayTimer = 0.5f;

        battleState = BattleState.SECONDROLL;
    }
    public void ThirdRoll()
    {
        RollDice();
        toggle1.interactable = false;
        toggle2.interactable = false;
        toggle3.interactable = false;
        toggle4.interactable = false;
        toggle5.interactable = false;
        delayTimer = 0.5f;

        battleState = BattleState.THIRDROLL;
    }


    public bool DiceNotMoving()
    {
        for (int i = 0; i < playerDice.Count; i++)
        {
            if (!playerDice[i].GetComponent<Dice>().finishedRolling)
            {
                return false;
            }
        }
        for (int i = 0; i < enemyDice.Count; i++)
        {
            if (!enemyDice[i].GetComponent<Dice>().finishedRolling)
            {
                return false;
            }
        }

        delayTimer -= Time.deltaTime;
        if (delayTimer <= 0f) //this is just so the next stage doesnt trigger immediately, as the first frame technically has all the dice at 0 velocity
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    
    public TextMeshProUGUI enemyscore1;
    public TextMeshProUGUI enemyscore2;
    public TextMeshProUGUI enemyscore3;
    public TextMeshProUGUI enemyscore4;
    public TextMeshProUGUI enemyscore5;
    public TextMeshProUGUI enemyscore6;
    public TextMeshProUGUI enemyscore7;
    public TextMeshProUGUI enemyscore8;
    public TextMeshProUGUI enemyscore9;
    public TextMeshProUGUI enemyscore10;
    public TextMeshProUGUI enemyscore11;
    public TextMeshProUGUI enemyscore12;
    public TextMeshProUGUI enemyscore13;
    public TextMeshProUGUI totalEnemyScore;

    public TextMeshProUGUI playerscore1;
    public TextMeshProUGUI playerscore2;
    public TextMeshProUGUI playerscore3;
    public TextMeshProUGUI playerscore4;
    public TextMeshProUGUI playerscore5;
    public TextMeshProUGUI playerscore6;
    public TextMeshProUGUI playerscore7;
    public TextMeshProUGUI playerscore8;
    public TextMeshProUGUI playerscore9;
    public TextMeshProUGUI playerscore10;
    public TextMeshProUGUI playerscore11;
    public TextMeshProUGUI playerscore12;
    public TextMeshProUGUI playerscore13;
    public TextMeshProUGUI totalPlayerScore;

    public TextMeshProUGUI realplayerscore1;
    public TextMeshProUGUI realplayerscore2;
    public TextMeshProUGUI realplayerscore3;
    public TextMeshProUGUI realplayerscore4;
    public TextMeshProUGUI realplayerscore5;
    public TextMeshProUGUI realplayerscore6;
    public TextMeshProUGUI realplayerscore7;
    public TextMeshProUGUI realplayerscore8;
    public TextMeshProUGUI realplayerscore9;
    public TextMeshProUGUI realplayerscore10;
    public TextMeshProUGUI realplayerscore11;
    public TextMeshProUGUI realplayerscore12;
    public TextMeshProUGUI realplayerscore13;
    

    public int enemySelectedScore;
    public int playerSelectedScore;

    public List<int> playerHand = new List<int>();
    public List<int> enemyHand = new List<int>();

    public void CalculateScores()
    {
        enemySelectedScore = 0;
        playerSelectedScore = 0;
        int currentEnemyScore = 0;
        playerHand.Clear();
        enemyHand.Clear();
        Dictionary<TextMeshProUGUI, int> enemyScorePicker = new Dictionary<TextMeshProUGUI, int>();
        for (int i = 0; i < playerDice.Count; i++)
        {
            playerHand.Add(playerDice[i].GetComponent<Dice>().value + 1);


            if(enemyDice[i].GetComponent<Dice>().cheatDiceValue != -1)
            {
                enemyHand.Add(enemyDice[i].GetComponent<Dice>().cheatDiceValue + 1);
            }
            else
            {
                enemyHand.Add(enemyDice[i].GetComponent<Dice>().value + 1);
            }
        }

        

        //code for Aces
        int pointsPlayer = 0;
        int pointsEnemy = 0;
        for (int i = 0; i < playerDice.Count; i++)
        {
            if (playerHand[i] == 1)
            {
                pointsPlayer += 1;
            }
            if(enemyHand[i] == 1)
            {
                pointsEnemy += 1;
            }
        }
        playerscore1.text = pointsPlayer.ToString();
        if(enemyscore1.color == Color.white)
        {
            enemyscore1.text = pointsEnemy.ToString();
            enemyScorePicker.Add(enemyscore1, pointsEnemy);
        }
        else
        {
            currentEnemyScore += int.Parse(enemyscore1.text);
        }
        Debug.Log("Enemy score: " + pointsEnemy + " and Player score: " + pointsPlayer);

        //code for Twos
        pointsPlayer = 0;
        pointsEnemy = 0;
        for (int i = 0; i < playerDice.Count; i++)
        {
            if (playerHand[i] == 2)
            {
                pointsPlayer += 2;
            }
            if (enemyHand[i] == 2)
            {
                pointsEnemy += 2;
            }
        }
        playerscore2.text = pointsPlayer.ToString();
        if (enemyscore2.color == Color.white)
        {
            enemyscore2.text = pointsEnemy.ToString();
            enemyScorePicker.Add(enemyscore2, pointsEnemy);
        }
        else
        {
            currentEnemyScore += int.Parse(enemyscore2.text);
        }
        Debug.Log("Enemy score: " + pointsEnemy + " and Player score: " + pointsPlayer);

        //code for Threes
        pointsPlayer = 0;
        pointsEnemy = 0;
        for (int i = 0; i < playerDice.Count; i++)
        {
            if (playerHand[i] == 3)
            {
                pointsPlayer += 3;
            }
            if (enemyHand[i] == 3)
            {
                pointsEnemy += 3;
            }
        }
        playerscore3.text = pointsPlayer.ToString();
        if (enemyscore3.color == Color.white)
        {
            enemyscore3.text = pointsEnemy.ToString();
            enemyScorePicker.Add(enemyscore3, pointsEnemy);
        }
        else
        {
            currentEnemyScore += int.Parse(enemyscore3.text);
        }
        Debug.Log("Enemy score: " + pointsEnemy + " and Player score: " + pointsPlayer);

        //code for Fours
        pointsPlayer = 0;
        pointsEnemy = 0;
        for (int i = 0; i < playerDice.Count; i++)
        {
            if (playerHand[i] == 4)
            {
                pointsPlayer += 4;
            }
            if (enemyHand[i] == 4)
            {
                pointsEnemy += 4;
            }
        }
        playerscore4.text = pointsPlayer.ToString();
        if (enemyscore4.color == Color.white)
        {
            enemyscore4.text = pointsEnemy.ToString();
            enemyScorePicker.Add(enemyscore4, pointsEnemy);
        }
        else
        {
            currentEnemyScore += int.Parse(enemyscore4.text);
        }
        Debug.Log("Enemy score: " + pointsEnemy + " and Player score: " + pointsPlayer);

        //code for Fives
        pointsPlayer = 0;
        pointsEnemy = 0;
        for (int i = 0; i < playerDice.Count; i++)
        {
            if (playerHand[i] == 5)
            {
                pointsPlayer += 5;
            }
            if (enemyHand[i] == 5)
            {
                pointsEnemy += 5;
            }
        }
        playerscore5.text = pointsPlayer.ToString();
        if (enemyscore5.color == Color.white)
        {
            enemyscore5.text = pointsEnemy.ToString();
            enemyScorePicker.Add(enemyscore5, pointsEnemy);
        }
        else
        {
            currentEnemyScore += int.Parse(enemyscore5.text);
        }
        Debug.Log("Enemy score: " + pointsEnemy + " and Player score: " + pointsPlayer);

        //code for Sixes
        pointsPlayer = 0;
        pointsEnemy = 0;
        for (int i = 0; i < playerDice.Count; i++)
        {
            if (playerHand[i] == 6)
            {
                pointsPlayer += 6;
            }
            if (enemyHand[i] == 6)
            {
                pointsEnemy += 6;
            }
        }
        playerscore6.text = pointsPlayer.ToString();
        if (enemyscore6.color == Color.white)
        {
            enemyscore6.text = pointsEnemy.ToString();
            enemyScorePicker.Add(enemyscore6, pointsEnemy);
        }
        else
        {
            currentEnemyScore += int.Parse(enemyscore6.text);
        }
        Debug.Log("Enemy score: " + pointsEnemy + " and Player score: " + pointsPlayer);

        //code for Three of a Kind
        pointsPlayer = 0;
        pointsEnemy = 0;
        playerHand.Sort();
        if ((playerHand[0] == playerHand[1] && playerHand[1] == playerHand[2]) || 
            (playerHand[1] == playerHand[2] && playerHand[2] == playerHand[3]) ||
            (playerHand[2] == playerHand[3] && playerHand[3] == playerHand[4]))
        {
            pointsPlayer = playerHand[0] + playerHand[1] + playerHand[2] + playerHand[3] + playerHand[4];
        }
        else
        {
            pointsPlayer = 0;
        }
        enemyHand.Sort();
        if ((enemyHand[0] == enemyHand[1] && enemyHand[1] == enemyHand[2]) ||
            (enemyHand[1] == enemyHand[2] && enemyHand[2] == enemyHand[3]) ||
            (enemyHand[2] == enemyHand[3] && enemyHand[3] == enemyHand[4]))
        {
            pointsEnemy = enemyHand[0] + enemyHand[1] + enemyHand[2] + enemyHand[3] + enemyHand[4];
        }
        else
        {
            pointsEnemy = 0;
        }
        playerscore7.text = pointsPlayer.ToString();
        if (enemyscore7.color == Color.white)
        {
            enemyscore7.text = pointsEnemy.ToString();
            enemyScorePicker.Add(enemyscore7, pointsEnemy);
        }
        else
        {
            currentEnemyScore += int.Parse(enemyscore7.text);
        }
        Debug.Log("Enemy score: " + pointsEnemy + " and Player score: " + pointsPlayer);

        //code for Four of a Kind
        pointsPlayer = 0;
        pointsEnemy = 0;
        playerHand.Sort();
        if ((playerHand[0] == playerHand[1] && playerHand[1] == playerHand[2] && playerHand[2] == playerHand[3]) || (playerHand[1] == playerHand[2] && playerHand[2] == playerHand[3] && playerHand[3] == playerHand[4]))
        {
            pointsPlayer = playerHand[0] + playerHand[1] + playerHand[2] + playerHand[3] + playerHand[4];
        }
        else
        {
            pointsPlayer = 0;
        }
        enemyHand.Sort();
        if ((enemyHand[0] == enemyHand[1] && enemyHand[1]  == enemyHand[2] && enemyHand[2]  == enemyHand[3]) || (enemyHand[1]  == enemyHand[2] && enemyHand[2]  == enemyHand[3] && enemyHand[3]  == enemyHand[4]))
        {
            pointsEnemy = enemyHand[0] + enemyHand[1] + enemyHand[2] + enemyHand[3] + enemyHand[4];
        }
        else
        {
            pointsEnemy = 0;
        }
        playerscore8.text = pointsPlayer.ToString();
        if (enemyscore8.color == Color.white)
        {
            enemyscore8.text = pointsEnemy.ToString();
            enemyScorePicker.Add(enemyscore8, pointsEnemy);
        }
        else
        {
            currentEnemyScore += int.Parse(enemyscore8.text);
        }
        Debug.Log("Enemy score: " + pointsEnemy + " and Player score: " + pointsPlayer);

        //code for Full House
        pointsPlayer = 0;
        pointsEnemy = 0;
        playerHand.Sort();
        if (playerHand[0] == playerHand[1] && playerHand[1] == playerHand[2] && (playerHand[3] == playerHand[4]) ||
             ((playerHand[0] == playerHand[1]) && (playerHand[2] == playerHand[3] && playerHand[3] == playerHand[4])))
        {
            pointsPlayer = 25;
        }
        else
        {
            pointsPlayer = 0;
        }
        enemyHand.Sort();
        if (enemyHand[0] == enemyHand[1] && enemyHand[1] == enemyHand[2] && (enemyHand[3] == enemyHand[4]) ||
             ((enemyHand[0] == enemyHand[1]) && (enemyHand[2] == enemyHand[3] && enemyHand[3] == enemyHand[4])))
        {
            pointsEnemy = 25;
        }
        else
        {
            pointsEnemy = 0;
        }
        playerscore9.text = pointsPlayer.ToString();
        if (enemyscore9.color == Color.white)
        {
            enemyscore9.text = pointsEnemy.ToString();
            enemyScorePicker.Add(enemyscore9, pointsEnemy);
        }
        else
        {
            currentEnemyScore += int.Parse(enemyscore9.text);
        }
        Debug.Log("Enemy score: " + pointsEnemy + " and Player score: " + pointsPlayer);

        //code for Small Straight
        pointsPlayer = 0;
        pointsEnemy = 0;
        playerHand.Sort();
        if((playerHand.Contains(1) && playerHand.Contains(2) && playerHand.Contains(3) && playerHand.Contains(4)) || 
           (playerHand.Contains(2) && playerHand.Contains(3) && playerHand.Contains(4) && playerHand.Contains(5)) ||
           (playerHand.Contains(3) && playerHand.Contains(4) && playerHand.Contains(5) && playerHand.Contains(6)))
        //if ((playerHand[0] + 1 == playerHand[1] && playerHand[1] + 1 == playerHand[2] && playerHand[2] + 1 == playerHand[3]) || (playerHand[1] + 1 == playerHand[2] && playerHand[2] + 1 == playerHand[3] && playerHand[3] + 1 == playerHand[4]))
        {
            pointsPlayer = 30;
        }
        else
        {
            pointsPlayer = 0;
        }
        enemyHand.Sort();
        if((playerHand.Contains(1) && playerHand.Contains(2) && playerHand.Contains(3) && playerHand.Contains(4) && playerHand.Contains(5)) ||
            (playerHand.Contains(2) && playerHand.Contains(3) && playerHand.Contains(4) && playerHand.Contains(5) && playerHand.Contains(6)))
        //if ((enemyHand[0] + 1 == enemyHand[1] && enemyHand[1] + 1 == enemyHand[2] && enemyHand[2] + 1 == enemyHand[3]) || (enemyHand[1] + 1 == enemyHand[2] && enemyHand[2] + 1 == enemyHand[3] && enemyHand[3] + 1 == enemyHand[4]))
        {
            pointsEnemy = 30;
        }
        else
        {
            pointsEnemy = 0;
        }
        playerscore10.text = pointsPlayer.ToString();
        if (enemyscore10.color == Color.white)
        {
            enemyscore10.text = pointsEnemy.ToString();
            enemyScorePicker.Add(enemyscore10, pointsEnemy);
        }
        else
        {
            currentEnemyScore += int.Parse(enemyscore10.text);
        }       
        Debug.Log("Enemy score: " + pointsEnemy + " and Player score: " + pointsPlayer);

        //code for Large Straight
        pointsPlayer = 0;
        pointsEnemy = 0;
        playerHand.Sort();
        if (playerHand[0]+1 == playerHand[1] && playerHand[1]+1 == playerHand[2] && playerHand[2]+1 == playerHand[3] && playerHand[3]+1 == playerHand[4])
        {
            pointsPlayer = 40;
        }
        else
        {
            pointsPlayer = 0;
        }
        enemyHand.Sort();
        if (enemyHand[0] + 1 == enemyHand[1] && enemyHand[1] + 1 == enemyHand[2] && enemyHand[2] + 1 == enemyHand[3] && enemyHand[3] + 1 == enemyHand[4])
        {
            pointsEnemy = 40;
        }
        else
        {
            pointsEnemy = 0;
        }
        playerscore11.text = pointsPlayer.ToString();
        if (enemyscore11.color == Color.white)
        {
            enemyscore11.text = pointsEnemy.ToString();
            enemyScorePicker.Add(enemyscore11, pointsEnemy);
        }
        else
        {
            currentEnemyScore += int.Parse(enemyscore11.text);
        }
        Debug.Log("Enemy score: " + pointsEnemy + " and Player score: " + pointsPlayer);

        //code for Yahtzee
        pointsPlayer = 0;
        pointsEnemy = 0;

        if (playerHand[0] == playerHand[1] && playerHand[0] == playerHand[2] && playerHand[0] == playerHand[3] && playerHand[0] == playerHand[4])
        {
            pointsPlayer = 50;
        }
        else
        {
            pointsPlayer = 0;
        }

        if (enemyHand[0] == enemyHand[1] && enemyHand[0] == enemyHand[2] && enemyHand[0] == enemyHand[3] && enemyHand[0] == enemyHand[4])
        {
            pointsEnemy = 50;
        }
        else
        {
            pointsEnemy = 0;
        }

        playerscore12.text = pointsPlayer.ToString();
        if (enemyscore12.color == Color.white)
        {
            enemyscore12.text = pointsEnemy.ToString();
            enemyScorePicker.Add(enemyscore12, pointsEnemy);
        }
        else
        {
            currentEnemyScore += int.Parse(enemyscore12.text);
        }
        Debug.Log("Enemy score: " + pointsEnemy + " and Player score: " + pointsPlayer);

        //code for Wildcard
        pointsPlayer = 0;
        pointsEnemy = 0;
        for (int i = 0; i < playerDice.Count; i++)
        {
                pointsPlayer += playerHand[i];

                pointsEnemy += enemyHand[i];
            
        }
        playerscore13.text = pointsPlayer.ToString();
        if (enemyscore13.color == Color.white)
        {
            enemyscore13.text = pointsEnemy.ToString();
            enemyScorePicker.Add(enemyscore13, pointsEnemy);
        }
        else
        {
            currentEnemyScore += int.Parse(enemyscore13.text);
        }
        Debug.Log("Enemy score: " + pointsEnemy + " and Player score: " + pointsPlayer);

        //code for Total

        //
        //(int.Parse(enemyscore1.text) +
        //                        int.Parse(enemyscore2.text) + 
        //                        int.Parse(enemyscore3.text) + 
        //                        int.Parse(enemyscore4.text) + 
        //                        int.Parse(enemyscore5.text) + 
        //                        int.Parse(enemyscore6.text) + 
        //                        int.Parse(enemyscore7.text) + 
        //                        int.Parse(enemyscore8.text) + 
        //                        int.Parse(enemyscore9.text) + 
        //                        int.Parse(enemyscore10.text) + 
        //                        int.Parse(enemyscore11.text) + 
        //                        int.Parse(enemyscore12.text) + 
        //                        int.Parse(enemyscore13.text)).ToString(); //add the rest when they work
        totalPlayerScore.text = (int.Parse(realplayerscore1.text) +
                                 int.Parse(realplayerscore2.text) +
                                 int.Parse(realplayerscore3.text) +
                                 int.Parse(realplayerscore4.text) +
                                 int.Parse(realplayerscore5.text) +
                                 int.Parse(realplayerscore6.text) +
                                 int.Parse(realplayerscore7.text) +
                                 int.Parse(realplayerscore8.text) +
                                 int.Parse(realplayerscore9.text) +
                                 int.Parse(realplayerscore10.text) +
                                 int.Parse(realplayerscore11.text) +
                                 int.Parse(realplayerscore12.text) +
                                 int.Parse(realplayerscore13.text)).ToString(); //add the rest when they work


        //enemy selects the highest scoring result
        var topscore = enemyScorePicker.OrderByDescending(pair => pair.Value).Take(1);
        foreach (KeyValuePair<TextMeshProUGUI, int> item in topscore)
        {
            item.Key.color = Color.red;
            enemySelectedScore = item.Value;

        }
        totalEnemyScore.text = (currentEnemyScore + enemySelectedScore).ToString();


    }


    public void assignBattleRoles()
    {
        for (int i = 0; i < playerDice.Count; i++)
        {
            playerDice[i].GetComponent<Dice>().hp = playerSelectedScore + 10;
            playerDice[i].GetComponent<Dice>().hpMax = playerSelectedScore + 10;
            int attack = playerDice[i].GetComponent<Dice>().value + -1;
            int defense = Mathf.CeilToInt((playerDice[i].GetComponent<Dice>().value + 1f) / 2f);

            for (int j = 0; j < playerDice.Count; j++)
            {
                if(playerDice[j].GetComponent<Dice>().value == playerDice[i].GetComponent<Dice>().value)
                {
                    attack += 2;
                }
                else if((playerDice[j].GetComponent<Dice>().value -1 == playerDice[i].GetComponent<Dice>().value || 
                         playerDice[j].GetComponent<Dice>().value +1 == playerDice[i].GetComponent<Dice>().value) )
                {
                    defense += 1;
                }
            }
            playerDice[i].GetComponent<Dice>().attack = attack;
            playerDice[i].GetComponent<Dice>().defense = defense;

            enemyDice[i].GetComponent<Dice>().hp = enemySelectedScore + 10;
            enemyDice[i].GetComponent<Dice>().hpMax = enemySelectedScore + 10;
            int attack2 = enemyDice[i].GetComponent<Dice>().value + -1;
            int defense2 = Mathf.CeilToInt((enemyDice[i].GetComponent<Dice>().value + 1f) / 2f) - 1;

            for (int k = 0; k < enemyDice.Count; k++)
            {
                if (enemyDice[k].GetComponent<Dice>().value == enemyDice[i].GetComponent<Dice>().value)
                {
                    attack2 += 2;
                }
                else if ((enemyDice[k].GetComponent<Dice>().value - 1 == enemyDice[i].GetComponent<Dice>().value ||
                         enemyDice[k].GetComponent<Dice>().value + 1 == enemyDice[i].GetComponent<Dice>().value))
                {
                    defense2 += 1;
                }
            }
            enemyDice[i].GetComponent<Dice>().attack = attack2;
            enemyDice[i].GetComponent<Dice>().defense = defense2;


            playerDice[i].GetComponent<Dice>().AssignClass();
            enemyDice[i].GetComponent<Dice>().AssignClass();
        }
    }








}
