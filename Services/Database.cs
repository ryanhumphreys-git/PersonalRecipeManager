using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using PersonalRecipeManger.DatabaseModels;
using PersonalRecipeManger.Models;

namespace PersonalRecipeManger.Services;

public class SqlDatabase : IDataStore
{
    // GETS
   public Entity GetEntity(string name)
   {
        using var db = new RecipeContext();
        return db.Entity.Where(e => e.Name == name).FirstOrDefault();
   }

   public double GetRecipeCost(Guid nextId)
    {
        using var db = new RecipeContext();
        return (from i in db.Ingredients
                join ri in db.RecipeIngredients on i.Id equals ri.IngredientId
                where ri.RecipeId == nextId
                select i.Cost).Sum();
    }

   public List<RecipeIngredientsDTO> GetRecipeIngredients(string recipeString)
    {
        using var db = new RecipeContext();
        var queryItem = from r in db.Recipes
                        join ri in db.RecipeIngredients on r.Id equals ri.RecipeId
                        join i in db.Ingredients on ri.IngredientId equals i.Id
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

    public Ingredients GetIngredientByName(string ingredientName)
    {
        using var db = new RecipeContext();
        return db.Ingredients
            .FirstOrDefault(i => i.Name == ingredientName);
    }

    public KitchenIngredients GetKitchenIngredientById(Guid ingredientId)
    {
        using var db = new RecipeContext();
        return db.KitchenIngredients
            .FirstOrDefault(ki => ki.IngredientId == ingredientId);
    }

    public ToolsAndEquipment GetToolsAndEquipmentByName(string toolName)
    {
        using var db = new RecipeContext();
        return db.ToolsAndEquipment
            .FirstOrDefault(te => te.Name == toolName);
    }


    // CHECKS
    public bool CheckIfRecipeExists(string recipeName)
    {
        using var db = new RecipeContext();
        return db.Recipes
            .Any(r => r.Name == recipeName);
    }    

    public bool CheckIfIngredientExists(Ingredients ingredients)
    {
        using var db = new RecipeContext();
        return db.Ingredients
                .Any(ki => ki.Name == ingredients.Name);
    } 

    // INSERTS
    public void InsertRecipeIngredient(RecipeIngredients newRecipeItem)
    {
        using var db = new RecipeContext();
        db.RecipeIngredients.Add(newRecipeItem);
        db.SaveChanges();
    }

    public void InsertRecipeEquipmentOrTools(RecipeToolsAndEquipment newToolOrEquipment)
    {
        using var db = new RecipeContext();
        db.RecipeToolsAndEquipment.Add(newToolOrEquipment);
            db.SaveChanges();
    }

    public void InsertNewRecipe(Recipes newRecipe)
    {
        using var db = new RecipeContext();
        db.Recipes.Add(newRecipe);
        db.SaveChanges();
    }

    public void InsertNewIngredient(Ingredients newIngredient)
    {
        using var db = new RecipeContext();
        db.Ingredients.Add(newIngredient);
        db.SaveChanges();
    }

    public void InsertNewKitchenIngredient(KitchenIngredients newIngredient)
    {
        using var db = new RecipeContext();
        db.KitchenIngredients.Add(newIngredient);
        db.SaveChanges();
    }

    public void InsertNewToolOrEquipment(ToolsAndEquipment newItem)
    {
        using var db = new RecipeContext();
        db.ToolsAndEquipment.Add(newItem);
        db.SaveChanges();
    }

    public void InsertKitchenNewToolOrEquipment(KitchenToolsAndEquipment newToolOrEquipment)
    {
        using var db = new RecipeContext();
        db.KitchenToolsAndEquipment.Add(newToolOrEquipment);
        db.SaveChanges();
    }

    // UPDATES
    public void UpdateKitchenIngredientQuantity(KitchenIngredients newIngredient)
    {
        using var db = new RecipeContext();
        db.KitchenIngredients
            .Where(ki => ki.IngredientId == newIngredient.IngredientId)
            .ExecuteUpdate(setters => setters
                .SetProperty(ki => ki.Quantity, newIngredient.Quantity));
    }

    public void UpdateIngredientProperties(Ingredients newIngredient)
    {
        using var db = new RecipeContext();
        db.Ingredients
            .Where(i => i.Id == newIngredient.Id)
            .ExecuteUpdate(setters => setters
                .SetProperty(i => i.Cost, newIngredient.Cost)
                .SetProperty(i => i.UnitOfMeasurement, newIngredient.UnitOfMeasurement));
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

    // SELECTS
    public List<Ingredients> SelectAllIngredients()
    {
        using var db = new RecipeContext();
        return db.Ingredients.ToList();
    }

    public List<ToolsAndEquipment> SelectAllToolsAndEquipment()
    {
        using var db = new RecipeContext();
        return db.ToolsAndEquipment.ToList();
    }

    public List<Recipes> SelectAllKnownRecipes()
    {
        using var db = new RecipeContext();
        return db.Recipes.ToList();
    }
}