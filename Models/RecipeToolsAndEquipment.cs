namespace PersonalRecipeManger.Models;

public partial class RecipeToolsAndEquipment
{
    public Guid AutoId { get; set; }
    public Guid RecipeId { get; set; }
    public Guid ItemId { get; set; }
    public double Quantity { get; set; }

    public RecipeToolsAndEquipment(Guid recipeId, Guid itemId, double quantity)
    {
        RecipeId = recipeId;
        ItemId = itemId;
        Quantity = quantity;
    }
}
