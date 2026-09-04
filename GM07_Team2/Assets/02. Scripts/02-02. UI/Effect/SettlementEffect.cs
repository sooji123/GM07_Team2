using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettlementEffect : EffectBase
{
    [SerializeField]
    private List<TMP_Text> _texts = new();
    [SerializeField]
    private RectTransform _totalRevenue;
    [SerializeField]
    private TMP_Text _totalRevenueText;

    private List<Vector3> _restScales = new();
    private Vector3 _totalRestScale;
    private bool _hasCachedState;
    private bool _isPlaying;
    private int _totalRevenueValue;
    private float _highlightStartTime;
    private float _lastTextEnd;

    public void SetTotalRevenu(int totalRevenue)
    {
        _totalRevenueValue = totalRevenue;
    }
    override public void Prepare()
    {
        Kill();
        CacheState();
        for(int i = 0; i < _texts.Count; i++)
        {
            _texts[i].alpha = 0f;
            _texts[i].transform.localScale = _restScales[i] * 0.85f;
        }
        _totalRevenue.localScale = _totalRestScale * 0.85f;
        _totalRevenueText.text = "0";
    }
    override public Tween Play()
    {
        Prepare();
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        _lastTextEnd = 0.2f;
        for (int i = 0; i < _texts.Count; i++)
        {
            TMP_Text text = _texts[i];
            float delay = 0.2f + i * 0.2f;
            sequence.Insert(delay, text.DOFade(1f, 0.3f));
            sequence.Insert(delay, text.transform.DOScale(_restScales[i], 0.3f).SetEase(Ease.OutBack));
            _lastTextEnd = Mathf.Max(_lastTextEnd, delay + 0.3f);
        }
        _highlightStartTime = _lastTextEnd + 0.2f;

        sequence.Insert(_highlightStartTime, _totalRevenue.DOScale(_totalRestScale, 0.35f).SetEase(Ease.OutBack));
        sequence.Insert(_highlightStartTime, DOTween.To(() => 0, value => _totalRevenueText.text = $"{value:N0}", _totalRevenueValue, 0.35f).SetEase(Ease.OutQuad));
        sequence.Insert(_highlightStartTime + 0.35f, _totalRevenue.DOPunchScale(Vector3.one * 0.25f, 0.35f));

        sequence.OnComplete(() => CompleteEffect());
        _isPlaying = true;
        _tween = sequence;
        return _tween;
    }
    public bool TrySkipToHighlight()
    {
        if (!_isPlaying || _tween == null || !_tween.IsActive())
        {
            return false;
        }

        if (_tween.Elapsed() < _lastTextEnd)
        {
            _tween.Goto(_lastTextEnd, true);
        }
        return true;
    }
    private void CompleteEffect()
    {
        if (_totalRevenue != null)
        {
            _totalRevenue.localScale = _totalRestScale;
        }
        if (_totalRevenueText != null)
        {
            _totalRevenueText.text = $"{_totalRevenueValue:N0}";
        }

        _isPlaying = false;
        _tween = null;
    }
    public override void Kill()
    {
        base.Kill();
        _isPlaying = false;
    }
    private void CacheState()
    {
        if (_hasCachedState)
        {
            return;
        }
        _restScales.Clear();
        foreach (var text in _texts)
        {
            _restScales.Add(text.transform.localScale);
        }
        _totalRestScale = _totalRevenue.localScale;
        _hasCachedState = true;
    }
}
