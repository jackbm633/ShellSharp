namespace ShellSharp
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            Console.Write("$ ");
            Console.Out.Flush();

            // Wait for user input
            Console.ReadLine();
        }
    }
}
