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
    }

    internal class Commit
    {
        public static readonly IReadOnlyList<Commit> All = new List<Commit>
        {
            new Commit
            {
                Sha1Hash = "7c75579b683a9f5e96b69edb46c86b378f7cd134",
                Tag = "initial",
                Functions = new List<string>
                {
                    Constants.Namespace + "Class1.Test_Function()",
                    Constants.Namespace + "Class2.Class3.Test_Func2()"
                }
            },
            new Commit
            {
                Sha1Hash = "400813d461ece9f108529ed8577560a54c34d841",
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
                Sha1Hash = "751bfa5c1a389a967ee438f74e27edc62f990032",
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