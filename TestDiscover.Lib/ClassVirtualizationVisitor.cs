using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace TestDiscover.Lib
{
    public class ClassVirtualizationVisitor : SymbolVisitor
    {

        public List<INamedTypeSymbol> Classes { get; set; } = new List<INamedTypeSymbol>();
        public override void VisitNamedType(INamedTypeSymbol symbol)
        {
            if(symbol.TypeKind != TypeKind.Interface)
                Classes.Add(symbol);
            Parallel.ForEach(symbol.GetMembers(), s => s.Accept(this));
        }

        public override void VisitNamespace(INamespaceSymbol symbol)
        {
            Parallel.ForEach(symbol.GetMembers(), s => s.Accept(this));
        }
    }
}