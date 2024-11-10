// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace AterraEngine.Unions.Generator.Sample;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public partial struct GenericUnion<T> : IUnion<Success<T>, None, Error<string>>;

[UnionAliases(aliasT0: "SuccessWithValue")]
public partial struct GenericUnionWithAlias<T> : IUnion<Success<T>, None, Error<string>>;