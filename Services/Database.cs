using Microsoft.EntityFrameworkCore;
using PersonalRecipeManger.DatabaseModels;
using PersonalRecipeManger.Models;

namespace PersonalRecipeManger.Services;

public class Database : IDataStore
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
                        join items in db.Items on recipeItems.ItemId equals items.Id
                        where recipes.Name == recipeString
                        select new RecipeIngredientsDTO
                        {
                            Name = items.Name,
                            UnitOfMeasurement = items.UnitOfMeasurement,
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

    public void InsertRecipeItemsFromDictionary(Dictionary<string, double> newItems, Guid newRecipeId)
    {
        using var db = new RecipeContext();
        List<Guid> itemIdToAdd = new();
        List<double> itemAmountToAdd = new();
        foreach (var item in newItems)
        {
            var queryItemId = db.Items.Where(i => i.Name == item.Key).FirstOrDefault().Id;
            
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

    public double GetRecipeCost(Guid nextId)
    {
        using var db = new RecipeContext();
        return (from items in db.Items
                join recipeItems in db.RecipeItems on items.Id equals recipeItems.ItemId
                where items.ItemTypeId == 1 && recipeItems.RecipeId == nextId
                select items.Cost).Sum();
    }

    public void InsertNewRecipe(Recipes newRecipe)
    {
        using var db = new RecipeContext();
        db.Add(newRecipe);
        db.SaveChanges();
    }

    public void InsertNewItem(Item newItem)
    {
        using var db = new RecipeContext();
        db.Items.Add(newItem);
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

    public List<string> SelectItemsOfType(int typeItem)
    {
        using var db = new RecipeContext();
        return (from items in db.Items
                where items.ItemTypeId == typeItem
                select items.Name).ToList();
    }

    public List<Recipes> SelectCurrentRecipes()
    {
        using var db = new RecipeContext();
        return db.Recipes.ToList();
    }

}