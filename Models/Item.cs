namespace PersonalRecipeManger.Models;

public partial class Item
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double Cost { get; set; }
    public double Quantity { get; set; }

    public Item(Guid id, string name, double cost, double quantity)
    {
        Id = id;
        Name = name;
        Cost = cost;
        Quantity = quantity;
    }
}
