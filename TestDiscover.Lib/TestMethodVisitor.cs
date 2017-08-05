using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace TestDiscover.Lib
{
    public class TestMethodVisitor : SymbolVisitor
    {
        private const string FactMetadataName = "Fact";
        private const string TheoryMetaDataName = "Theory";

        private static readonly string[] TestAttributes = {FactMetadataName, TheoryMetaDataName};
        public ConcurrentQueue<IMethodSymbol> Methods { get; } = new ConcurrentQueue<IMethodSymbol>();
        

        public override void VisitMethod(IMethodSymbol symbol)
        {
            if (symbol.GetAttributes().Any(x => TestAttributes.Contains(x.AttributeClass.MetadataName)))
            {
                Methods.Enqueue(symbol);
            }
        }

        public override void VisitNamedType(INamedTypeSymbol symbol)
        {
            Parallel.ForEach(symbol.GetMembers(), s => s.Accept(this));
        }

        public override void VisitNamespace(INamespaceSymbol symbol)
        {
            Parallel.ForEach(symbol.GetMembers(), s => s.Accept(this));
        }
    }
}