using PersonalRecipeManger.Models;

namespace PersonalRecipeManger.Services;
public class HandleUserInput
{
    private IDataStore dataStore;
    public HandleUserInput(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public Entity LoadEntityInformation(string name)
    {
        return dataStore.GetEntity(name);
    }

    public Entity HandleUpdate(Entity currentEntity)
    {
        Entity newEntity = GetInputService.GetNewEntityInformation();
        dataStore.UpdateEntityInformation(newEntity, currentEntity.Id);
        return newEntity;
    }

    public void HandleShowRecipe()
    {
        GetInputService.GetInput("What recipe would you like to view?", out string recipeString);
        var recipeIngredients = dataStore.GetRecipeIngredients(recipeString);

        if (recipeIngredients.Count == 0)
        {
            Console.WriteLine("You don't know that recipe");
            return;
        }
        Console.WriteLine($"Making a {recipeString} requires: ");

        foreach(var item in recipeIngredients)
        {
            Console.WriteLine($"{item.Quantity} " +
                              $"{item.UnitOfMeasurement} of " +
                              $"{item.Name}.");
        }
    }

    public void HandleAddRecipe()
    {
        GetInputService.GetInput("Enter the name of the recipe: ", out string newRecipeName);

        var recipeCheck = dataStore.CheckIfRecipeExists(newRecipeName);
        if (recipeCheck != "not found")
        {
            Console.WriteLine("you already have this recipe");
            return;
        }

        GetInputService.GetInput("Enter the difficulty of the recipe (1-10): ", out int newRecipeDifficulty);
        GetInputService.GetInput("Enter the time this recipe takes to make: ", out double newRecipeTime);
        GetInputService.GetInput("Enter the ingredients then the amount needed for the recipe", out Dictionary<string, double> newIngredients);
        GetInputService.GetInput("Enter the tools and equipment needed for the recipe", out List<string> newTools);
        Guid newRecipeId = Guid.NewGuid();

        dataStore.InsertRecipeIngredientsFromDictionary(newIngredients, newRecipeId);
        dataStore.InsertRecipeEquipmentAndToolsFromList(newTools, newRecipeId);
        
        double newRecipeCost = dataStore.GetRecipeCost(newRecipeId);

        Recipes newRecipe = new Recipes(newRecipeId, newRecipeName, newRecipeDifficulty, newRecipeTime, newRecipeCost);

        dataStore.InsertNewRecipe(newRecipe);
    }

    public void HandleAddIngredient()
    {
        GetInputService.GetInput("Enter the name of the ingredient: ", out string newItemName);
        GetInputService.GetInput("Enter the cost of the ingredient: ", out double newItemCost);
        GetInputService.GetInput("Enter the amount of the ingredient you got: ", out double newItemAmount);
        GetInputService.GetInput("Enter the units associated for the amount ", out string newItemUnits);

        bool ingredientExists = dataStore.CheckIfIngredientExists(newItemName);
        if(ingredientExists)
        {
            dataStore.UpdateIngredientQuantity(newItemName, newItemAmount);
        }
        if(!ingredientExists)
        {
            var newItem = new Ingredients(Guid.NewGuid(), newItemName, newItemCost, newItemAmount, newItemUnits);
            dataStore.InsertNewIngredient(newItem);
        }
        
    }

    public void HandleAddToolOrEquipment()
    {
        GetInputService.GetInput("Enter the name of the tool/equipment: ", out string newItemName);
        GetInputService.GetInput("Enter the cost of the tool/equipment: ", out double newItemCost);
        GetInputService.GetInput("Enter the amount of the tool/equipment you got: ", out double newItemAmount);

        var newItem = new ToolsAndEquipment(Guid.NewGuid(), newItemName, newItemCost, newItemAmount);
        dataStore.InsertNewToolOrEquipment(newItem);
    }

    public void HandleShow(Entity newEntity)
    {
        GetInputService.GetInput("What would you like to show?", out string showString);

        try
        {
            if (showString == "recipes")
            {
                newEntity.RecipeList = dataStore.SelectCurrentRecipes();
                Console.WriteLine("You have recipes for: ");
                foreach (Recipes item in newEntity.RecipeList)
                {
                    Console.WriteLine($"{item.Name}");
                }
            }
            else
            {
                string typeItem = showString.ToLower() switch
                {
                    "ingredients" or "i" => "ingredient",
                    "tools" or "tool" => "tool",
                    "equipment" => "equipment",
                    _ => throw new ArgumentException("Please input 'ingredients', 'tools', or 'equipment")
                };
                var itemList = dataStore.SelectItemsOfType(typeItem);
                Console.WriteLine($"You have the {showString}: ");
                foreach (string item in itemList)
                {
                    Console.WriteLine($"{item}");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
    }
}
