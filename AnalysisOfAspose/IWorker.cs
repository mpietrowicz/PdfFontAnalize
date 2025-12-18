namespace AnalysisOfAspose;

public interface IWorker
{
    public void SetOutputFolder(string outputFolder);
    public Task<string> ConvertToPdf(string inputFilePath, string prefix);
}
