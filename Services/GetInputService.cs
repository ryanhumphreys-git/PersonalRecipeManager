using PersonalRecipeManger.Models;
namespace PersonalRecipeManger.Services;

public class GetInputService
{
    public static Entity GetNewEntityInformation()
    {
        Entity newEntity;
        GetInput("Please enter your name: ", out string name);
        GetInput("Please enter your age: ", out int age);
        GetInput("Please enter your cooking skill on a scale from 1-10", out int cookingSkill);
        GetInput("What type of kitchen do you have? (bare, current, dream)", out string kitchenType);

        Guid kitchen = kitchenType switch
        {
            "bare" => new Guid("49DCD928-895E-4C87-85FC-9623A838E361"),
            "current" => new Guid("5F12AA74-9257-475A-B444-F36A7D551833"),
            "dream" => new Guid("5F08301D-3693-4A64-86ED-CA941C8494F8"),
            _ => Guid.NewGuid()
        };

        newEntity = new Entity(Guid.NewGuid(), name, age, cookingSkill, kitchen);
        return newEntity;
    }

    public static void GetInput(string message, out double value)
    {
        bool isNotValid;
        value = 0;
        do
        {
            Console.WriteLine(message);
            isNotValid = false;
            try
            {
                string? input1 = Console.ReadLine();
                value = string.IsNullOrWhiteSpace(input1) ? 
                                throw new ArgumentException("The input must not be null") :
                                Convert.ToDouble(input1);
            }
            catch(Exception e)
            {
                isNotValid = true;
                Console.WriteLine($"{e.Message}");
            }
        } while (isNotValid);
    }
    public static void GetInput(string message, out string value)
    {
        bool isNotValid;
        value = string.Empty;
        do
        {
            Console.WriteLine(message);
            isNotValid = false;
            try
            {
                string? input1 = Console.ReadLine();
                value = string.IsNullOrWhiteSpace(input1) ?
                                throw new ArgumentException("The input must not be null") :
                                input1;
            }
            catch(Exception e)
            {
                isNotValid = true;
                Console.WriteLine($"{e.Message}");
            }
        } while (isNotValid);
    }
    public static void GetInput(string message, out int value)
    {
        bool isNotValid;
        value = 0;
        do
        {
            Console.WriteLine(message);
            isNotValid = false;
            try
            {
                string? input1 = Console.ReadLine();
                value = string.IsNullOrWhiteSpace(input1) ? 
                                throw new ArgumentException("The input must not be null") :
                                Convert.ToInt32(input1);
            }
            catch(Exception e)
            {
                isNotValid = true;
                Console.WriteLine($"{e.Message}");
            }
        } while (isNotValid);
    }
    public static void GetInput(string message, out bool value)
    {
        bool isNotValid;
        value = false;
        do
        {
            Console.WriteLine(message);
            isNotValid = false;
            try
            {
                string? input1 = Console.ReadLine();
                if (!(input1 == "Y" || input1 == "N"))
                {
                    throw new ArgumentException("You must choose Y or N");
                }
                value = input1 == "Y";
            }
            catch(Exception e)
            {
                isNotValid = true;
                Console.WriteLine($"{e.Message}");
            }
        } while (isNotValid);
    }

    public static void GetInput(string message, out Dictionary<string, double> value)
    {
        bool isNotValid;
        value = new Dictionary<string, double>();
        do
        {
            Console.WriteLine(message);
            isNotValid = false;
            try
            {
                List<string> keyList = new();
                List<double> valueList = new();
                do
                {
                    GetInput("Input an item: (use end to stop adding ingredients)", out string newIngredient);
                    if(newIngredient == "end")
                    {
                        break;
                    }
                    keyList.Add(newIngredient);
                    GetInput("How much of that item do you need (no units)", out double amount);
                    valueList.Add(amount);
                } while(keyList.Count < 100);
                
                for(int i = 0; i < keyList.Count(); i++)
                {
                    value.Add(keyList[i], valueList[i]);
                }

            }
            catch(Exception e)
            {
                isNotValid = true;
                Console.WriteLine($"{e.Message}");
            }
        } while(isNotValid);
    }

    public static void GetInput(string message, out List<string> value)
    {
        bool isNotValid;
        value = new List<string>();
        do
        {
            Console.WriteLine(message);
            isNotValid = false;
            try
            {
                do
                {
                    GetInput("Input an item: (use end to stop adding items)", out string newItem);
                    if(newItem == "end")
                    {
                        break;
                    }
                    value.Add(newItem);
                } while(value.Count < 100);
            }
            catch(Exception e)
            {
                isNotValid = true;
                Console.WriteLine($"{e.Message}");
            }
        } while(isNotValid);
    }

}