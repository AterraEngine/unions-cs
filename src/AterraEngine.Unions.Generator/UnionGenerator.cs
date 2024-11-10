// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;

namespace AterraEngine.Unions.Generator;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Generator(LanguageNames.CSharp)]
public class UnionGenerator : IIncrementalGenerator {
    public void Initialize(IncrementalGeneratorInitializationContext context) {
        // Detect types with the IUnion<> interface
        IncrementalValueProvider<ImmutableArray<UnionObject>> unionStructs = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: (node, _) => node is StructDeclarationSyntax { BaseList: not null },
                GatherUnionStructInfo)
            .Where(info => info is not null)
            .Select((info, _) => new UnionObject(
                info?.StructName!,
                info?.Namespace!,
                info?.TypesWithAliases!,
                (ImmutableArray<string>)info?.TypeParameters!)
            )
            .Collect();

        // Register the source output
        context.RegisterSourceOutput(context.CompilationProvider.Combine(unionStructs), GenerateSources);
    }

    private static UnionObject? GatherUnionStructInfo(GeneratorSyntaxContext context, CancellationToken cancellationToken) {
        if (context.Node is not StructDeclarationSyntax structDeclaration) return null;
        if (context.SemanticModel.GetDeclaredSymbol(structDeclaration) is not {} structSymbol) return null;

        // Check if the struct implements IUnion<>
        INamedTypeSymbol? iUnionInterface = structSymbol.Interfaces.FirstOrDefault(i => i.Name.Equals("IUnion") && i.IsGenericType);
        if (iUnionInterface is null) return null;

        // Extract the type arguments from IUnion<>
        ImmutableArray<ITypeSymbol> typeArguments = iUnionInterface.TypeArguments.ToImmutableArray();

        // Fetch aliases from the UnionAliases attribute
        AttributeData? aliasAttributeData = structSymbol.GetAttributes()
            .FirstOrDefault(attr => attr.AttributeClass?.Name == "UnionAliasesAttribute");

        Dictionary<ITypeSymbol, string?> typesWithAliases = ExtractTypesWithAliases(aliasAttributeData, typeArguments);

        // Collect generic type parameters if present
        ImmutableArray<ITypeParameterSymbol> genericTypeParameters = structSymbol.TypeParameters;

        return new UnionObject(
            structSymbol.Name,
            structSymbol.ContainingNamespace.ToDisplayString(),
            typesWithAliases,
            genericTypeParameters.Select(tp => tp.ToDisplayString()).ToImmutableArray()
        );
    }

    private static Dictionary<ITypeSymbol, string?> ExtractTypesWithAliases(AttributeData? aliasAttributeData, ImmutableArray<ITypeSymbol> typeArguments) {
        var aliases = new List<string?>(new string?[typeArguments.Length]);

        if (aliasAttributeData is { ConstructorArguments : { Length: > 0 } arguments }) {
            for (int i = 0; i < typeArguments.Length; i++) {
                aliases[i] = arguments[i].Value as string;
            }
        }

        return typeArguments.Zip(aliases, (type, alias) => (type, alias))
            .ToDictionary<(ITypeSymbol type, string? alias), ITypeSymbol, string?>(
                tuple => tuple.type,
                tuple => tuple.alias,
                SymbolEqualityComparer.Default
            );
    }

    private static void GenerateSources(SourceProductionContext context, (Compilation, ImmutableArray<UnionObject>) source) {
        ImmutableArray<UnionObject> classDeclarations = source.Item2;

        foreach (UnionObject unionInfo in classDeclarations) {
            context.AddSource($"{unionInfo.Namespace}.{unionInfo.StructName}_Union.g.cs", GenerateUnionCode(unionInfo));
        }
    }

    private static string GenerateUnionCode(UnionObject unionObject) {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("// <auto-generated />");

        var namespaces = new HashSet<string> { "System" };
        namespaces.UnionWith(unionObject.TypesWithAliases.Keys
            .Select(type => type.ContainingNamespace?.ToDisplayString())
            .Where(ns => !string.IsNullOrEmpty(ns) && ns != unionObject.Namespace)!) ;

        foreach (string? ns in namespaces) {
            stringBuilder.AppendLine($"using {ns};");
        }

        stringBuilder.AppendLine($"namespace {unionObject.Namespace};");
        stringBuilder.AppendLine($"public readonly partial struct {unionObject.GetStructClassName()} {{");
        stringBuilder.AppendLine("    public object Value { get; init; } = default!;");

        foreach (KeyValuePair<ITypeSymbol, string?> kvp in unionObject.TypesWithAliases) {
            ITypeSymbol? typeSymbol = kvp.Key;
            string alias = kvp.Value ?? GetAlias(kvp);
            string isAlias = $"Is{alias}";
            stringBuilder.AppendLine($"    public bool {isAlias} {{ get; init; }} = false;");
            stringBuilder.AppendLine($"    public {typeSymbol} As{alias} => ({typeSymbol})Value;");
            stringBuilder.AppendLine($"    public bool TryGetAs{alias}(out {typeSymbol} value) {{");
            stringBuilder.AppendLine($"        if ({isAlias}) {{");
            stringBuilder.AppendLine($"            value = As{alias};");
            stringBuilder.AppendLine("            return true;");
            stringBuilder.AppendLine("        }");
            stringBuilder.AppendLine("        value = default;");
            stringBuilder.AppendLine("        return false;");
            stringBuilder.AppendLine("    }");
            stringBuilder.AppendLine($"    public static implicit operator {unionObject.GetStructClassName()}({typeSymbol} value) => new {unionObject.GetStructClassName()}() {{");
            stringBuilder.AppendLine("        Value = value,");
            stringBuilder.AppendLine($"        {isAlias} = true");
            stringBuilder.AppendLine("    };");
        }

        stringBuilder.AppendLine("}");

        return stringBuilder.ToString();
    }

    private static string GetAlias(KeyValuePair<ITypeSymbol, string?> keyValuePair) {
        return keyValuePair.Value ?? GetTypeAlias(keyValuePair.Key);
    }

    private static string GetTypeAlias(ITypeSymbol type) {
        if (type is not INamedTypeSymbol namedType) return type.Name;

        if (namedType.IsTupleType) {
            return "TupleOf" + string.Join("And", namedType.TupleElements.Select(e => GetTypeAlias(e.Type)));
        }

        string name = namedType.Name;
        if (namedType.IsGenericType) {
            name += $"Of{string.Join("And", namedType.TypeArguments.Select(GetTypeAlias))}";
        }

        return name;
    }
}
