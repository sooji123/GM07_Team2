using System;
using System.Collections.Generic;

using UnityEngine;

public class RecipeManager : MonoBehaviourSingleton<RecipeManager>
{
    [SerializeField]
    private RecipeDataBase _dataBase;

    private List<Recipe> _recipes;
    
    public int Count => _recipes.Count;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        _recipes = new List<Recipe>();
        if(_dataBase != null)
        {
            foreach (RecipeData data in _dataBase.RecipeDatas)
            {
                Recipe newRecipe = new Recipe(data);
                _recipes.Add(newRecipe);
            }
        }
    }

    public bool TryGetRecipeIndex(int index, out Recipe recipe)
    {
        // index 범위 제한
        if (index < 0 || index >= _recipes.Count)
        {
            recipe = null;
            return false;
        }

        recipe = _recipes[index];
        return recipe != null;
    }
}
