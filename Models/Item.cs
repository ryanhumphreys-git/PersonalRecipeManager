namespace PersonalRecipeManger.Models;

public partial class Item
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double Cost { get; set; }

    public Item(Guid id, string name, double cost)
    {
        Id = id;
        Name = name;
        Cost = cost;
    }
}
