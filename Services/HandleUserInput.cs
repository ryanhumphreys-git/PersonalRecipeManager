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

        foreach(var item in newIngredients)
        {
            Guid itemGuid = dataStore.GetIngredientByName(item.Key).Id;
            double itemAmount = item.Value;
            RecipeIngredients newRecipeItem = new(newRecipeId, itemGuid, itemAmount);
            dataStore.InsertRecipeIngredient(newRecipeItem);
        }

        foreach(string item in newTools)
        {
            Guid itemGuid = dataStore.GetToolsAndEquipmentByName(item).Id;
            RecipeToolsAndEquipment newToolsOrEquipment = new(newRecipeId, itemGuid, 1);
            dataStore.InsertRecipeEquipmentOrTools(newToolsOrEquipment);
        }
        
        double newRecipeCost = dataStore.GetRecipeCost(newRecipeId);

        Recipes newRecipe = new Recipes(newRecipeId, newRecipeName, newRecipeDifficulty, newRecipeTime, newRecipeCost);

        dataStore.InsertNewRecipe(newRecipe);
    }

    public void HandleAddIngredient(Entity newEntity)
    {
        GetInputService.GetInput("Enter the name of the ingredient: ", out string newItemName);
        GetInputService.GetInput("Enter the cost of the ingredient: ", out double newItemCost);
        GetInputService.GetInput("Enter the amount of the ingredient you got: ", out double newItemAmount);
        GetInputService.GetInput("Enter the units associated for the amount ", out string newItemUnits);

        Guid newIngredientId = Guid.NewGuid();
        Ingredients newIngredient = new Ingredients(newIngredientId, newItemName, newItemCost, newItemUnits);
        KitchenIngredients newKitchenIngredient = new KitchenIngredients(Guid.NewGuid(), newEntity.KitchenTypeId, newIngredientId, newItemAmount);
        bool ingredientExists = dataStore.CheckIfIngredientExists(newIngredient);
        if(ingredientExists)
        {
            Ingredients currentIngredientInfo = dataStore.GetIngredientByName(newItemName);
            KitchenIngredients currentKitchenIngredient = dataStore.GetKitchenIngredientById(currentIngredientInfo.Id);

            currentKitchenIngredient.Quantity += newItemAmount;
            dataStore.UpdateKitchenIngredientQuantity(currentKitchenIngredient);

            if(currentIngredientInfo.Cost - newItemCost != 0 || currentIngredientInfo.UnitOfMeasurement != newItemUnits)
            {
                currentIngredientInfo.Cost = newItemCost;
                currentIngredientInfo.UnitOfMeasurement = newItemUnits;
                dataStore.UpdateIngredientProperties(currentIngredientInfo);
            }
        }
        if(!ingredientExists)
        {
            dataStore.InsertNewIngredient(newIngredient);
            dataStore.InsertNewKitchenIngredient(newKitchenIngredient);
        }
    }

    public void HandleAddToolOrEquipment()
    {
        GetInputService.GetInput("Enter the name of the tool/equipment: ", out string newItemName);
        GetInputService.GetInput("Enter the cost of the tool/equipment: ", out double newItemCost);
        GetInputService.GetInput("Enter the amount of the tool/equipment you got: ", out double newItemAmount);

        var newItem = new ToolsAndEquipment(Guid.NewGuid(), newItemName, newItemCost);
        dataStore.InsertNewToolOrEquipment(newItem);
    }

    public void HandleShow(Entity newEntity)
    {
        GetInputService.GetInput("What would you like to show?", out string showString);

        try
        {
            if (showString.ToLower() == "recipes")
            {
                newEntity.RecipeList = dataStore.SelectAllKnownRecipes();
                Console.WriteLine("You have recipes for: ");
                foreach (Recipes item in newEntity.RecipeList)
                {
                    Console.WriteLine($"{item.Name}");
                }
            }
            if (showString.ToLower() == "ingredients")
            {
                var ingredientList = dataStore.SelectAllIngredients();
                Console.WriteLine("You have the ingredients: ");
                foreach (Ingredients item in ingredientList)
                {
                    Console.WriteLine($"{item.Name}");
                }
            }
            if (showString.ToLower() == "tools" || showString.ToLower() == "equipment")
            {
                var equipmentAndToolList = dataStore.SelectAllToolsAndEquipment();
                Console.WriteLine("You have the tools and equipment: ");
                foreach (ToolsAndEquipment item in equipmentAndToolList)
                {
                    Console.WriteLine($"{item.Name}");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
    }
}
