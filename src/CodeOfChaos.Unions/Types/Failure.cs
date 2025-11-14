// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents a failure state in a computation or operation, encapsulating a value of a specific type.
/// </summary>
public readonly struct Failure {
    public static readonly Failure Empty = new();
}

/// <summary>
///     Represents a failure, typically used within union types or result-based patterns.
/// </summary>
/// <typeparam name="T">The type of the value that describes the failure.</typeparam>
public readonly record struct Failure<T>(T Value) : IValue<T> {
    public static readonly Failure<T> Empty = new();
}
