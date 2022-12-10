﻿using System.CodeDom.Compiler;

namespace MsbRpc.Generator.Extensions;

public static class TextWriterExtensions
{
    public readonly ref struct ParenthesesEnclosure
    {
        private readonly IndentedTextWriter _writer;

        public ParenthesesEnclosure(IndentedTextWriter writer)
        {
            _writer = writer;
            _writer.Write("(");
        }

        public void Dispose()
        {
            _writer.Write(")");
        }
    }

    public static ParenthesesEnclosure EncloseInParentheses(this IndentedTextWriter writer) => new(writer);

    public static void WriteFileHeader(this TextWriter writer, string fileScopedNamespace)
    {
        writer.WriteFileHeader();
        writer.WriteFileScopedNamespace(fileScopedNamespace);
    }

    public static void WriteLineSemicolon(this TextWriter writer) => writer.WriteLine(";");

    public static void WriteCommaDelimiter(this TextWriter writer) => writer.Write(", ");

    private static void WriteFileHeader(this TextWriter writer)
    {
        writer.WriteGeneratedFileComment();
        writer.WriteEnableNullable();
        writer.WriteLine();
    }

    private static void WriteFileScopedNamespace(this TextWriter writer, string namespaceName)
    {
        writer.WriteLine($"namespace {namespaceName};");
        writer.WriteLine();
    }

    private static void WriteGeneratedFileComment(this TextWriter writer)
    {
        writer.WriteLine("// <auto-generated/>");
    }

    private static void WriteEnableNullable(this TextWriter writer)
    {
        writer.WriteLine("#nullable enable");
    }
}