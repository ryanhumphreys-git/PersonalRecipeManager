namespace PersonalRecipeManger.Models;

public class ToolsAndEquipment : Item
{
    public ToolsAndEquipment(Guid id, string name, double cost, double quantity)
        : base(id, name, cost, quantity)
    {

    }
}