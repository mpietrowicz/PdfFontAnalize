namespace CommonDocxStateAnalyze;

/// <summary>
///  Analyzes fonts used in DOCX documents.
/// </summary>
public static class AnalyzeFonts
{
    public static HashSet<string> GetUsedFonts(string filePath)
    {
        using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return GetUsedFonts(fileStream);
    }
    
    public static HashSet<string> GetUsedFonts(FileStream docx)
    {
        var document = new Document(docx);
        var fonts = new HashSet<string>();
        foreach (var paragraph in document.FontInfos)
        {
            var fontName = paragraph.Name;
            if (!string.IsNullOrEmpty(fontName))
            {
                fonts.Add(fontName);
            }
        }

        return fonts;
    }
}