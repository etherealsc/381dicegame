using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI realScoreText;
    public BattleController battleController;
    // Start is called before the first frame update
    void Start()
    {
        buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        realScoreText = gameObject.transform.parent.gameObject.GetComponent<TextMeshProUGUI>();
        battleController = GameObject.Find("ArenaManager").GetComponent<BattleController>();
    }

    public void LockInScore()
    {
        battleController.playerSelectedScore = int.Parse(buttonText.text);
        realScoreText.text = buttonText.text;
        gameObject.GetComponent<Button>().interactable = false;
        gameObject.SetActive(false);
        battleController.assignBattleRoles();
        battleController.LockEverything();
        battleController.roundTimerNumber = 30f;
        battleController.battleState = BattleController.BattleState.BATTLING;
    }
}
