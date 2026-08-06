using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviourSingleton<CurrencyManager>
{
    [SerializeField, Min(0)]
    private int _initialMoney;

    public int Money { get; private set; }

    public Action<int, ECurrencyTransactionType> OnMoneyTransaction;
    public Action<int> OnMoneyChanged;

    private void Start()
    {
        InitMoney(_initialMoney);
    }

    public void InitMoney(int money)
    {
        Money = money;
        OnMoneyChanged?.Invoke(Money);
    }

    public void AddMoney(int amount, ECurrencyTransactionType transactionType)
    {
        if(amount <= 0)
        {
            return;
        }
        Money += amount;
        OnMoneyChanged?.Invoke(Money);
        OnMoneyTransaction?.Invoke(amount, transactionType);
    }

    public bool TrySpendMoney(int amount, ECurrencyTransactionType transactionType)
    {
        if (amount <= 0 || Money < amount)
        {
            return false;
        }
        Money -= amount;
        OnMoneyChanged?.Invoke(Money);
        OnMoneyTransaction?.Invoke(amount, transactionType);
        return true;
    }
}
