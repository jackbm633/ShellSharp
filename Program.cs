using ShellSharp;

int lastExitCode = 0;

while (true)
{
    Console.Write("$ ");
    await Console.Out.FlushAsync();

    // Wait for user input
    var command = Utils.SplitLine(Console.ReadLine()!);
    
    if (command.Any())
    {
        if (BuiltIns.BuiltInsList.ContainsKey(command.First()))
        {
            lastExitCode = BuiltIns.BuiltInsList[command.First()].Invoke(command, lastExitCode);
        }
        else
        {
            Console.WriteLine($"{command.First()}: command not found");
        }
    }
}


