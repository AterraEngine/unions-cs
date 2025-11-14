# 🔗 CodeOfChaos.Unions 🔗

A Union Library for DotNet

## Overview

`CodeOfChaos.Unions` is a comprehensive library for creating and managing union types in .NET.
It leverages the latest features of C# 13.0, whilst built for NetStandard 2.0 to provide a robust and easily compatible framework for representing
multiple and diverse data types as a single unit.
The package was inspired by the OneOf package.

### Features

- **Type Safety**: Ensure type safety with union types that encapsulate various data forms.
- **Ease of Use**: Simplified API to integrate union types seamlessly into your project.
- **Performance Optimizations**: Designed with performance in mind to handle high-scale applications.
- **Generate**: Not satisfied with the basic unions we have made for you? No worries, you can generate your own using
  `CodeOfChaos.Unions.Generator`
- **Auto Alias**: Instead of having a pre-made `Union<T0,T1,...>` base type which simply provides a `IsT0` or `AsT1`
  api, this package generates all unions from the ground up.
    - This allows us to create insert any names we want. By default, it will choose the name of the types chosen for the
      aliases, example : `IsTrue` `AsString`.
    - You can also set your own alias for specific types using the attribute `[UnionAliases(aliasT2:"Something")]`. See
      usage in the example below.
- **Up to 16**: By default the `IUnion<>` interface allows up to 16 types within the union. Because we use casting
  instead of the index based approach by OneOf, there doesn't need to be a limit to this in the future.

### Getting Started

#### Installation

You can install `CodeOfChaos.Unions` via NuGet Package Manager:

```bash
dotnet add package CodeOfChaos.Unions
```

You can install `CodeOfChaos.Unions.Generator` via NuGet Package Manager:

```bash
dotnet add package CodeOfChaos.Unions.Generator
```

#### Usage

Here is a basic example to demonstrate how to create and use union types with `CodeOfChaos.Unions`.

```csharp
using CodeOfChaos.Unions;

TrueOrFalse trueOrFalse = new True();

if (trueOrFalse.IsTrue) {    
    // Do stuff here
}
```

```csharp
using CodeOfChaos.Unions;

ManyOneNoneOrError<int, string> union = new Many<int>([1, 2, 3]);
if (union.TryGetAsMany(out Many<T>? values) {
  // Do stuff here
}
if (union.TryGetAsOne(out One<T>? value) {
  // Do stuff here
}
if (union.TryGetAsNone(out None? value) {
  // Do stuff here
}
if (union.TryGetAsError(out Error<T>? value) {
  // Do stuff here
}
```

Using `.Value` will incur boxing. If you want to avoid boxing, it is currently advised to use the `TryGetAs{TypeName}`
method or a combination of `Is{TypeName}` and `As{Typename}` properties.

```csharp
using CodeOfChaos.Unions;

ManyOneNoneOrError<int, string> union = new One<int>(1);
switch (union.Value) {
    case Many<int>: //...
    case One<int>: //...
    case None: //...
    case Error: //...
}

if (union.TryGetAsNone(out None? value) {
    // ...        
}
```

Another version of using the switch case would be like the following example.
Although a little more cumbersome to write, it will do the same as the above example, without boxing.

```csharp
TrueOrFalse union = new False();
switch (union) {
    case {IsTrue: true, AsTrue: var trueValue}: 
        Assert.Equal(new True(), trueValue);
        break;
    case {IsFalse: true, AsFalse: var falseValue}: 
        Assert.Equal(new False(), falseValue);
        break;
}
```

You can directly use the predefined `Union<>` and `Union<T0, T1, ...>` structs provided by `CodeOfChaos.Unions` for
common use cases.
This struct has the downside of it's aliases being named `isT0`, `asT0`, etc and is this less easy to follow along what
is being referenced.

```csharp
using CodeOfChaos.Unions;

public class UnionExample {
    public Union<string, int> GetSomeValue(bool input) {
        if (input) return "Something";
        else return 0;
    }
}
```

Creating your own unions is easily done by installing `CodeOfChaos.Unions.Generators` and following the example:

```csharp
using CodeOfChaos.Unions;

[UnionAliases(aliasT2:"ErrorTuple")]
public readonly partial struct TrueFalseOrErrorTuple() : IUnion<True, False, (Error<string>, Type)>;
// Which will generate the following, instead of a default generated name for the 3rd type in the union.
// - IsErrorTuple
// - AsErrorTuple
// - TryGetErrorTuple(...)
```

Here is an advanced example demonstrating a custom union type with user-defined aliases:

```csharp
using CodeOfChaos.Unions;

[UnionAliases(aliasT2: "ErrorTuple")]
public readonly partial struct TrueFalseOrErrorTuple : IUnion<True, False, (Error<string>, Type)>;

class Program {
    static void Main() {
        TrueFalseOrErrorTuple union = new True();
        
        if (union.IsTrue) {
            Console.WriteLine("It's true!");
        }

        // Using the custom alias
        union = new ((new Error<string>("An error occurred"), typeof(int)));

        if (union.IsErrorTuple) {
            var errorTuple = union.AsErrorTuple;
            Console.WriteLine($"Error: {errorTuple.Item1.Message}, Type: {errorTuple.Item2}");
        }
    }
}
```

