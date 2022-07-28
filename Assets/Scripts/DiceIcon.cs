using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceIcon : MonoBehaviour
{
    public Sprite sprite;
    //also make a health bar
    public GameObject playerDice;

    public Toggle toggle;

    void Start()
    {
        toggle = gameObject.GetComponentInChildren<Toggle>();

        toggle.onValueChanged.AddListener(value => playerDice.GetComponent<Dice>().Lock());

    }

    public void Assign()
    {
        //toggle.isOn = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
