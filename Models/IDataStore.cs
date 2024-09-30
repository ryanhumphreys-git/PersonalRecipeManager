namespace PersonalRecipeManger.Models;

public interface IDataStore
{
    public Entity GetEntity(string name);
    public List<RecipeIngredientsDTO> GetRecipeIngredients(string recipeString);
    public string CheckIfRecipeExists(string recipeName);
    public void InsertRecipeIngredientsFromDictionary(Dictionary<string, double> newItems, Guid newRecipeId);
    public void InsertRecipeEquipmentAndToolsFromList(List<string> newEquipment, Guid newRecipeId);
    public double GetRecipeCost(Guid nextId);
    public void InsertNewRecipe(Recipes newRecipe);
    public void InsertNewIngredient(Ingredients newItem);
    public void InsertNewTool(Tools newItem);
    public void InsertNewEquipment(Equipment newItem);
    public void UpdateEntityInformation(Entity newEntity, Guid id);
    public List<string> SelectItemsOfType(string typeItem);
    public List<Recipes> SelectCurrentRecipes();
}