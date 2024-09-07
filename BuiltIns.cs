using System.Runtime.InteropServices;
using System.Globalization;

namespace ShellSharp
{
    /// <summary>
    /// Contains built-in shell commands.
    /// </summary>
    public static class BuiltIns
    {
        private static readonly Dictionary<string, Func<IEnumerable<string>, int, int>> builtInsList = 
            new() { { "exit", Exit }, { "echo", Echo }, { "type", TypeCommand } };

        /// <summary>
        /// Getter for the list of builtins.
        /// </summary>
        public static Dictionary<string, Func<IEnumerable<string>, int, int>> 
            BuiltInsList { get => builtInsList; }
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

        public static string? GetCommandPath(string commandName)
        {
            string[] pathexts;
            pathexts = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? 
                Environment.GetEnvironmentVariable("PATHEXT")!
                .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries) : [""];
            string[] path = Environment.GetEnvironmentVariable("PATH")!.Split(Path.PathSeparator);
            string? result = (from pathItem in path
              from pathext in pathexts
              let fullPath = Path.Join(pathItem, commandName + pathext)
              where File.Exists(fullPath)
              select fullPath).FirstOrDefault();
            return result;
        }

        
    }
}
