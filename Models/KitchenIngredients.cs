namespace PersonalRecipeManger.Models;

public partial class KitchenIngredients
{
    public Guid AutoId { get; set; }
    public Guid KitchenId { get; set; }
    public Guid IngredientId { get; set; }
    public double Quantity { get; set; }
    public virtual Ingredients? Ingredient { get; set; }
    public virtual KitchenType? Kitchen { get; set; }

    public KitchenIngredients(Guid autoId, Guid kitchenId, Guid ingredientId, double quantity)
    {
        AutoId = autoId;
        KitchenId = kitchenId;
        IngredientId = ingredientId;
        Quantity = quantity;
    }
}
