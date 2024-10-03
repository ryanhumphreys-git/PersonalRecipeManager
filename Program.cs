using PersonalRecipeManger.Models;
using PersonalRecipeManger.Services;

public class Program
{
    /* 
        TODO: 
        1. Add current and dream kitchen types
            - update database with my personal current kitchen equipment/ingredients
            - insert more recipes that I know and use
        2. think about breakdown of ingredient to things like
            - condiment
            - produce
            - meats
            - pasta (starch?)
            - look online for categories
        3. Add shopping list functionality
            - can only shop for items in ingredients or toolsandequipment list
            - way to store/complete shopping list
            - when completed, archive list and add ingredients to kitchen
        4. Make use of foreign key relationships within code to further improve database performance and readability
    */

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
            handleUserInput.RefreshItems();

            Console.WriteLine("Input a command: ");
            
            string? input = Console.ReadLine();

            if(input == "update")
            {
                newEntity = handleUserInput.HandleUpdateEntity(newEntity);
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
                handleUserInput.HandleAddIngredient(newEntity);
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
            if (input == "refresh")
            {
                handleUserInput.RefreshItems();
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