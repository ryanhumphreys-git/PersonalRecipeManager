using PersonalRecipeManger.Models;
using PersonalRecipeManger.Services;

public class Program
{
    static void Main()
    {
        
        HandleUserInput handleUserInput = new(new SqlDatabase());
        Entity? newEntity = null;

        Console.WriteLine("Welcome to your recipe manager.");
        Console.WriteLine("Keep track and organize your recipes, inventory, and equipment.");

        Console.WriteLine("");

        GetInputService.GetInput("Would you like to load your account information? (Y/N)", out bool loadInfo);
        if(loadInfo)
        {
            // GetInputService.GetInput("Enter entity name", out string name);
            newEntity = handleUserInput.LoadEntityInformation("Ryan");
        }
        if (newEntity is null)
        {
            if(loadInfo)
            {
                Console.WriteLine("That person does not exist, please create a new user:");
            }
            
            newEntity = GetInputService.GetNewEntityInformation();
        }

        ListCommands();

        while (true)
        {
            Console.WriteLine("Input a command: ");
            
            string? input = Console.ReadLine();

            if(input == "update")
            {
                newEntity = handleUserInput.HandleUpdate(newEntity);
            }
            if(input == "show")
            {
                handleUserInput.HandleShow(newEntity);
            }
            if (input == "show recipe")
            {
                handleUserInput.HandleShowRecipe();
            }
            if (input == "add ingredient")
            {
                handleUserInput.HandleAddIngredient();
            }
            if (input == "add tool" || input == "add equipment")
            {
                handleUserInput.HandleAddToolOrEquipment();
            }
            if (input == "add recipe")
            {
                handleUserInput.HandleAddRecipe();
            }
            if (input == "help")
            {
                ListCommands();
            }
            Console.WriteLine();
        }
    }

    private static void ListCommands()
    {
        Console.WriteLine("Commands:");
        Console.WriteLine("'show' - brings you to the menu to show recipes, ingredients, tools, equipment");
        Console.WriteLine("'add <input>' - allows you to add a recipe, ingredient, tool, or equipment");
        Console.WriteLine("'update' - allows you to re enter your personal information");
        Console.WriteLine("To view commands type 'help'");
    }
}