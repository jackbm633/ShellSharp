Console.Write("$ ");
await Console.Out.FlushAsync();

// Wait for user input
var command = Console.ReadLine();
Console.WriteLine($"{command}: command not found");
