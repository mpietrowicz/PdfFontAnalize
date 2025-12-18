using AnalysisOfAspose;

var worker = new AsposeWorker() as IWorker;

var outputFolderLinux = "/data";
if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
{
    outputFolderLinux = $"results/{DateTime.UtcNow:yyyyMMdd_HHmmss}";
    Console.WriteLine("Running on Windows OS");
    if (!Directory.Exists(outputFolderLinux))
    {
        Directory.CreateDirectory(outputFolderLinux);
    }
}
else
{
    Console.WriteLine("Running on Linux-based OS");
}
worker.SetOutputFolder(outputFolderLinux);

Console.WriteLine("--------------------------------------------------");
Console.WriteLine("-----KONWERSJA BEZ DODATKOWYCH CZCIONEK-----------");
Console.WriteLine("--------------------------------------------------");

Console.WriteLine("Converting document without additional fonts...");
string v = await worker.ConvertToPdf("DocxFiles/TestCzcionek.docx","nofonts");
Console.WriteLine($"Converted file path: {v}");

Console.WriteLine("--------------------------------------------------");
Console.WriteLine("-----KONWERSJA Z DODATKOWYMI CZCIONKAMI-----------");
Console.WriteLine("--------------------------------------------------");
//var windowsFontsDir = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);

var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
var directory = Path.GetDirectoryName(location) ?? "";
var aptosFontsDir = Path.Combine(directory, "Fonts");
Console.WriteLine($"Using fonts from: {aptosFontsDir}");
var worker2 = new AsposeWorker(aptosFontsDir) as IWorker;
worker2.SetOutputFolder(outputFolderLinux);

string v2 = await worker2.ConvertToPdf("DocxFiles/TestCzcionek.docx","addfonts");