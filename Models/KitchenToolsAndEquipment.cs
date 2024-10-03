namespace PersonalRecipeManger.Models;

public partial class KitchenToolsAndEquipment
{
    public Guid AutoId { get; set; }
    public Guid KitchenId { get; set; }
    public Guid ToolsAndEquipmentId { get; set; }
    public double Quantity { get; set; }
    public virtual KitchenType? Kitchen { get; set; }
    public virtual ToolsAndEquipment? ToolsAndEquipment { get; set; }

    public KitchenToolsAndEquipment(Guid autoId, Guid kitchenId, Guid toolAndEquipmentId, double quantity)
    {
        AutoId = autoId;
        KitchenId = kitchenId;
        ToolsAndEquipmentId = toolAndEquipmentId;
        Quantity = quantity;
    }
}
