namespace PersonalRecipeManger.Models;

public partial class KitchenToolsAndEquipment
{
    public Guid AutoId { get; set; }
    public Guid KitchenId { get; set; }
    public Guid ToolsAndEquipmentId { get; set; }
    public decimal Quantity { get; set; }
    public virtual KitchenType? Kitchen { get; set; }
    public virtual ToolsAndEquipment? ToolsAndEquipment { get; set; }
}
