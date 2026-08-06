using TMPro;
using UnityEngine;

public class UI_ClosePanel : MonoBehaviour
{
    [SerializeField]
    private DailySettlementManager _settlementManager;
    [SerializeField]
    private TMP_Text _dayText;
    [SerializeField]
    private TMP_Text _settlementText;

    private void OnEnable()
    {
        if(_settlementManager == null)
        {
            return;
        }
        _settlementManager.OnSettlementCompleted += RefreshSettlement;
        if (_settlementManager.DailySettlementData == null)
        {
            return;
        }
        RefreshSettlement(_settlementManager.DailySettlementData);
    }

    private void OnDisable()
    {
        if (_settlementManager != null)
        {
            _settlementManager.OnSettlementCompleted -= RefreshSettlement;
        }
    }

    private void RefreshSettlement(DailySettlementData data)
    {
        if(_dayText == null || _settlementText == null)
        {
            return;
        }

        _dayText.text = $"Day - {data.Day}\n";

        _settlementText.text =
            $"CustomerCount : {data.CustomerCount}\n" +
            $"SalesRevenue : {data.SalesRevenue:N0}\n" +
            $"TipRevenue : {data.TipRevenue:N0}\n" +
            "======================\n" +
            $"TotalRevenue : {data.TotalRevenue:N0}\n\n" +

            $"RentExpense : {data.RentExpense:N0}\n" +
            $"WageExpense : {data.WageExpense:N0}\n" +
            $"OtherExpense : {data.OtherExpense:N0}\n" +
            $"OtherExpense : {data.OtherExpense:N0}\n" +
            "======================\n" +
            $"TotalExpense : {data.TotalExpense:N0}\n\n" +

            $"NetProfit : {data.NetProfit:N0}\n" +
            $"Money : {CurrencyManager.Instance.Money:N0}";
    }
}
