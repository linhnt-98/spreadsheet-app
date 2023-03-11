using System;

namespace HW5
{
    /// <summary>
    /// The main program.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            bool running = true;
            string expression = string.Empty;
            ExpressionTree expressionTree = new ExpressionTree(expression);

            while (running)
            {
                Console.WriteLine(string.Empty);
                Console.WriteLine($"Menu (current expression=\"{expression}\")");
                Console.WriteLine("  1 = Enter a new expression");
                Console.WriteLine("  2 = Set a variable value");
                Console.WriteLine("  3 = Evaluate Tree");
                Console.WriteLine("  4 = Quit");

                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    continue;
                switch (input)
                {
                    case "1":
                        Console.Write("Enter new expression: ");
                        var expr = Console.ReadLine();
                        if (string.IsNullOrEmpty(expr))
                        {
                            Console.WriteLine("Expression must not be empty");
                            continue;
                        }
                        expression = expr;
                        expressionTree = new ExpressionTree(expression);
                        break;

                    case "2":
                        if (string.IsNullOrEmpty(expression))
                        {
                            Console.WriteLine("No expression to edit.");
                            continue;
                        }

                        Console.Write("Enter variable name: ");
                        string variable = Console.ReadLine();
                        Console.Write("Enter variable value: ");
                        string valueArg = Console.ReadLine();

                        if (string.IsNullOrEmpty(variable) || string.IsNullOrEmpty(valueArg))
                        {
                            Console.WriteLine("Invalid input");
                            continue;
                        }

                        if (double.TryParse(valueArg, out double value))
                        {
                            expressionTree.SetVariable(variable, value);
                        }
                        else
                        {
                            Console.WriteLine("Not a valid value.");
                            continue;
                        }
                        break;

                    case "3":
                        Console.WriteLine($"Expression Tree Value: {expressionTree.Evaluate()}");
                        break;

                    case "4":
                        Console.WriteLine("Done");
                        running = false;
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
