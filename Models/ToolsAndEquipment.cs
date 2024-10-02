namespace PersonalRecipeManger.Models;

public class ToolsAndEquipment : Item
{
    public virtual ICollection<KitchenToolsAndEquipment> KitchenToolsAndEquipments { get; set; } = new List<KitchenToolsAndEquipment>();
    public virtual ICollection<RecipeToolsAndEquipment> RecipeToolsAndEquipments { get; set; } = new List<RecipeToolsAndEquipment>();
    public ToolsAndEquipment(Guid id, string name, double cost)
        : base(id, name, cost)
    {

    }
}