---

### Developer's Guide
When you download/fork the repo and open it in your editor, you'll most likely get hit with warnings and errors that `CodeOfChaos.Unions` is not setup correctly.
To resolve this, simply build `CodeOfChaos.Unions.Generators` and you should be all set.
This is due to the fact that a lot of the functionality of this discriminated unions library is not handwritten, but auto generated by incremental generators.

---

### Benchmarks

The following is a result of the benchmarks ran at [Benchmarks.CodeOfChaos.Unions](tests/Benchmarks.CodeOfChaos.Unions).
Benchmark results were last updated for version `6.2.0`

> BenchmarkDotNet v0.15.6, Windows 11 (10.0.26200.7019)
> AMD Ryzen 9 5950X 4.20GHz, 1 CPU, 32 logical and 16 physical cores
> .NET SDK 10.0.100
> [Host]     : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
> DefaultJob : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3


#### Default benchmarks:

| Method                                               |       Mean |     Error |    StdDev |     Median | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|------------------------------------------------------|-----------:|----------:|----------:|-----------:|------:|--------:|-------:|----------:|------------:|
| CodeOfChaosUnions_SuccessOrFailure_SwitchCase_Struct |  0.0016 ns | 0.0028 ns | 0.0082 ns |  0.0000 ns | 0.000 |    0.00 |      - |         - |          NA |
| CodeOfChaosUnions_UnionT8_TryGetAs                   |  0.0410 ns | 0.0320 ns | 0.0695 ns |  0.0000 ns | 0.007 |    0.01 |      - |         - |          NA |
| CodeOfChaosUnions_UnionT8_SwitchCase_Value           |  0.0982 ns | 0.0513 ns | 0.1070 ns |  0.0676 ns | 0.017 |    0.02 |      - |         - |          NA |
| CodeOfChaosUnions_SuccessOrFailure_SwitchCase_Value  |  4.1695 ns | 0.1144 ns | 0.3355 ns |  4.1213 ns | 0.720 |    0.08 | 0.0014 |      24 B |          NA |
| CodeOfChaosUnions_TrueFalse_TryGetAsTrue             |  5.8224 ns | 0.1478 ns | 0.4193 ns |  5.7490 ns | 1.005 |    0.10 |      - |         - |          NA |
| OneOf_SuccessOrFailure_SwitchCase_Value              |  7.3993 ns | 0.1810 ns | 0.5336 ns |  7.3967 ns | 1.277 |    0.13 | 0.0014 |      24 B |          NA |
| OneOfTrueFalse_TryGetAsTrue                          | 13.6592 ns | 0.4600 ns | 1.3419 ns | 13.3749 ns | 2.358 |    0.28 | 0.0038 |      64 B |          NA |
| OneOf_OneOfT8_SwitchCase_Value                       | 15.4853 ns | 0.3707 ns | 0.9093 ns | 15.3650 ns | 2.673 |    0.24 | 0.0038 |      64 B |          NA |
| OneOf_OneOfT8_TryGetAs                               | 18.1008 ns | 0.4226 ns | 1.2262 ns | 17.7133 ns | 3.124 |    0.30 | 0.0038 |      64 B |          NA |
| Dunet_TrueFalse_MatchTrue                            | 31.2708 ns | 0.7507 ns | 2.1659 ns | 31.1582 ns | 5.397 |    0.53 | 0.0105 |     176 B |          NA |

#### Enhanced benchmarks
More operations per invoke to return some more useful data.

| Method                                                 |      Mean |     Error |    StdDev |    Median |   Gen0 | Allocated |
|--------------------------------------------------------|----------:|----------:|----------:|----------:|-------:|----------:|
| CodeOfChaosUnions_UnionT8_SwitchCase_Value_Enhanced    |  6.817 ns | 0.2039 ns | 0.5915 ns |  6.653 ns | 0.0000 |         - |
| CodeOfChaosUnions_UnionT8_TryGetAs_Enhanced            | 11.772 ns | 0.2274 ns | 0.2793 ns | 11.756 ns |      - |         - |
| CodeOfChaosUnions_RefUnionT8_SwitchCase_Value_Enhanced | 20.998 ns | 0.5742 ns | 1.6930 ns | 20.598 ns | 0.0043 |      72 B |
| CodeOfChaosUnions_RefUnionT8_TryGetAs_Enhanced         | 21.454 ns | 0.8363 ns | 2.4527 ns | 20.756 ns | 0.0043 |      72 B |
| OneOf_OneOfT8_SwitchCase_Value_Enhanced                | 21.587 ns | 0.4261 ns | 0.9263 ns | 21.608 ns | 0.0038 |      64 B |
| OneOf_OneOfT8_TryGetAs_Enhanced                        | 33.099 ns | 1.2486 ns | 3.6815 ns | 33.476 ns | 0.0038 |      64 B |

#### Recursive Benchmarks
Used to view the impact of ref Unions versus value Unions.


