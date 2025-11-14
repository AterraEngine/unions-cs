// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
public class UnionExtraAttribute(UnionExtra extra) : Attribute {
    public UnionExtra Extra { get; } = extra;
}

[Flags]
public enum UnionExtra {
    None = 0b0,
    GenerateFrom = 0b1,
    GenerateAsValue = 0b10
}
