// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions.Generators.Sample;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// [UnionExtra(UnionExtra.GenerateAsValue)]
// public readonly partial struct SuccessOrFailure<TSuccess, TFailure>() : IUnion<Success<TSuccess>, Failure<TFailure>>, ISuccessOrFailure<TSuccess, TFailure> where TSuccess : notnull where TFailure : notnull {
//     
//     public bool TryGetAsSuccessValue(out TSuccess value) {
//         if (IsSuccess) {
//             value = AsSuccess.Value;
//             return true;
//         }
//         value = default!;
//         return false;
//     }
//     
// }


public partial record RecordUnion() : IUnion<int, string> { }
