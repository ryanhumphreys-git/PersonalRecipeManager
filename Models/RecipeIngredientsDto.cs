namespace PersonalRecipeManger.Models;

public class RecipeIngredientsDTO
{
    public string Name { get; set; } = string.Empty;
    public double Quantity { get; set; }
    public string UnitOfMeasurement { get; set; } = string.Empty;
}