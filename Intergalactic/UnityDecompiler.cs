﻿#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System.IO;
using System.Threading.Tasks;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.ProjectDecompiler;
using ICSharpCode.Decompiler.Metadata;
using ModdingToolkit.Magicka.Decompiling;

namespace Intergalactic
{
    public class UnityDecompiler : IDecompiler
    {
        public Task DecompileFile(string from, string to)
        {
            if (!Directory.Exists(to))
                Directory.CreateDirectory(to);

            using PEFile module = new(from);
            UniversalAssemblyResolver resolver = new(from, false, module.Reader.DetectTargetFrameworkId());

            WholeProjectDecompiler decompiler = new(GetSettings(module), resolver, resolver, null);
            decompiler.DecompileProject(module, to);

            return Task.CompletedTask;
        }

        internal static DecompilerSettings GetSettings(PEFile module)
        {
            return new(LanguageVersion.CSharp7_3)
            {
                RemoveDeadCode = true,
                RemoveDeadStores = true,

                Ranges = false,

                ThrowOnAssemblyResolveErrors = false,
                UseSdkStyleProjectFormat = WholeProjectDecompiler.CanUseSdkStyleProjectFormat(module),
            };
        }
    }
}