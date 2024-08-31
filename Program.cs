int lastExitCode = 0;
Dictionary<string, Action<IEnumerable<string>>> builtins = new() { { "exit", Exit } };

while (true)
{
    Console.Write("$ ");
    await Console.Out.FlushAsync();

    // Wait for user input
    var command = Console.ReadLine()!.TrimEnd().Split(" ").Where(s => s.Length > 0);
    
    if (command.Any())
    {
        if (builtins.ContainsKey(command.First()))
        {
            builtins[command.First()].Invoke(command);
        }
        Console.WriteLine($"{command.First()}: command not found");
    }
}


void Exit(IEnumerable<string> commandInput)
{
    if (commandInput.Count() > 1 && byte.TryParse(commandInput.ElementAt(1), out byte exitcode))
    {
        Environment.Exit(exitcode);
    }
    else
    {
        Environment.Exit(lastExitCode);
    }
}