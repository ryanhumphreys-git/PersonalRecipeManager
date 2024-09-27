using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalRecipeManger.Models;
public class Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public int CookingSkill { get; set; }
    public int KitchenTypeId { get; set; }
    [NotMapped]
    public Kitchen EntityKitchen { get; set; }
    [NotMapped]
    public ShoppingList EntityShoppingList { get; set; }
    [NotMapped]
    public List<Recipes> RecipeList { get; set; } = new();

    public Entity(Guid id, string name, int age, int cookingSkill,
                  int kitchenTypeId)
    {
        Id = id;
        Name = name;
        Age = age;
        CookingSkill = cookingSkill;
        KitchenTypeId = kitchenTypeId;
    }
}