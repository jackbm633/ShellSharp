using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

int lastExitCode = 0;
Dictionary<string, Action<IEnumerable<string>>> builtins = [];
builtins = new() { { "exit", Exit }, { "echo", Echo }, { "type", TypeCommand } };

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


void TypeCommand(IEnumerable<string> commandInput)
{
    if (commandInput.Skip(1).Any())
    {
        var commandName = commandInput.Skip(1).First();        
        if (builtins.ContainsKey(commandName))
        {
            Console.WriteLine($"{commandName} is a shell builtin");
        }
        else
        {
            string? commandPath = GetCommandPath(commandName);
            if (commandPath is not null)
            {
                Console.WriteLine($"{commandName} is {commandPath}"); 
            }
            else
            {
                Console.WriteLine($"{commandName}: not found");
            }
        }
    }
    else
    {
        lastExitCode = 1;
    }
}

string? GetCommandPath(string commandName)
{
    string[] pathexts;
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        pathexts = Environment.GetEnvironmentVariable("PATHEXT")!.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);
    }
    else
    {
        pathexts = [""];
    }
    string[] path = Environment.GetEnvironmentVariable("PATH")!.Split(Path.PathSeparator);
    foreach (string pathItem in path)
    {
        foreach (string pathext in pathexts)
        {
            if (File.Exists(Path.Join(pathItem, commandName + pathext)))
            {
                return Path.Join(pathItem, commandName + pathext);
            }
        }
    }
    return null;
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