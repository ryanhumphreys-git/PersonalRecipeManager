namespace PersonalRecipeManger.Models;

public partial class KitchenToolsAndEquipment
{
    public Guid AutoId { get; set; }
    public Guid KitchenId { get; set; }
    public Guid ItemId { get; set; }
    public decimal Quantity { get; set; }
}
