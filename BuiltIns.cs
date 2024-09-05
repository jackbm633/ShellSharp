using System.Runtime.InteropServices;
using System.Globalization;
using System.Linq;

namespace ShellSharp
{
    public static class BuiltIns
    {


        private static readonly Dictionary<string, Func<IEnumerable<string>, int, int>> builtInsList = new() { { "exit", Exit }, { "echo", Echo }, { "type", TypeCommand } };

        public static Dictionary<string, Func<IEnumerable<string>, int, int>> BuiltInsList { get => builtInsList; }

        public static int Exit(IEnumerable<string> commandInput, int globalExitCode)
        {
            if (commandInput.Count() > 1 && byte.TryParse(commandInput.ElementAt(1), CultureInfo.CurrentCulture, out byte exitcode))
            {
                Environment.Exit(exitcode);
            }
            else
            {
                Environment.Exit(globalExitCode);
            }
            return 0;
        }

        public static int Echo(IEnumerable<string> commandInput, int globalExitCode)
        {
            Console.WriteLine(string.Join(" ", commandInput.Skip(1)));
            return 0;
        }


        public static int TypeCommand(IEnumerable<string> commandInput, int globalExitCode)
        {
            if (commandInput.Skip(1).Any())
            {
                var commandName = commandInput.Skip(1).First();        
                if (BuiltInsList.ContainsKey(commandName))
                {
                    Console.WriteLine($"{commandName} is a shell builtin");
                }
                else
                {
                    string? commandPath = GetCommandPath(commandName);
                    Console.WriteLine(commandPath is not null 
                        ? $"{commandName} is {commandPath}"
                        : $"{commandName}: not found");
                }
                return 0;
            }
            else
            {
                return 1;
            }
        }

        static string? GetCommandPath(string commandName)
        {
            string[] pathexts;
            pathexts = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? Environment.GetEnvironmentVariable("PATHEXT")!.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries) : [""];
            string[] path = Environment.GetEnvironmentVariable("PATH")!.Split(Path.PathSeparator);
            foreach (var (pathItem, pathext) in from string pathItem in path
                                                let pathext = pathexts.First(pathext => File.Exists(Path.Join(pathItem, commandName + pathext)))
                                                select (pathItem, pathext))
            {
                return Path.Join(pathItem, commandName + pathext);
            }

            return null;
        }

        
    }
}
