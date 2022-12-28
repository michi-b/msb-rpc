using Microsoft.CodeAnalysis.Text;

namespace MsbRpc.Generator.CodeWriters.Files;

internal struct WriterResult
{
    public readonly SourceText SourceText;
    public readonly string FileName;

    public WriterResult(string fileName, SourceText sourceText)
    {
        FileName = fileName;
        SourceText = sourceText;
    }
}