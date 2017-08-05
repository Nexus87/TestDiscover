using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace TestDiscover.Lib
{
    public class ClassVirtualizationVisitor : SymbolVisitor
    {
        private const string FactMetadataName = "Fact";
        private const string TheoryMetaDataName = "Theory";

        private static readonly string[] TestAttributes = {FactMetadataName, TheoryMetaDataName};
        public ConcurrentQueue<IMethodSymbol> Methods { get; set; } = new ConcurrentQueue<IMethodSymbol>();
        

        public override void VisitMethod(IMethodSymbol symbol)
        {
            if (symbol.GetAttributes().Any(x => TestAttributes.Contains(x.AttributeClass.MetadataName)))
            {
                Methods.Enqueue(symbol);
            }
        }

        public override void VisitNamedType(INamedTypeSymbol symbol)
        {
            var attr = symbol.GetAttributes();
            Parallel.ForEach(symbol.GetMembers(), s => s.Accept(this));
        }

        public override void VisitNamespace(INamespaceSymbol symbol)
        {
            Parallel.ForEach(symbol.GetMembers(), s => s.Accept(this));
        }
    }
}