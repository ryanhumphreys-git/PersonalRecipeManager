namespace PersonalRecipeManger.Models;

public partial class KitchenType
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual ICollection<KitchenIngredients> KitchenIngredients { get; set; } = new List<KitchenIngredients>();
    public virtual ICollection<KitchenToolsAndEquipment> KitchenToolsAndEquipments { get; set; } = new List<KitchenToolsAndEquipment>();
}
