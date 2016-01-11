using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Analyzer;
using Analyzer.ReturnTypes;
using Mono.Cecil;

[ContractClass(typeof(ITypeContract))]
public interface IType
{
    TypeDefinition TypeDefinition { get; }
    IAssembly Assembly { get; }
    string FullName { get; }
    TypeReferenceCount GetReferencedTypes();
    TypeReferenceCount GetBaseTypes();
    TypeReferenceCount GetTypesReferencedByMethods();
    IEnumerable<IAssembly> GetUsedAssemblies();
}

[ContractClassFor(typeof(IType))]
abstract class ITypeContract : IType
{
    public TypeDefinition TypeDefinition { get; private set; }
    public IAssembly Assembly { get; private set; }
    public string FullName { get; private set; }
    public TypeReferenceCount GetReferencedTypes()
    {
        Contract.Ensures(Contract.Result<TypeReferenceCount>() != null);

        return default(TypeReferenceCount);
    }

    public TypeReferenceCount GetBaseTypes()
    {
        Contract.Ensures(Contract.Result<TypeReferenceCount>() != null);

        return default(TypeReferenceCount);
    }

    public TypeReferenceCount GetTypesReferencedByMethods()
    {
        Contract.Ensures(Contract.Result<TypeReferenceCount>() != null);

        return default(TypeReferenceCount);
    }

    public IEnumerable<IAssembly> GetUsedAssemblies()
    {
        Contract.Ensures(Contract.Result<IEnumerable<IAssembly>>() != null);
        Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<IAssembly>>(), t => t != null));

        return default(IEnumerable<IAssembly>);
    }
}