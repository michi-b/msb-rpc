using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.Text;
using MsbRpc.Generator.CodeWriters.Utility;
using MsbRpc.Generator.GenerationTree;

namespace MsbRpc.Generator.CodeWriters.Files;

internal abstract class CodeFileWriter
{
    private readonly string _generatedNamespace;

    protected abstract string FileName { get; }

    protected virtual string[] UsedNamespaces => Array.Empty<string>();

    protected CodeFileWriter(ContractNode contract) : this(contract.Namespace) { }

    private CodeFileWriter(string generatedNamespace) => _generatedNamespace = generatedNamespace;

    public Result Generate()
    {
        IndentedTextWriter writer = CreateCodeWriter();
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

    protected abstract void Write(IndentedTextWriter writer);

    private IndentedTextWriter CreateCodeWriter()
    {
        IndentedTextWriter writer = new(new StringWriter());
        
        writer.WriteFileHeader(_generatedNamespace, UsedNamespaces);

        return writer;
    }
}