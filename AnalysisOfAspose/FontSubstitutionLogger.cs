using System.Text;
using Aspose.Words;

namespace AnalysisOfAspose;

public sealed class FontSubstitutionLogger : IWarningCallback
{
    private readonly List<string> _lines = new();

    public void Warning(WarningInfo info)
    {
        if (info.WarningType == WarningType.FontSubstitution)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Black;
             var message = $"[{info.WarningType}]{DateTime.UtcNow:O} | {info.Description}";
             _lines.Add(message);
            Console.WriteLine(message);
            Console.ResetColor();
        }
        else
        {

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Black;
            var message = $"[{info.WarningType}]{DateTime.UtcNow:O} | {info.Description}";
             _lines.Add(message);
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

    public void Save(string path)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllLines(path, _lines, Encoding.UTF8);
    }
}
