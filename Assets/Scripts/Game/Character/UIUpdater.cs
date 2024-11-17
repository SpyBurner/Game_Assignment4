using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour
{
    public GameObject player;
    
    private PlayerCore playerCore;
    private PlayerStat playerStat;

    private Text healthText;
    private Text redText;
    private Text greenText;
    private Text yellowText;

    void Start()
    {
        //Delayed start
        Invoke("ButtonInit", 0.5f);
    }

    void ButtonInit()
    {
        playerCore = player.GetComponent<PlayerCore>();
        playerStat = player.GetComponent<PlayerStat>();

        //Turn
        playerCore.OnTurnStart.AddListener(OnTurnStart);
        playerCore.OnTurnEnd.AddListener(OnTurnEnd);

        //HP
        playerStat.OnDamage.AddListener(OnHealthChange);
        playerStat.OnHeal.AddListener(OnHealthChange);

        //Mana
        playerStat.OnManaChange.AddListener(OnManaChange);

        OnHealthChange();
        transform.Find("EndTurnButton").GetComponent<Button>().onClick.AddListener(() => playerCore.EndTurn());
    }

    void OnTurnStart()
    {
        foreach (Transform child in transform)
        {
            Button button = child.GetComponent<Button>();
            if (button != null)
            {
                button.interactable = true;
            }
        }
    }

    void OnTurnEnd()
    {
        foreach (Transform child in transform)
        {
            Button button = child.GetComponent<Button>();
            if (button != null)
            {
                button.interactable = false;
            }
        }
    }

    void OnHealthChange()
    {
        healthText.text = playerStat.currentHP + "/" + playerStat.maxHP;
    }

    void OnManaChange()
    {
        redText.text = playerStat.attackPoint.ToString();
        greenText.text = playerStat.movePoint.ToString();
        yellowText.text = playerStat.shieldPoint.ToString();
    }

}
