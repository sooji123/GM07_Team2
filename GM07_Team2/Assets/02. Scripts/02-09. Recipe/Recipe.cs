using UnityEngine;

public class Recipe
{
    public RecipeData Data { get; private set; } // 정적 데이터
    public int RecipeId => Data.RecipeId;
    public ERecipeGrade Grade { get; private set; } = ERecipeGrade.Normal; // 레시피 등급
    public bool Unlocked { get; private set; } = false; // 해금 여부

    public Recipe(RecipeData data)
    {
        Data = data;
    }
    public void Unlock()
    {
        Grade = (ERecipeGrade)Random.Range(0, (int)ERecipeGrade.Size);
        Unlocked = true;
    }
    public RecipeSaveData Save()
    {
        return new RecipeSaveData(RecipeId, Grade, Unlocked);
    }
    public void Load()
    {
        // RecipeId 기반 저장된 데이터 불러오는 로직 추가 예정
    }
}
