using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace TestDiscover.Lib
{
    public class SolutionScanner
    {
        private readonly Solution _solution;

        public SolutionScanner(string solutionPath)
        {
            var msWorkspace = MSBuildWorkspace.Create();
            _solution = msWorkspace.OpenSolutionAsync(solutionPath).Result;
        }


        public List<string> Scan()
        {
            var classVisitor = new ClassVirtualizationVisitor();

            Parallel.ForEach(_solution.Projects,
                x => classVisitor.Visit(x.GetCompilationAsync().Result.Assembly.GlobalNamespace));

            return classVisitor.Methods
                .Select(x => x.ToString())
                .ToList();
        }
    }
}