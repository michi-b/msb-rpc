using System;
using JetBrains.Annotations;

namespace MsbRpc.Generator.Attributes;

/// <summary>
///     indicates that an attribute class is used as a marker for a generator
/// </summary>
[MeansImplicitUse(ImplicitUseKindFlags.Access)]
[AttributeUsage(AttributeTargets.Class)]
internal class GeneratorMarkerAttribute : Attribute { }