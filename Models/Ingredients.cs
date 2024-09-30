namespace PersonalRecipeManger.Models;

public class Ingredients : Item
{
    public string UnitOfMeasurement { get; set; }
    public Ingredients(Guid id, string name, double cost, double quantity, string unitOfMeasurement)
        : base(id, name, cost, quantity)
    {
        UnitOfMeasurement = unitOfMeasurement;
    }
}