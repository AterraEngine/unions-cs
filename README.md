# 🔗 AterraEngine.Unions 🔗

A Union Library for DotNet

## Overview

`AterraEngine.Unions` is a comprehensive library for creating and managing union types in .NET.
It leverages the latest features of C# 13.0 and .NET 9.0 to provide a robust and efficient framework for representing
multiple and diverse data types as a single unit.
The package was inspired by the OneOf package.

### Features

- **Type Safety**: Ensure type safety with union types that encapsulate various data forms.
- **Ease of Use**: Simplified API to integrate union types seamlessly into your project.
- **Performance Optimizations**: Designed with performance in mind to handle high-scale applications.
- **Generate**: Not satisfied with the basic unions we have made for you? No worries, you can generate your own using
  `AterraEngine.Unions.Generator`
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

You can install `AterraEngine.Unions` via NuGet Package Manager:

```bash
dotnet add package AterraEngine.Unions
```

You can install `AterraEngine.Unions.Generator` via NuGet Package Manager:

```bash
dotnet add package AterraEngine.Unions.Generator
```

#### Usage

Here is a basic example to demonstrate how to create and use union types with `AterraEngine.Unions`.

```csharp
using AterraEngine.Unions;

TrueOrFalse trueOrFalse = new True();

if (trueOrFalse.IsTrue) {    
    // Do stuff here
}
```

```csharp
using AterraEngine.Unions;

ManyOneNoneOrError<int, string> union = new Many<int>([1, 2, 3]);
if (union.TryGetAsMany(out Many<T> values) {
  // Do stuff here
}
if (union.TryGetAsOne(out One<T> value) {
  // Do stuff here
}
if (union.TryGetAsNone(out None value) {
  // Do stuff here
}
if (union.TryGetAsError(out Error<T> value) {
  // Do stuff here
}
```

Using `.Value` will incur boxing. If you want to avoid boxing, it is currently advised to use the `TryGetAs{TypeName}`
method or a combination of `Is{TypeName}` and `As{Typename}` properties.

```csharp
using AterraEngine.Unions;

ManyOneNoneOrError<int, string> union = new One<int>(1);
switch (union.Value) {
    case Many<int>: //...
    case One<int>: //...
    case None: //...
    case Error: //...
}

if (union.TryGetAsNone(out None value) {
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

You can directly use the predefined `Union<>` and `Union<T0, T1, ...>` structs provided by `AterraEngine.Unions` for
common use cases.
This struct has the downside of it's aliases being named `isT0`, `asT0`, etc and is this less easy to follow along what
is being referenced.

```csharp
using AterraEngine.Unions;

public class UnionExample {
    public Union<string, int> GetSomeValue(bool input) {
        if (input) return "Something";
        else return 0;
    }
}
```

Creating your own unions is easily done by installing `AterraEngine.Unions.Generators` and following the example:

```csharp
using AterraEngine.Unions;

[UnionAliases(aliasT2:"ErrorTuple")]
public readonly partial struct TrueFalseOrErrorTuple() : IUnion<True, False, (Error<string>, Type)>;
// Which will generate the following, instead of a default generated name for the 3rd type in the union.
// - IsErrorTuple
// - AsErrorTuple
// - TryGetErrorTuple(...)
```

Here is an advanced example demonstrating a custom union type with user-defined aliases:

```csharp
using AterraEngine.Unions;

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
When you download/fork the repo and open it in your editor, you'll most likely get hit with warnings and errors that `AterraEngine.Unions` is not setup correctly.
To resolve this, simply build `AterraEngine.Unions.Generators` and you should be all set.
This is due to the fact that a lot of the functionality of this discriminated unions library is not handwritten, but auto generated by incremental generators.

---

### Benchmarks

The following is a result of the benchmarks found
at [Benchmarks.AterraEngine.Unions](tests/Benchmarks.AterraEngine.Unions).
Benchmark results were last updated for version `3.10.0`

> BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4541/23H2/2023Update/SunValley3)
>
> AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
>
> .NET SDK 9.0.100
>
> [Host]     : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX2
>
> DefaultJob : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX2

#### Normal benchmarks:

| Method                                                |       Mean |     Error |    StdDev | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|-------------------------------------------------------|-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| AterraEngineUnions_UnionT8_TryGetAs                   |  0.0000 ns | 0.0000 ns | 0.0000 ns | 0.000 |    0.00 |      - |         - |          NA |
| AterraEngineUnions_UnionT8_SwitchCase_Value           |  0.0539 ns | 0.0368 ns | 0.0344 ns | 0.010 |    0.01 |      - |         - |          NA |
| AterraEngineUnions_SuccessOrFailure_SwitchCase_Struct |  0.1732 ns | 0.0101 ns | 0.0094 ns | 0.032 |    0.00 |      - |         - |          NA |
| AterraEngineUnions_SuccessOrFailure_SwitchCase_Value  |  4.7191 ns | 0.0976 ns | 0.0913 ns | 0.862 |    0.02 | 0.0014 |      24 B |          NA |
| AterraEngineUnions_TrueFalse_TryGetAsTrue             |  5.4756 ns | 0.0474 ns | 0.0444 ns | 1.000 |    0.01 |      - |         - |          NA |
| OneOf_SuccessOrFailure_SwitchCase_Value               |  7.2725 ns | 0.1632 ns | 0.2004 ns | 1.328 |    0.04 | 0.0014 |      24 B |          NA |
| OneOfTrueFalse_TryGetAsTrue                           | 11.2695 ns | 0.2091 ns | 0.1956 ns | 2.058 |    0.04 | 0.0038 |      64 B |          NA |
| OneOf_OneOfT8_SwitchCase_Value                        | 11.9177 ns | 0.2238 ns | 0.1984 ns | 2.177 |    0.04 | 0.0038 |      64 B |          NA |
| OneOf_OneOfT8_TryGetAs                                | 15.4805 ns | 0.3515 ns | 0.3288 ns | 2.827 |    0.06 | 0.0038 |      64 B |          NA |
| Dunet_TrueFalse_MatchTrue                             | 30.9437 ns | 0.6507 ns | 0.9121 ns | 5.652 |    0.17 | 0.0105 |     176 B |          NA |

#### Enhanced benchmarks

| Method                                               |      Mean |     Error |    StdDev |   Gen0 | Allocated |
|------------------------------------------------------|----------:|----------:|----------:|-------:|----------:|
| AterraEngineUnions_UnionT8_SwitchCase_Value_Enhanced |  8.179 ns | 0.1608 ns | 0.2550 ns |      - |         - |
| AterraEngineUnions_UnionT8_TryGetAs_Enhanced         | 15.285 ns | 0.2901 ns | 0.4252 ns |      - |         - |
| OneOf_OneOfT8_SwitchCase_Value_Enhanced              | 18.157 ns | 0.1611 ns | 0.1258 ns | 0.0038 |      64 B |
| OneOf_OneOfT8_TryGetAs_Enhanced                      | 28.218 ns | 0.3147 ns | 0.2943 ns | 0.0038 |      64 B |

