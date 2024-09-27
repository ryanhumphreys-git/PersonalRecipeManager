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

        if (recipeIngredients == null)
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
        GetInputService.GetInput("Enter the items then the number needed for the recipe", out Dictionary<string, double> newItems);

        Guid newRecipeId = Guid.NewGuid();

        dataStore.InsertRecipeItemsFromDictionary(newItems, newRecipeId);
        
        double newRecipeCost = dataStore.GetRecipeCost(newRecipeId);

        Recipes newRecipe = new Recipes(Guid.NewGuid(), newRecipeName, newRecipeDifficulty, newRecipeTime, newRecipeCost);

        dataStore.InsertNewRecipe(newRecipe);
    }

    public void HandleAddItem()
    {
        /* Would need 3 options for this to add different items with seperate item classes */
        GetInputService.GetInput("Enter the name of the item: ", out string newItemName);
        GetInputService.GetInput("Enter the type of item (ingredient, tool, equipment): ", out string newItemType);
        GetInputService.GetInput("Enter the cost of the item: ", out double newItemCost);
        GetInputService.GetInput("Enter the amount of the item you got: ", out double newItemAmount);
        GetInputService.GetInput("Enter the units associated for the amount (for" +
         "single objects or things counted one at a time use 'count')", out string newItemUnits);


        var newItem = new Item(Guid.NewGuid(), newItemName, newItemCost, newItemAmount,
                                newItemType switch
                                {
                                    "ingredient" => dataStore.GetItemTypeGuid("ingredient"),
                                    "equipment" => dataStore.GetItemTypeGuid("equipment"),
                                    "tool" => dataStore.GetItemTypeGuid("tool"),
                                    _ => throw new ArgumentException("Incorrect type assigned")
                                }, newItemUnits);
        dataStore.InsertNewItem(newItem);
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
                string typeItem = showString switch
                {
                    "ingredients" => "ingredient",
                    "tools" => "tool",
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
