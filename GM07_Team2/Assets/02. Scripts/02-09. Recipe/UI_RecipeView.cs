using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class UI_RecipeView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private TMP_Text _gradeText;
    [SerializeField] private TMP_Text _unlockedText;
    [SerializeField] private Button _unlockButton;

    private Recipe _recipe;
    public void Bind(Recipe recipe)
    {
        if (recipe == null)
        {
            return;
        }

        _recipe = recipe;
        _unlockButton.onClick.AddListener(Unlock);
    }

    // 호출 순서 명시하기 위한 메서드
    private void Unlock()
    {
        _recipe.Unlock();
        Draw();
    }
    public void Draw()
    {
        if (_recipe == null)
        {
            return;
        }

        if(_icon != null)
        {
            _icon.sprite = _recipe.Data.Icon;
        }
        if(_nameText != null)
        {
            _nameText.text = _recipe.Data.Name;
        }
        if(_costText != null)
        {
            _costText.text =  "Cost:" + _recipe.Data.Cost.ToString();
        }
        if(_priceText != null)
        {
            _priceText.text = "Price:" + _recipe.Data.Price.ToString();
        }
        if(_gradeText != null)
        {
            _gradeText.text = "Grade:" + _recipe.Grade.ToString();
        }
        if(_unlockedText != null)
        {
            _unlockedText.text = "Unlocked:"+_recipe.Unlocked.ToString();
        }
    }
}
