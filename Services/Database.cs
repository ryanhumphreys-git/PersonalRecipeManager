using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using PersonalRecipeManger.DatabaseModels;
using PersonalRecipeManger.Models;

namespace PersonalRecipeManger.Services;

public class SqlDatabase : IDataStore
{
   public Entity GetEntity(string name)
   {
        using var db = new RecipeContext();
        return db.Entity.Where(e => e.Name == name).FirstOrDefault();
   }

   public List<RecipeIngredientsDTO> GetRecipeIngredients(string recipeString)
    {
        using var db = new RecipeContext();
        var queryItem = from recipes in db.Recipes
                        join recipeItems in db.RecipeItems on recipes.Id equals recipeItems.RecipeId
                        join ingredients in db.Ingredients on recipeItems.ItemId equals ingredients.Id
                        where recipes.Name == recipeString
                        select new RecipeIngredientsDTO
                        {
                            Name = ingredients.Name,
                            UnitOfMeasurement = ingredients.UnitOfMeasurement,
                            Quantity = recipeItems.Quantity
                        };
        List<RecipeIngredientsDTO> recipeIngredients = queryItem.ToList();
        return recipeIngredients;
    }

    public string CheckIfRecipeExists(string recipeName)
    {
        using var db = new RecipeContext();
        return db.Recipes
            .FirstOrDefault(r => r.Name == recipeName)
            ?.Name ?? "not found";
    }    

    public void InsertRecipeIngredientsFromDictionary(Dictionary<string, double> newItems, Guid newRecipeId)
    {
        using var db = new RecipeContext();
        List<Guid> itemIdToAdd = new();
        List<double> itemAmountToAdd = new();
        foreach (var item in newItems)
        {
            var queryItemId = db.Ingredients.Where(i => i.Name == item.Key).FirstOrDefault().Id;
            
            itemIdToAdd.Add(queryItemId);
            itemAmountToAdd.Add(item.Value);
        }

        for (int i = 0; i < itemIdToAdd.Count(); i++)
        {
            RecipeItems newRecipeItem = new(newRecipeId, itemIdToAdd[i], itemAmountToAdd[i]);
            db.RecipeItems.Add(newRecipeItem);
            db.SaveChanges();
        }
    }

    public void InsertRecipeEquipmentAndToolsFromList(List<string> newEquipment, Guid newRecipeId)
    {
        using var db = new RecipeContext();
        List<Guid> itemIdToAdd = new();
        foreach (var item in newEquipment)
        {
            var queryItemId = db.Equipment
                .Select(x => new { x.Id, x.Name, x.Cost, x.Quantity})
                .Union(db.Tools
                    .Select(x => new { x.Id, x.Name, x.Cost, x.Quantity}))
                .Where(i => i.Name == item)
                .FirstOrDefault()
                .Id;
            
            itemIdToAdd.Add(queryItemId);
        }

        for (int i = 0; i < itemIdToAdd.Count(); i++)
        {
            RecipeItems newRecipeItem = new(newRecipeId, itemIdToAdd[i], 1);
            db.RecipeItems.Add(newRecipeItem);
            db.SaveChanges();
        }
    }

    public double GetRecipeCost(Guid nextId)
    {
        using var db = new RecipeContext();
        return (from items in db.Ingredients
                join recipeItems in db.RecipeItems on items.Id equals recipeItems.ItemId
                where recipeItems.RecipeId == nextId
                select items.Cost).Sum();
    }

    public void InsertNewRecipe(Recipes newRecipe)
    {
        using var db = new RecipeContext();
        db.Add(newRecipe);
        db.SaveChanges();
    }

    /* Would need functions for each item type with seperate item classes */
    public void InsertNewIngredient(Ingredients newItem)
    {
        using var db = new RecipeContext();
        db.Ingredients.Add(newItem);
        db.SaveChanges();
    }

    public void InsertNewTool(Tools newItem)
    {
        using var db = new RecipeContext();
        db.Tools.Add(newItem);
        db.SaveChanges();
    }

    public void InsertNewEquipment(Equipment newItem)
    {
        using var db = new RecipeContext();
        db.Equipment.Add(newItem);
        db.SaveChanges();
    }

    public void UpdateEntityInformation(Entity newEntity, Guid id)
    {
        using var db = new RecipeContext();
        db.Entity
            .Where(e => e.Id == id)
            .ExecuteUpdate(setters => setters
                .SetProperty(e => e.Name, newEntity.Name)
                .SetProperty(e => e.Age, newEntity.Age)
                .SetProperty(e => e.CookingSkill, newEntity.CookingSkill)
                .SetProperty(e => e.KitchenTypeId, newEntity.KitchenTypeId));
    }

    public List<string> SelectItemsOfType(string typeItem)
    {
        using var db = new RecipeContext();
        if(typeItem == "ingredient")
        {
            return (from items in db.Ingredients
                    select items.Name).ToList();
        }
        if(typeItem == "tool")
        {
            return (from items in db.Tools
                    select items.Name).ToList();
        }
        if(typeItem == "equipment")
        {
            return (from items in db.Equipment
                    select items.Name).ToList();
        }
        throw new ArgumentException("Incorrect input (non-user)");
    }

    public List<Recipes> SelectCurrentRecipes()
    {
        using var db = new RecipeContext();
        return db.Recipes.ToList();
    }

}