using System;
using System.Collections.Generic;
using System.IO;

namespace TestDiscover.Tests
{
    public static class Constants
    {
        public const string Namespace = "TestProject.";
        public const string Solution = "TestProject.sln";
        public static readonly string RepoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\TestProject");
        public static readonly string SolutionPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\TestProject\TestProject.sln");
    }

    internal class Commit
    {
        public static readonly IReadOnlyList<Commit> All = new List<Commit>
        {
            new Commit
            {
                Sha1Hash = "ea146c0d7dff2d46d2b87fbbb28f14b4419b901a",
                Tag = "initial",
                Functions = new List<string>
                {
                    Constants.Namespace + "Class1.Test_Function()",
                    Constants.Namespace + "Class2.Class3.Test_Func2()"
                }
            },
            new Commit
            {
                Sha1Hash = "0533e77fc72c686c16becb419c3d25f5aa68b8e3",
                Tag = "second",
                Functions = new List<string>
                {
                    Constants.Namespace + "Class1.Test_Function()",
                    Constants.Namespace + "Class1.Test_Function2(int, string)",
                    Constants.Namespace + "Class2.Class3.Test_Func2()",
                    Constants.Namespace + "Class2.Class3.Test_Func3()"
                }
            },
            new Commit
            {
                Sha1Hash = "6120d65b8b3aca596483f1fb8ac4d6fda2b59875",
                Tag = "third",
                Functions = new List<string>
                {
                    Constants.Namespace + "Class1.Test_Function()",
                    Constants.Namespace + "Class1.Test_Function2(int, string)",
                    Constants.Namespace + "Class2.Class3.Test_Func2()",
                    Constants.Namespace + "Class2.Class3.Test_Func3()",
                    Constants.Namespace + "Class4.Test_Func5()"
                }
            }
        };

        public string Tag { get; private set; }
        public string Sha1Hash { get; private set; }
        public IReadOnlyList<string> Functions { get; private set; }
    }
}