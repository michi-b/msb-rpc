﻿using System;
using JetBrains.Annotations;

namespace MsbRpc.Attributes;

[MeansImplicitUse(ImplicitUseKindFlags.Access)]
internal class MayBeUsedByGeneratorAttribute : Attribute { }