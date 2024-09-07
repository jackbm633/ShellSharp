using System.Diagnostics;
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
        lastExitCode = await RunCommand(lastExitCode, command);
    }
}

static async Task<int> RunCommand(int lastExitCode, IEnumerable<string> command)
{
    if (BuiltIns.BuiltInsList.ContainsKey(command.First()))
    {
        lastExitCode = BuiltIns.BuiltInsList[command.First()].Invoke(command, lastExitCode);
    }
    else
    {
        var path = BuiltIns.GetCommandPath(command.First());
        if (path is not null)
        {
            var cmd = new Process();
            cmd.StartInfo.FileName = path;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.Arguments = string.Join(" ", command.Skip(1));
            cmd.Start();
            Console.WriteLine(await cmd.StandardOutput.ReadToEndAsync());
            await cmd.WaitForExitAsync();
            return cmd.ExitCode;
        }
        else
        {
            Console.WriteLine($"{command.First()}: command not found");
        }
    }

    return lastExitCode;
}
