using System.Linq.Expressions;
using PersonalRecipeManger.Models;

namespace PersonalRecipeManger.Services;
public class HandleUserInput
{
    private IDataStore dataStore;
    private List<Ingredients> ingredientList = new();
    private List<ToolsAndEquipment> toolsAndEquipmentList = new();
    
    public HandleUserInput(IDataStore dataStore)
    {
        this.dataStore = dataStore;
        RefreshItems();
    }

    public void RefreshItems()
    {
        ingredientList = dataStore.SelectAllIngredients();
        toolsAndEquipmentList = dataStore.SelectAllToolsAndEquipment();
    }

    public Entity LoadEntityInformation(string name)
    {
        return dataStore.GetEntity(name);
    }

    public Entity HandleUpdateEntity(Entity currentEntity)
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

        var recipeExists = dataStore.CheckIfRecipeExists(newRecipeName);
        if (recipeExists)
        {
            Console.WriteLine("you already have this recipe");
            return;
        }

        GetInputService.GetInput("Enter the difficulty of the recipe (1-10): ", out int newRecipeDifficulty);
        GetInputService.GetInput("Enter the time this recipe takes to make: ", out double newRecipeTime);
        GetInputService.GetInput("Enter the ingredients along with the associated amount and units", out List<(string ingredient, double amount, string units)> newIngredients);
        GetInputService.GetInput("Enter the tools and equipment needed for the recipe", out List<string> newTools);
        GetInputService.GetInput("Enter the units of: ", out string newRecipeUnits);
        Guid newRecipeId = Guid.NewGuid();

        Recipes newRecipe = new Recipes(newRecipeId, newRecipeName, newRecipeDifficulty, newRecipeTime, 0);
        double newRecipeCost = 0;

        foreach(var item in newIngredients)
        {
            var itemObject = ingredientList.FirstOrDefault(i => i.Name == item.ingredient);
            double itemAmount = item.amount;
            RecipeIngredients newRecipeItem = new(newRecipeId, itemObject.Id, itemAmount);

            newRecipeCost += itemObject.Cost;
            newRecipe.RecipeIngredients.Add(newRecipeItem);
        }

        foreach(string item in newTools)
        {
            Guid itemGuid = dataStore.GetToolsAndEquipmentByName(item).Id;

            var toolAndEquipmentObject = toolsAndEquipmentList.FirstOrDefault(te => te.Name == item);
            RecipeToolsAndEquipment newToolsOrEquipment = new(newRecipeId, toolAndEquipmentObject.Id, 1);
            newRecipe.RecipeToolsAndEquipments.Add(newToolsOrEquipment);
        }

        newRecipe.Cost = newRecipeCost;

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
        newIngredient.KitchenIngredients.Add(new KitchenIngredients(Guid.NewGuid(), newEntity.KitchenTypeId, newIngredientId, newItemAmount));

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
        }
    }

    public void HandleAddToolOrEquipment(Entity newEntity)
    {
        GetInputService.GetInput("Enter the name of the tool/equipment: ", out string newItemName);
        GetInputService.GetInput("Enter the cost of the tool/equipment: ", out double newItemCost);
        GetInputService.GetInput("Enter the amount of the tool/equipment you got: ", out double newItemAmount);

        Guid newToolAndEquipmentId = Guid.NewGuid();

        var newToolAndEquipment = new ToolsAndEquipment(newToolAndEquipmentId, newItemName, newItemCost);
        newToolAndEquipment.KitchenToolsAndEquipments.Add(new KitchenToolsAndEquipment(Guid.NewGuid(), newEntity.KitchenTypeId, newToolAndEquipmentId, newItemAmount));
        
        dataStore.InsertNewToolOrEquipment(newToolAndEquipment);
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
