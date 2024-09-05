using System.Text.RegularExpressions;

namespace ShellSharp
{
    public static partial class Utils
    {
        public static IEnumerable<string> SplitLine(string line)
        {
            List<string> result = [];
            result.AddRange(SplitRegex().Matches(line).Select(match =>
            {
                if (match.Groups[1].Value.Length > 0)
                {
                    return match.Groups[1].Value;
                }
                return match.Groups[0].Value;
            }));
            return result;
        }

        [GeneratedRegex("\"([^\"\\\\]*(?:\\\\.[^\"\\\\]*)*)\"|'([^'\\\\]*(?:\\\\.[^'\\\\]*)*)'|[^\\s]+")]
        private static partial Regex SplitRegex();
    }
}
