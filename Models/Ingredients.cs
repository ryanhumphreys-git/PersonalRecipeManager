namespace PersonalRecipeManger.Models;

public class Ingredients : Item
{
    public string UnitOfMeasurement { get; set; }
    public virtual ICollection<KitchenIngredients> KitchenIngredients { get; set; } = new List<KitchenIngredients>();
    public virtual ICollection<RecipeIngredients> RecipeIngredients { get; set; } = new List<RecipeIngredients>();

    public Ingredients(Guid id, string name, double cost, string unitOfMeasurement)
        : base(id, name, cost)
    {
        UnitOfMeasurement = unitOfMeasurement;
    }
}