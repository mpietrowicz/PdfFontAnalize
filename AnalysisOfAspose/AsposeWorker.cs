using Aspose.Words;
using Aspose.Words.Fonts;

namespace AnalysisOfAspose;

public class AsposeWorker : IWorker
{
    Aspose.Words.Loading.LoadOptions _loadOptions = new Aspose.Words.Loading.LoadOptions();
    private string _outputFolder;
    private const string DefaultFontName = "Arial";

    public void SetOutputFolder(string outputFolder)
    {
        _outputFolder = outputFolder;
    }

    public AsposeWorker()
    {
        var fontSettings = new FontSettings();
        fontSettings.SubstitutionSettings.FontConfigSubstitution.Enabled = false;
        fontSettings.SubstitutionSettings.TableSubstitution.Enabled = false;
        fontSettings.SubstitutionSettings.FontInfoSubstitution.Enabled = false;
        fontSettings.SubstitutionSettings.DefaultFontSubstitution.Enabled = true;
        fontSettings.SubstitutionSettings.DefaultFontSubstitution.DefaultFontName = DefaultFontName;
        List<FontSourceBase> sourceBases = new List<FontSourceBase>()
        {
            // Use system fonts only
            new Aspose.Words.Fonts.SystemFontSource(),
        };
        fontSettings.SetFontsSources(sourceBases.ToArray());
        Aspose.Words.Loading.LoadOptions loadOptions = new Aspose.Words.Loading.LoadOptions()
        {
            FontSettings = fontSettings
        };
        _loadOptions = loadOptions;
    }

    public AsposeWorker(string fontFolder = "")
    {
        if (!Directory.Exists(fontFolder))
        {
            throw new DirectoryNotFoundException($"Font folder '{fontFolder}' not found.");
        }
        else
        {
            Console.WriteLine($"Using additional fonts from folder: {fontFolder}");
        }

        var fontSettings = new FontSettings();

        fontSettings.SubstitutionSettings.FontConfigSubstitution.Enabled = false;
        fontSettings.SubstitutionSettings.TableSubstitution.Enabled = false;
        fontSettings.SubstitutionSettings.FontInfoSubstitution.Enabled = false;
        fontSettings.SubstitutionSettings.DefaultFontSubstitution.Enabled = true;
        fontSettings.SubstitutionSettings.DefaultFontSubstitution.DefaultFontName = DefaultFontName;

        List<FontSourceBase> sourceBases = new List<FontSourceBase>()
        {
            // Use system fonts
            new Aspose.Words.Fonts.SystemFontSource(),
            // Add custom font folder
            new FolderFontSource(fontFolder, true),
        };
        fontSettings.SetFontsSources(sourceBases.ToArray());
        Aspose.Words.Loading.LoadOptions loadOptions = new Aspose.Words.Loading.LoadOptions()
        {
            FontSettings = fontSettings
        };
        _loadOptions = loadOptions;
    }

    public Task<string> ConvertToPdf(string inputFilePath, string prefix = "")
    {
        var logger = new FontSubstitutionLogger();
        var doc = new Document(inputFilePath, _loadOptions);
        doc.WarningCallback = logger;
        var pdfPath = System.IO.Path.ChangeExtension(inputFilePath, "_" + prefix + ".pdf");

        var savingOptions = new Aspose.Words.Saving.PdfSaveOptions()
        {
            SaveFormat = SaveFormat.Pdf,
            MemoryOptimization = true,
            JpegQuality = 80,
            DownsampleOptions =
            {
                Resolution = 150,
                ResolutionThreshold = 150,
            },
            // basic font embedding options to reduce file size
            FontEmbeddingMode = Aspose.Words.Saving.PdfFontEmbeddingMode.EmbedNone,
            EmbedFullFonts = false,
            UseCoreFonts = false
        };

        var outputFilePath = Path.Combine(_outputFolder, Path.GetFileName(pdfPath));
        doc.Save(outputFilePath, savingOptions);
        return Task.FromResult(outputFilePath);
    }
}