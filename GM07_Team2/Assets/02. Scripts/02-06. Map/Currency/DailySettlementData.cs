using System;

[Serializable]
public sealed class DailySettlementData
{
    public int Day;
    public int CustomerCount;
    public int SalesRevenue;
    public int TipRevenue;
    public int RentExpense;
    public int WageExpense;
    public int OtherExpense;

    public int TotalRevenue => SalesRevenue + TipRevenue;
    public int TotalExpense => RentExpense + WageExpense + OtherExpense;
    public int NetProfit => TotalRevenue - TotalExpense;
}
