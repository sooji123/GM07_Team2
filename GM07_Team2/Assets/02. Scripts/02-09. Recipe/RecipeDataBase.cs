using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "RecipeDataBase", menuName = "Recipe/RecipeDataBase")]
public class RecipeDataBase : ScriptableObject
{
    [SerializeField]
    private List<RecipeData> _recipeDatas = new List<RecipeData>();
    public IReadOnlyList<RecipeData> RecipeDatas => _recipeDatas;
    public int Count => _recipeDatas.Count;

    // 레시피 ID를 통해 데이터를 찾는 메서드
    public bool TryGetRecipeData(int recipeId, out RecipeData recipeData)
    {
        foreach (var data in _recipeDatas)
        {
            if(data.RecipeId == recipeId)
            {
                recipeData = data;
                return true;
            }
        }
        recipeData = null;
        return false;
    }
}
