namespace PersonalRecipeManger.Models;

public interface IDataStore
{
    public Guid GetItemTypeGuid(string itemType);
    public Entity GetEntity(string name);
    public List<RecipeIngredientsDTO> GetRecipeIngredients(string recipeString);
    public string CheckIfRecipeExists(string recipeName);
    public void InsertRecipeItemsFromDictionary(Dictionary<string, double> newItems, Guid newRecipeId);
    public double GetRecipeCost(Guid nextId);
    public void InsertNewRecipe(Recipes newRecipe);
    public void InsertNewItem(Item newItem);
    public void UpdateEntityInformation(Entity newEntity, Guid id);
    public List<string> SelectItemsOfType(string typeItem);
    public List<Recipes> SelectCurrentRecipes();
}