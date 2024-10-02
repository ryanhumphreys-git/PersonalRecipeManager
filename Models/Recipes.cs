namespace PersonalRecipeManger.Models;

public partial class Recipes
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Difficulty { get; set; }
    public double Time { get; set; }
    public double Cost { get; set; }
    public virtual ICollection<RecipeIngredients> RecipeIngredients { get; set; } = new List<RecipeIngredients>();
    public virtual ICollection<RecipeToolsAndEquipment> RecipeToolsAndEquipments { get; set; } = new List<RecipeToolsAndEquipment>();
    public Recipes(Guid id, string name, int difficulty, double time, double cost)
    {
        Id = id;
        Name = name;
        Difficulty = difficulty;
        Time = time;
        Cost = cost;
    }
}
