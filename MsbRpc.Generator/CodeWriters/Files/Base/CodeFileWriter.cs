using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis.Text;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;

namespace MsbRpc.Generator.CodeWriters.Files.Base;

internal abstract class CodeFileWriter
{
    protected readonly ContractNode Contract;

    protected abstract string FileName { get; }

    protected virtual string[] UsedNamespaces => Array.Empty<string>();

    protected CodeFileWriter(ContractNode contract) => Contract = contract;

    public Result Generate()
    {
        IndentedTextWriter writer = CreateWriter();

        WriteFileHeader(writer);

        Write(writer);

        return new Result(FileName, writer.GetResult());
    }

    public readonly struct Result
    {
        public readonly string FileName;
        public readonly SourceText Code;

        public Result(string fileName, string code)
        {
            FileName = fileName;
            Code = SourceText.From(code, Encoding.UTF8);
        }
    }

    private void WriteFileHeader(TextWriter writer)
    {
        writer.WriteFileHeader(Contract.Namespace, UsedNamespaces);
    }

    protected abstract void Write(IndentedTextWriter writer);

    private static IndentedTextWriter CreateWriter()
    {
        IndentedTextWriter writer = new(new StringWriter());
        return writer;
    }
}