namespace PersonalRecipeManger.Models;

public partial class KitchenIngredients
{
    public Guid AutoId { get; set; }
    public Guid KitchenId { get; set; }
    public Guid ItemId { get; set; }
    public double Quantity { get; set; }

    public KitchenIngredients(Guid autoId, Guid kitchenId, Guid itemId, double quantity)
    {
        AutoId = autoId;
        KitchenId = kitchenId;
        ItemId = itemId;
        Quantity = quantity;
    }
}
