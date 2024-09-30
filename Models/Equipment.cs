namespace PersonalRecipeManger.Models;

public class Equipment : Item
{
    public Equipment(Guid id, string name, double cost, double quantity)
        : base(id, name, cost, quantity)
    {

    }
}