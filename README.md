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
using AterraEngine.Unions;

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

| Method                                                |       Mean |     Error |    StdDev |     Median | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|-------------------------------------------------------|-----------:|----------:|----------:|-----------:|------:|--------:|-------:|----------:|------------:|
| AterraEngineUnions_UnionT8_TryGetAs                   |  0.0058 ns | 0.0096 ns | 0.0090 ns |  0.0000 ns | 0.001 |    0.00 |      - |         - |          NA |
| AterraEngineUnions_UnionT8_SwitchCase_Value           |  0.0398 ns | 0.0335 ns | 0.0372 ns |  0.0276 ns | 0.007 |    0.01 |      - |         - |          NA |
| AterraEngineUnions_SuccessOrFailure_SwitchCase_Struct |  0.2689 ns | 0.0295 ns | 0.0276 ns |  0.2725 ns | 0.047 |    0.00 |      - |         - |          NA |
| AterraEngineUnions_SuccessOrFailure_SwitchCase_Value  |  3.8638 ns | 0.1025 ns | 0.0959 ns |  3.8513 ns | 0.670 |    0.02 | 0.0014 |      24 B |          NA |
| OneOf_SuccessOrFailure_SwitchCase_Value               |  5.7265 ns | 0.1393 ns | 0.3173 ns |  5.6165 ns | 0.993 |    0.06 | 0.0014 |      24 B |          NA |
| AterraEngineUnions_TrueFalse_TryGetAsTrue             |  5.7657 ns | 0.1016 ns | 0.0950 ns |  5.7967 ns | 1.000 |    0.02 |      - |         - |          NA |
| OneOfTrueFalse_TryGetAsTrue                           |  7.4966 ns | 0.1982 ns | 0.2507 ns |  7.4139 ns | 1.301 |    0.05 | 0.0038 |      64 B |          NA |
| OneOf_OneOfT8_SwitchCase_Value                        |  7.9805 ns | 0.2031 ns | 0.2173 ns |  7.9621 ns | 1.384 |    0.04 | 0.0038 |      64 B |          NA |
| OneOf_OneOfT8_TryGetAs                                | 11.9939 ns | 0.1411 ns | 0.2024 ns | 11.9915 ns | 2.081 |    0.05 | 0.0038 |      64 B |          NA |
| Dunet_TrueFalse_MatchTrue                             | 20.8607 ns | 0.2432 ns | 0.2031 ns | 20.8425 ns | 3.619 |    0.07 | 0.0105 |     176 B |          NA |

#### Enhanced benchmarks

| Method                                                  |      Mean |     Error |    StdDev |   Gen0 | Allocated |
|---------------------------------------------------------|----------:|----------:|----------:|-------:|----------:|
| AterraEngineUnions_UnionT8_SwitchCase_Value_Enhanced    |  8.161 ns | 0.1571 ns | 0.1312 ns |      - |         - |
| AterraEngineUnions_UnionT8_TryGetAs_Enhanced            | 15.817 ns | 0.2973 ns | 0.2781 ns |      - |         - |
| AterraEngineUnions_RefUnionT8_SwitchCase_Value_Enhanced | 17.382 ns | 0.3451 ns | 0.3389 ns | 0.0043 |      72 B |
| OneOf_OneOfT8_SwitchCase_Value_Enhanced                 | 18.630 ns | 0.3602 ns | 0.3699 ns | 0.0038 |      64 B |
| AterraEngineUnions_RefUnionT8_TryGetAs_Enhanced         | 20.968 ns | 0.1666 ns | 0.1559 ns | 0.0043 |      72 B |
| OneOf_OneOfT8_TryGetAs_Enhanced                         | 30.125 ns | 0.5904 ns | 0.6318 ns | 0.0038 |      64 B |

