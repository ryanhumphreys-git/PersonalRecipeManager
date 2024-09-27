namespace PersonalRecipeManger.Models;

public partial class Item
{
    public Guid Id { get; set; }
    public Guid ItemTypeId { get; set; }
    public string Name { get; set; }
    public double Cost { get; set; }
    public double Quantity { get; set; }
    public string UnitOfMeasurement { get; set; }

    public Item(Guid id, string name, double cost, double quantity, Guid itemTypeId, string unitOfMeasurement)
    {
        Id = id;
        ItemTypeId = itemTypeId;
        Name = name;
        Cost = cost;
        Quantity = quantity;
        UnitOfMeasurement = unitOfMeasurement;
    }
}
