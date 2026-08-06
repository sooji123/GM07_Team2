using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainGame : MonoBehaviour
{
    [Header("Heirarchy")]
    [SerializeField]
    private GameFlowManager _gameFlowManager;
    [SerializeField]
    private TMP_Text _dayText;
    [SerializeField]
    private TMP_Text _remainingTimeText;
    [SerializeField]
    private TMP_Text _gameStateText;
    [SerializeField]
    private TMP_Text _moneyText;
    [SerializeField]
    private Button _openButton;
    [SerializeField]
    private Button _nextdayButton;
    [SerializeField]
    private GameObject _closePanel;

    private void Start()
    {
        if(_gameFlowManager == null)
        {
            return;
        }
        if(_openButton != null)
        {
            _openButton.onClick.AddListener(() => _gameFlowManager.OnClickOpen());
        }
        if(_nextdayButton != null)
        {
            _nextdayButton.onClick.AddListener(() => _gameFlowManager.OnClickNextDay());
        }
        _gameFlowManager.OnGameStateChanged += RefreshGameState;
        _gameFlowManager.OnRemainingTimeChanged += RefreshRemainingTime;
        _gameFlowManager.OnDayChanged += RefreshDay;
        CurrencyManager.Instance.OnMoneyChanged += RefreshMoney;
    }
    private void OnDisable()
    {
        if(_gameFlowManager == null)
        {
            return;
        }
        if (_openButton != null)
        {
            _openButton.onClick.RemoveListener(() => _gameFlowManager.OnClickOpen());
        }
        if (_nextdayButton != null)
        {
            _nextdayButton.onClick.RemoveListener(() => _gameFlowManager.OnClickNextDay());
        }
        _gameFlowManager.OnGameStateChanged -= RefreshGameState;
        _gameFlowManager.OnRemainingTimeChanged -= RefreshRemainingTime;
        _gameFlowManager.OnDayChanged -= RefreshDay;
        CurrencyManager.Instance.OnMoneyChanged -= RefreshMoney;
    }

    private void RefreshGameState(EGameState gameState)
    {
        if(_gameStateText == null)
        {
            return;
        }

        RefreshButton();
        _gameStateText.text = GetGameStateText(gameState);
    }
    private void RefreshRemainingTime(float remainingTime)
    {
        if(_remainingTimeText == null)
        {
            return;
        }

        int totalSeconds = Mathf.CeilToInt(remainingTime);

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        _remainingTimeText.text = $"{minutes:00}:{seconds:00}";
    }
    private void RefreshDay(int day)
    {
        if(_dayText == null)
        {
            return;
        }
        _dayText.text = $"Day - {day}";
    }
    private void RefreshMoney(int money)
    {
        if(_moneyText == null)
        {
            return;
        }
        _moneyText.text = $"{money:N0}";
    }
    private void RefreshButton()
    {
        bool isPreparing = _gameFlowManager.GameState == EGameState.Preparing;
        bool isClosed = _gameFlowManager.GameState == EGameState.Close;

        if(_openButton != null)
        {
            _openButton.gameObject.SetActive(isPreparing);
        }
        if(_closePanel != null)
        {
            _closePanel.SetActive(isClosed);
        }
    }
    private string GetGameStateText(EGameState gameState)
    {
        return gameState switch
        {
            EGameState.Preparing => "Preparing",
            EGameState.Open => "Open",
            EGameState.ClosingWait => "Closing Wait",
            EGameState.Close => "Close",
            _ => ""
        };
    }
}
