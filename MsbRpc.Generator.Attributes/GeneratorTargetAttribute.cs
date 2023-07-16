#region

using System;
using JetBrains.Annotations;

#endregion

namespace MsbRpc.Generator.Attributes;

/// <summary>
///     indicates that a property or field is necessary information for code generation
/// </summary>
[MeansImplicitUse(ImplicitUseKindFlags.Access)]
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
internal class GeneratorTargetAttribute : Attribute { }