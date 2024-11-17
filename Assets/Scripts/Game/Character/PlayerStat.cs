using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStat : MonoBehaviour
{
    public PlayerStatSO stat;



    [SerializeField]
    public int maxHP { get; private set; }

    [SerializeField]
    public int currentHP { get; private set; }
    [SerializeField]
    public int shield { get; private set; }


    [SerializeField]
    public int attackPoint { get; private set; }

    [SerializeField]
    public int movePoint { get; private set; }

    [SerializeField]
    public int shieldPoint { get; private set; }

    [Space]
    public UnityEvent OnDamage;
    public UnityEvent OnHeal;

    public UnityEvent OnShieldChange;

    public UnityEvent OnDeath;

    public UnityEvent OnManaChange;

    private void OnValidate()
    {
        //Nah
    }

    // Start is called before the first frame update
    void Start()
    {
        if (stat != null)
        {
            maxHP = stat.maxHP;
            currentHP = maxHP;
            shield = 0;
            attackPoint = stat.startingAttack;
            movePoint = stat.startingMove;
            shieldPoint = stat.startingShield;
        }
        else
        {
            throw new System.Exception("PlayerStatSO is not assigned");
        }

        OnDamage = new UnityEvent();
        OnHeal = new UnityEvent();
        OnDeath = new UnityEvent();
        OnManaChange = new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        if (damage - shield > 0)
        {
            damage -= shield;
            shield = 0;
        }
        else
        {
            shield -= damage;
            damage = 0;
        }
        currentHP -= damage;
        
        OnDamage.Invoke();
        OnShieldChange.Invoke();

        if (currentHP <= 0)
        {
            OnDeath.Invoke();
        }
    }

    public void Heal(int heal)
    {
        currentHP += heal;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        OnHeal.Invoke();
    }

    public bool UseMana(int type, int amount = 1)
    {
        bool result = false;
        switch (type)
        {
            case 0:
                if (attackPoint >= amount)
                {
                    attackPoint -= amount;
                    result = true;
                }
                break;
            case 1:
                if (movePoint >= amount)
                {
                    movePoint -= amount;
                    result = true;
                }
                break;
            case 2:
                if (shieldPoint >= amount)
                {
                    shieldPoint -= amount;
                    shield += amount;
                    result = true;
                }
                break;
        }
        OnManaChange.Invoke();
        OnShieldChange.Invoke();
        return result;
    }

    private void AddMana(int type)
    {
        switch (type)
        {
            case 0:
                attackPoint++;
                break;
            case 1:
                movePoint++;
                break;
            case 2:
                shieldPoint++;
                break;
        }
        OnManaChange.Invoke();
    }

    public void StartTurnReset()
    {
        int need = stat.maxTotalPoint - attackPoint - movePoint - shieldPoint;

        List<int> mana = new List<int>();

        for (int i = 0; i < need; i++)
        {
            mana.Add(Random.Range(0, 3));
        }

        foreach (int m in mana)
        {
            AddMana(m);
        }

        shield = 0;
        OnShieldChange.Invoke();
    }
}

