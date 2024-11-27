// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace AterraEngine.Unions.Generators;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct UnionObject(string structName, string nameSpace, Dictionary<ITypeSymbol, string?> typesWithAliases, ImmutableArray<string> typeParameters, bool isRecordStruct) {
    public string StructName { get; } = structName;
    public string Namespace { get; } = nameSpace;
    public Dictionary<ITypeSymbol, string?> TypesWithAliases { get; } = typesWithAliases;
    public ImmutableArray<string> TypeParameters { get; } = typeParameters;
    public bool IsRecordStruct { get; } = isRecordStruct;

    public string GetStructClassName() => TypeParameters.Length > 0
        ? $"{StructName}<{string.Join(", ", TypeParameters)}>"
        : StructName;
        

}