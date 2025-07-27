# 🔗 AterraEngine.Unions 🔗

A Union Library for DotNet

## Overview

`AterraEngine.Unions` is a comprehensive library for creating and managing union types in .NET.
It leverages the latest features of C# 13.0, whilst built for NetStandard 2.0 to provide a robust and easily compatible framework for representing
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

The following is a result of the benchmarks ran at [Benchmarks.AterraEngine.Unions](tests/Benchmarks.AterraEngine.Unions).
Benchmark results were last updated for version `6.0.0`

> BenchmarkDotNet v0.15.2, Windows 11 (10.0.26100.4652/24H2/2024Update/HudsonValley)
> Unknown processor
> .NET SDK 9.0.300
> [Host]     : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
> DefaultJob : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


#### Default benchmarks:

| Method                                                |       Mean |     Error |    StdDev |     Median | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|-------------------------------------------------------|-----------:|----------:|----------:|-----------:|------:|--------:|-------:|----------:|------------:|
| AterraEngineUnions_UnionT8_SwitchCase_Value           |  0.0007 ns | 0.0023 ns | 0.0022 ns |  0.0000 ns | 0.000 |    0.00 |      - |         - |          NA |
| AterraEngineUnions_UnionT8_TryGetAs                   |  0.0114 ns | 0.0170 ns | 0.0244 ns |  0.0000 ns | 0.003 |    0.01 |      - |         - |          NA |
| AterraEngineUnions_SuccessOrFailure_SwitchCase_Struct |  0.0549 ns | 0.0220 ns | 0.0226 ns |  0.0617 ns | 0.016 |    0.01 |      - |         - |          NA |
| AterraEngineUnions_SuccessOrFailure_SwitchCase_Value  |  2.9125 ns | 0.0790 ns | 0.0999 ns |  2.9027 ns | 0.831 |    0.03 | 0.0029 |      24 B |          NA |
| AterraEngineUnions_TrueFalse_TryGetAsTrue             |  3.5049 ns | 0.0466 ns | 0.0413 ns |  3.5123 ns | 1.000 |    0.02 |      - |         - |          NA |
| OneOf_SuccessOrFailure_SwitchCase_Value               |  5.8254 ns | 0.1374 ns | 0.1786 ns |  5.7972 ns | 1.662 |    0.05 | 0.0029 |      24 B |          NA |
| OneOfTrueFalse_TryGetAsTrue                           |  6.7111 ns | 0.1740 ns | 0.3226 ns |  6.6231 ns | 1.915 |    0.09 | 0.0076 |      64 B |          NA |
| OneOf_OneOfT8_SwitchCase_Value                        |  8.3716 ns | 0.2069 ns | 0.4671 ns |  8.2768 ns | 2.389 |    0.14 | 0.0076 |      64 B |          NA |
| OneOf_OneOfT8_TryGetAs                                | 10.9138 ns | 0.2539 ns | 0.2494 ns | 10.9407 ns | 3.114 |    0.08 | 0.0076 |      64 B |          NA |
| Dunet_TrueFalse_MatchTrue                             | 19.4857 ns | 0.4307 ns | 0.5448 ns | 19.2483 ns | 5.560 |    0.17 | 0.0210 |     176 B |          NA |


#### Enhanced benchmarks
More operations per invoke to return some more useful data.

| Method                                                  |      Mean |     Error |    StdDev |    Median |   Gen0 | Allocated |
|---------------------------------------------------------|----------:|----------:|----------:|----------:|-------:|----------:|
| AterraEngineUnions_UnionT8_SwitchCase_Value_Enhanced    |  6.570 ns | 0.1246 ns | 0.1484 ns |  6.546 ns | 0.0000 |         - |
| AterraEngineUnions_UnionT8_TryGetAs_Enhanced            | 12.463 ns | 0.2353 ns | 0.2201 ns | 12.380 ns | 0.0000 |         - |
| AterraEngineUnions_RefUnionT8_SwitchCase_Value_Enhanced | 14.122 ns | 0.3017 ns | 0.8800 ns | 14.111 ns | 0.0086 |      72 B |
| OneOf_OneOfT8_SwitchCase_Value_Enhanced                 | 15.656 ns | 0.3073 ns | 0.3155 ns | 15.679 ns | 0.0077 |      64 B |
| AterraEngineUnions_RefUnionT8_TryGetAs_Enhanced         | 16.210 ns | 0.3223 ns | 0.5295 ns | 15.958 ns | 0.0086 |      72 B |
| OneOf_OneOfT8_TryGetAs_Enhanced                         | 24.493 ns | 0.4747 ns | 0.6004 ns | 24.766 ns | 0.0077 |      64 B |

#### Recursive Benchmarks
Used to view the impact of ref Unions versus value Unions.


