public class RecipeSaveData
{
    public int RecipeId { get; private set; }
    public ERecipeGrade Grade { get; private set; }
    public bool Unlocked { get; private set; }

    public RecipeSaveData(int recipeId, ERecipeGrade grade, bool unlocked)
    {
        RecipeId = recipeId;
        Grade = grade;
        Unlocked = unlocked;
    }
}
