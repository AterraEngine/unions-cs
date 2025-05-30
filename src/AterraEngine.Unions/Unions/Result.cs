// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("State", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial struct Result() : IUnion<bool, Error<string>> {
    public bool State => AsState;
    public bool TryGetState([NotNullWhen(true)] out bool? state) => TryGetAsState(out state);

    public static implicit operator bool(Result value) => value.AsState;

    public static Result FromError(string failure) => FromError(new Error<string>(failure));
}

[UnionAliases("Success", "Error")]
[UnionExtra(UnionExtra.GenerateFrom | UnionExtra.GenerateAsValue)]
public readonly partial struct Result<T>() : IUnion<T, Error<string>> {
    public static implicit operator bool(Result<T> value) => value.IsSuccess;
    public static Result<T> FromError(string failure) => FromError(new Error<string>(failure));
}
