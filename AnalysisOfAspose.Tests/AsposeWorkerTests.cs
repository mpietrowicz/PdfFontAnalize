using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace AnalysisOfAspose.Tests;

public class AsposeWorkerTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task ConvertToPdf_Success()
    {
        var _uut = new AsposeWorker();
        _uut.SetOutputFolder(Directory.GetCurrentDirectory());
        var data = await _uut.ConvertToPdf("DocxFiles/TestCzcionek.docx", "ConvertToPdf_Success");
        ClassicAssert.IsTrue(File.Exists(data));
    }
}