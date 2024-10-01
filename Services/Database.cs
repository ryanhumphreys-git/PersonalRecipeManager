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
        var queryItem = from r in db.Recipes
                        join ri in db.RecipeIngredients on r.Id equals ri.RecipeId
                        join i in db.Ingredients on ri.ItemId equals i.Id
                        where r.Name == recipeString
                        select new RecipeIngredientsDTO
                        {
                            Name = i.Name,
                            UnitOfMeasurement = i.UnitOfMeasurement,
                            Quantity = ri.Quantity
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

    public bool CheckIfIngredientExists(string ingredientName)
    {
        using var db = new RecipeContext();
        string ingredient =  (from ki in db.KitchenIngredients
                            join i in db.Ingredients on ki.ItemId equals i.Id
                            where i.Name == ingredientName
                            select i.Name).ToString() ?? "not found";
        if(ingredient != "not found")
        {
            return false;
        }
        else
        {
            return true;
        }
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
            RecipeIngredients newRecipeItem = new(newRecipeId, itemIdToAdd[i], itemAmountToAdd[i]);
            db.RecipeIngredients.Add(newRecipeItem);
            db.SaveChanges();
        }
    }

    public void InsertRecipeEquipmentAndToolsFromList(List<string> newEquipment, Guid newRecipeId)
    {
        using var db = new RecipeContext();
        List<Guid> itemIdToAdd = new();
        foreach (var item in newEquipment)
        {
            var queryItemId = db.ToolsAndEquipment
                .Where(i => i.Name == item)
                .FirstOrDefault()
                .Id;
            
            itemIdToAdd.Add(queryItemId);
        }

        for (int i = 0; i < itemIdToAdd.Count(); i++)
        {
            RecipeToolsAndEquipment newRecipeItem = new(newRecipeId, itemIdToAdd[i], 1);
            db.RecipeToolsAndEquipment.Add(newRecipeItem);
            db.SaveChanges();
        }
    }

    public double GetRecipeCost(Guid nextId)
    {
        using var db = new RecipeContext();
        return (from i in db.Ingredients
                join ri in db.RecipeIngredients on i.Id equals ri.ItemId
                where ri.RecipeId == nextId
                select i.Cost).Sum();
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
        var currentKitchen = db.Entity.Single(e => e.Name == "Ryan").KitchenTypeId;
        db.KitchenIngredients.Add(new KitchenIngredients(Guid.NewGuid(), currentKitchen, newItem.Id, newItem.Quantity));
        db.SaveChanges();
    }

    public void UpdateIngredientQuantity(string name, double quantity)
    {
        using var db = new RecipeContext();
        Guid ingredientId = GetIngredientId(name);
        var ingredient = db.KitchenIngredients.SingleOrDefault(ki => ki.ItemId == ingredientId);
        ingredient.Quantity += quantity;
        var ingredientTab = db.Ingredients.SingleOrDefault(i => i.Id == ingredientId);
        ingredientTab.Quantity += quantity;
        db.SaveChanges();
    }

    public void InsertNewToolOrEquipment(ToolsAndEquipment newItem)
    {
        using var db = new RecipeContext();
        db.ToolsAndEquipment.Add(newItem);
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
        if(typeItem == "tool" || typeItem == "equipment")
        {
            return (from items in db.ToolsAndEquipment
                    select items.Name).ToList();
        }
        throw new ArgumentException("Incorrect input (non-user)");
    }

    public List<Recipes> SelectCurrentRecipes()
    {
        using var db = new RecipeContext();
        return db.Recipes.ToList();
    }

    public Guid GetIngredientId(string ingredientName)
    {
        using var db = new RecipeContext();
        return db.Ingredients
            .FirstOrDefault(i => i.Name == ingredientName)
            .Id;
    }

}