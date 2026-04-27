// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace CodeOfChaos.Unions.Generators;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents a union object structure used in the generation of union types.
/// </summary>
public record UnionObject(
    string StructName,
    string NameSpace,
    Dictionary<ITypeSymbol, string?> TypesWithAliases, 
    ImmutableArray<string> TypeParameters,
    bool IsRecord,
    bool IsStruct,
    int ExtraGeneratorFlags
) {

    /// <summary>
    ///     Retrieves the structured class name of the union object, including its type parameters if applicable.
    /// </summary>
    public string GetStructClassName() => TypeParameters.Length > 0
        ? $"{StructName}<{string.Join(", ", TypeParameters)}>"
        : StructName;

    public bool HasFlagGenerateFrom() => (ExtraGeneratorFlags & 0b1) != 0;
    public bool HasFlagGenerateAsValue() => (ExtraGeneratorFlags & 0b10) != 0;

    public static bool IsValidGenerateAsValue(ITypeSymbol typeSymbol, out bool isValues, out string valueTypeName, out string notNullWhen, out string nullable, out string validIfTrue) {
        // Check if the typeSymbol inherits from IValue<T> or IValues<T>
        isValues = false;
        valueTypeName = string.Empty;
        notNullWhen = string.Empty;
        nullable = string.Empty;
        validIfTrue = string.Empty;

        if (typeSymbol.AllInterfaces.IsEmpty) return false;

        foreach (INamedTypeSymbol? @interface in typeSymbol.AllInterfaces) {
            string name = @interface.ConstructedFrom.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            switch (name) {
                case "global::CodeOfChaos.Unions.IValue<T>": {
                    isValues = false;
                    break;
                }

                case "global::CodeOfChaos.Unions.IValues<T>": {
                    isValues = true;
                    break;
                }

                default: continue;
            }

            valueTypeName = @interface.TypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

            bool isReferenceType = @interface.TypeArguments[0].IsReferenceType;
            bool isValueType = @interface.TypeArguments[0].IsValueType;

            notNullWhen = isReferenceType || !isValueType ? "[NotNullWhen(true)] " : string.Empty;
            nullable = isReferenceType || !isValueType ? "?" : string.Empty;
            validIfTrue = isReferenceType || !isValueType ? $"value{(isValues ? "s" : string.Empty)} is not null" : "true";
            
            return true;
        }

        return false;
    }
}
