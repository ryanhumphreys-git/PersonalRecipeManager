namespace PersonalRecipeManger.Models;

public partial class KitchenItems
{
    public int AutoId { get; set; }
    public int KitchenId { get; set; }
    public int ItemId { get; set; }
    public decimal Quantity { get; set; }
}
