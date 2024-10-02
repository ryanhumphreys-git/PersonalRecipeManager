namespace PersonalRecipeManger.Models;

public interface IDataStore
{
    // GETS
    public Entity GetEntity(string name);
    public double GetRecipeCost(Guid nextId);
    public List<RecipeIngredientsDTO> GetRecipeIngredients(string recipeString);
    public Ingredients GetIngredientByName(string ingredientName);
    public KitchenIngredients GetKitchenIngredientById(Guid ingredientName);
    public ToolsAndEquipment GetToolsAndEquipmentByName(string toolName);

    // CHECKS
    public string CheckIfRecipeExists(string recipeName);
    public bool CheckIfIngredientExists(Ingredients ingredient);

    // INSERTS
    public void InsertRecipeIngredient(RecipeIngredients newRecipeItem);
    public void InsertRecipeEquipmentOrTools(RecipeToolsAndEquipment newToolOrEquipment);
    public void InsertNewRecipe(Recipes newRecipe);
    public void InsertNewIngredient(Ingredients newItem);
    public void InsertNewKitchenIngredient(KitchenIngredients newIngredient);
    public void InsertNewToolOrEquipment(ToolsAndEquipment newItem);
    public void InsertKitchenNewToolOrEquipment(KitchenToolsAndEquipment newItem);

    // UPDATES
    public void UpdateKitchenIngredientQuantity(KitchenIngredients newIngredient);
    public void UpdateIngredientProperties(Ingredients newIngredient);
    public void UpdateEntityInformation(Entity newEntity, Guid id);

    // SELECTS
    public List<Ingredients> SelectAllIngredients();
    public List<ToolsAndEquipment> SelectAllToolsAndEquipment();
    public List<Recipes> SelectAllKnownRecipes();
}