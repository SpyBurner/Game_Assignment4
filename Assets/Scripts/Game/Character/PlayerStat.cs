using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
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
    public int attackPoint { get; private set; }

    [SerializeField]
    public int movePoint { get; private set; }

    [SerializeField]
    public int shieldPoint { get; private set; }

    [Space]
    public UnityEvent OnDamage;
    public UnityEvent OnHeal;
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
        currentHP -= damage;
        OnDamage.Invoke();

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

    public void UseMana(int type)
    {
        switch (type)
        {
            case 0:
                if (attackPoint > 0)
                    attackPoint--;
                break;
            case 1:
                if (movePoint > 0)
                    movePoint--;
                break;
            case 2:
                if (shieldPoint > 0)
                    shieldPoint--;
                break;
        }
        OnManaChange.Invoke();
    }

}

