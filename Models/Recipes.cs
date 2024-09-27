namespace PersonalRecipeManger.Models;

public partial class Recipes
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Difficulty { get; set; }
    public double Time { get; set; }
    public double Cost { get; set; }

    public Recipes(Guid id, string name, int difficulty, double time, double cost)
    {
        Id = id;
        Name = name;
        Difficulty = difficulty;
        Time = time;
        Cost = cost;
    }
}
