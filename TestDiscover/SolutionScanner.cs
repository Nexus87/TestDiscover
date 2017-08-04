using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace TestDiscover
{
    class SolutionScanner
    {
        private readonly Solution _solution;

        private const string FactMetadataName = "FactAttribute";
        private const string TheoryMetaDataName = "TheoryAttribute";
        public SolutionScanner(string solutionPath)
        {
            var msWorkspace = MSBuildWorkspace.Create();
            _solution = msWorkspace.OpenSolutionAsync(solutionPath).Result;
        }


        public List<string> Scan()
        {
            var classVisitor = new ClassVirtualizationVisitor();

            foreach (var project in _solution.Projects)
            {
                var compilation = project.GetCompilationAsync().Result;
                classVisitor.Visit(compilation.Assembly.GlobalNamespace);

            }

            return classVisitor.Classes.SelectMany(x => x.GetMembers())
                .Where(x => x.Kind == SymbolKind.Method
                            && HasFactOrTheoryAttribute(x))
                .Select(x => x.ToString())
                .ToList();

        }

        private static bool HasFactOrTheoryAttribute(ISymbol symbol)
        {
            return symbol.GetAttributes().Any(y =>
                y.AttributeClass.MetadataName == FactMetadataName ||
                y.AttributeClass.MetadataName == TheoryMetaDataName);
        }
    }
}