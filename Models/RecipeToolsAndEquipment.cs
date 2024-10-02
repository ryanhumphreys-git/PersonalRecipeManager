namespace PersonalRecipeManger.Models;

public partial class RecipeToolsAndEquipment
{
    public Guid AutoId { get; set; }
    public Guid RecipeId { get; set; }
    public Guid ToolsAndEquipmentId { get; set; }
    public double Quantity { get; set; }
    public virtual Recipes? Recipe { get; set; }
    public virtual ToolsAndEquipment? ToolsAndEquipment { get; set; }

    public RecipeToolsAndEquipment(Guid recipeId, Guid toolsAndEquipmentId, double quantity)
    {
        RecipeId = recipeId;
        ToolsAndEquipmentId = toolsAndEquipmentId;
        Quantity = quantity;
    }
}
