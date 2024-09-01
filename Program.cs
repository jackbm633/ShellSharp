using System.Text.RegularExpressions;

int lastExitCode = 0;
Dictionary<string, Action<IEnumerable<string>>> builtins = new() { { "exit", Exit }, { "echo", Echo } };

while (true)
{
    Console.Write("$ ");
    await Console.Out.FlushAsync();

    // Wait for user input
    var command = SplitLine(Console.ReadLine()!);
    
    if (command.Any())
    {
        if (builtins.ContainsKey(command.First()))
        {
            builtins[command.First()].Invoke(command);
        }
        else
        {
            Console.WriteLine($"{command.First()}: command not found");
        }
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

void Echo(IEnumerable<string> commandInput)
{
    Console.WriteLine(string.Join(" ", commandInput.Skip(1)));
    lastExitCode = 0;
}

IEnumerable<string> SplitLine(string line)
{
    List<string> result = [];
    string pattern = "\"([^\"\\\\]*(?:\\\\.[^\"\\\\]*)*)\"|'([^'\\\\]*(?:\\\\.[^'\\\\]*)*)'|[^\\s]+";
    result.AddRange(Regex.Matches(line, pattern).Select(match =>
    {
        if (match.Groups[1].Value.Length > 0)
        {
            return match.Groups[1].Value;
        }
        return match.Groups[0].Value;
    }));
    return result;
}