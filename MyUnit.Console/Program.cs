using MyUnit.Attributes;
using SystemArithmetic.Tests;

static void MessagePrint(string message, string result, bool isSucces, string? error = null)
{
    if (isSucces)
    {
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = ConsoleColor.White;
    }
    else
    {
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.White;
    }

    Console.Write(result);
    Console.ResetColor();

    Console.WriteLine(" "+message);

    if (error != null) { Console.WriteLine(error); }
   

}

MyTestRunner.TestPassed += (string data) =>
{
    MessagePrint(data, "Успех", true);
};

MyTestRunner.TestFailed += (string data, string error) =>
{
    MessagePrint(data, "Провал", false, error);
};

MyTestRunner.Run(typeof(SumOperationTest));