﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using None=AterraEngine.Unions.None;
using True=AterraEngine.Unions.True;
using TrueOrFalse=OneOf.Types.TrueOrFalse;

// To fix the warnings on the classes
// ReSharper disable InconsistentNaming

namespace Benchmarks.AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class DiscriminatedUnionsBenchmark {
    [Benchmark(Baseline = true)]
    public True? AterraEngineUnions_TrueFalse_TryGetAsTrue() {
        global::AterraEngine.Unions.TrueOrFalse union = new True();
        if (union.TryGetAsTrue(out True result)) return result;

        return null;
    }

    [Benchmark]
    public Success<string>? AterraEngineUnions_SuccessOrFailure_SwitchCase_Struct() {
        SuccessOrFailure<string, None> union = new Success<string>("Something as success");
        switch (union) {
            case { IsSuccess: true, AsSuccess: var successValue }: return successValue;
            case { IsFailure: true, AsFailure: var failureValue }: return null;
            default: return default!;
        }
    }

    [Benchmark]
    public Success<string>? AterraEngineUnions_SuccessOrFailure_SwitchCase_Value() {
        SuccessOrFailure<string, None> union = new Success<string>("Something as success");
        switch (union.Value) {
            case Success<string> success: return success;
            case Failure<None>: return null;
            default: return default!;
        }
    }

    [Benchmark]
    public string? AterraEngineUnions_UnionT8_SwitchCase_Value() {
        Union_T8 union = "value";
        switch (union.Value) {
            case bool: return null;
            case int: return null;
            case List<string>: return null;
            case float: return null;
            case double: return null;
            case short: return null;
            case Dictionary<int, bool>: return null;
            case string value: return value;

            default: return null;
        }
    }

    [Benchmark]
    public string? AterraEngineUnions_UnionT8_TryGetAs() {
        Union_T8 union = "value";
        if (union.TryGetAsString(out string? result)) return result;

        return null;
    }

    // ---------------------------------------------------------------------------------------------------------------------
    // OneOf
    // ---------------------------------------------------------------------------------------------------------------------
    [Benchmark]
    public TrueOrFalse.True? OneOfTrueFalse_TryGetAsTrue() {
        TrueOrFalse union = new TrueOrFalse.True();
        if (union.TryPickT0(out TrueOrFalse.True result, out _)) return result;

        return null;
    }

    [Benchmark]
    public OneOf.Types.Success<string>? OneOf_SuccessOrFailure_SwitchCase_Value() {
        OneOf_SuccessOrFailure<string, string> union = new OneOf.Types.Success<string>();

        switch (union.Value) {
            case OneOf.Types.Success<string> successValue: return successValue;
            case OneOf_SuccessOrFailure<string, string>.Failure<string>: return null;
            default: return default!;
        }
    }

    [Benchmark]
    public string? OneOf_OneOfT8_SwitchCase_Value() {
        OneOf_T8 union = "value";
        switch (union.Value) {
            case bool: return null;
            case int: return null;
            case List<string>: return null;
            case float: return null;
            case double: return null;
            case short: return null;
            case Dictionary<int, bool>: return null;
            case string value: return value;

            default: return null;
        }
    }

    [Benchmark]
    public string? OneOf_OneOfT8_TryGetAs() {
        OneOf_T8 union = "value";
        if (union.TryPickT7(out string result, out _)) return result;

        return null;
    }

    // ---------------------------------------------------------------------------------------------------------------------
    // Dunet
    // ---------------------------------------------------------------------------------------------------------------------
    [Benchmark]
    public Dunet_TrueOrFalse.Dunet_True Dunet_TrueFalse_MatchTrue() {
        Dunet_TrueOrFalse union = new Dunet_TrueOrFalse.Dunet_True();
        Dunet_TrueOrFalse.Dunet_True? result = null;
        union.MatchDunet_True(
            dunet_True: @true => result = @true,
            @else: () => result = null
        );

        return result!;
    }
}
