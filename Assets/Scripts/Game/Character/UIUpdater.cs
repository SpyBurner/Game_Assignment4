using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour
{
    public GameObject player;
    
    private PlayerCore playerCore;
    private PlayerStat playerStat;

    public Text healthText;
    public Text redText;
    public Text greenText;
    public Text yellowText;

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

        OnManaChange();

        transform.Find("EndTurnButton").GetComponent<Button>().onClick.AddListener(() => TurnManager.Instance.AdvanceTurn());
        if (TurnManager.Instance.turnID != playerCore.turnID)
        {
            OnTurnEnd();
        }

        transform.Find("AttackButton").GetComponent<Button>().onClick.AddListener(() => playerStat.UseMana(0));
        transform.Find("MoveButton").GetComponent<Button>().onClick.AddListener(() => playerStat.UseMana(1));
        transform.Find("ShieldButton").GetComponent<Button>().onClick.AddListener(() => playerStat.UseMana(2));
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
