using TMPro;
using UnityEngine;

public class UI_ClosePanel : MonoBehaviour
{
    [SerializeField]
    private DailySettlementManager _settlementManager;
    [SerializeField]
    private TMP_Text _dayText;
    [Header("Effect")]
    [SerializeField]
    private SettlementEffect _settlementEffect;
    [Header("Settlement Text")]
    [SerializeField]
    private TMP_Text _customerCount;
    [SerializeField]
    private TMP_Text _salesRevenue;
    [SerializeField]
    private TMP_Text _tipRevenue;
    [SerializeField]
    private TMP_Text _totalRevenue;
    [SerializeField]
    private TMP_Text _rentExpense;
    [SerializeField]
    private TMP_Text _wageExpense;
    [SerializeField]
    private TMP_Text _otherExpense;
    [SerializeField]
    private TMP_Text _totalExpense;
    [SerializeField]
    private TMP_Text _netProfit;

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
        AudioManager.Instance?.PlaySFX(EAudioType.Result);
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
        if(_dayText == null)
        {
            return;
        }

        _dayText.text = $"{data.Day}일차 정산\n";
        _customerCount.text = $"{data.CustomerCount}";
        _salesRevenue.text = $"{data.SalesRevenue:N0}";
        _tipRevenue.text = $"{data.TipRevenue:N0}";
        _totalRevenue.text = $"{data.TotalRevenue:N0}";
        _rentExpense.text = $"{data.RentExpense:N0}";
        _wageExpense.text = $"{data.WageExpense:N0}";
        _otherExpense.text = $"{data.OtherExpense:N0}";
        _totalExpense.text = $"{data.TotalExpense:N0}";
        _netProfit.text = "0";

        _settlementEffect?.SetTotalRevenu(data.NetProfit);
        _settlementEffect?.Play();
    }
    public bool TrySkipSettlementEffect()
    {
        return _settlementEffect != null && _settlementEffect.TrySkipToHighlight();
    }
}
