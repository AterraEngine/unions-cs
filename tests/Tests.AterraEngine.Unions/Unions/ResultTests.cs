// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ResultTests {
    [Test]
    public async Task Result_HasState() {
        // Arrange
        Result result = Result.FromState(true);

        // Act
        bool isState = result.IsState;
        bool isError = result.IsError;
        object? value = result.Value;

        // Assert
        await Assert.That(isState).IsTrue();
        await Assert.That(isError).IsFalse();
        await Assert.That(value).IsEqualTo(true);
    }

    [Test]
    public async Task Result_HasError() {
        // Arrange
        Result result = Result.FromError("Failure message");

        // Act
        bool isState = result.IsState;
        bool isError = result.IsError;
        object? value = result.Value;

        // Assert
        await Assert.That(isState).IsFalse();
        await Assert.That(isError).IsTrue();
        await Assert.That(value).IsTypeOf<Error<string>>();
        await Assert.That(((Error<string>)value!).Value).IsEqualTo("Failure message");
    }

    [Test]
    public async Task Result_Generic_Success() {
        // Arrange
        Result<int> result = Result<int>.FromSuccess(42);

        // Act
        bool isSuccess = result.IsSuccess;
        bool isError = result.IsError;
        object? value = result.Value;

        // Assert
        await Assert.That(isSuccess).IsTrue();
        await Assert.That(isError).IsFalse();
        await Assert.That(value).IsEqualTo(42);
    }

    [Test]
    public async Task Result_Generic_Error() {
        // Arrange
        Result<int> result = Result<int>.FromError("Failure message");

        // Act
        bool isSuccess = result.IsSuccess;
        bool isError = result.IsError;
        object? value = result.Value;

        // Assert
        await Assert.That(isSuccess).IsFalse();
        await Assert.That(isError).IsTrue();
        await Assert.That(value).IsTypeOf<Error<string>>();
        await Assert.That(((Error<string>)value!).Value).IsEqualTo("Failure message");
    }

    [Test]
    public async Task TryGetState_Success() {
        // Arrange
        Result result = Result.FromState(true);

        // Act
        bool success = result.TryGetState(out bool state);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(state).IsTrue();
    }

    [Test]
    public async Task TryGetState_Failure() {
        // Arrange
        Result result = Result.FromError("Failure message");

        // Act
        bool success = result.TryGetState(out bool state);

        // Assert
        await Assert.That(success).IsFalse();
        await Assert.That(state).IsFalse();// Default value of bool
    }

    [Test]
    public async Task Generic_TryGetSuccess_Success() {
        // Arrange
        Result<int> result = Result<int>.FromSuccess(42);

        // Act
        bool success = result.TryGetAsSuccess(out int value);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(value).IsEqualTo(42);
    }

    [Test]
    public async Task Generic_TryGetSuccess_Failure() {
        // Arrange
        Result<int> result = Result<int>.FromError("Failure message");

        // Act
        bool success = result.TryGetAsSuccess(out int value);

        // Assert
        await Assert.That(success).IsFalse();
        await Assert.That(value).IsEqualTo(0);
    }

    [Test]
    public async Task Result_Match_State() {
        // Arrange
        Result result = Result.FromState(true);

        // Act
        string output = result.Match(
            stateCase: state => state ? "Success" : "Failure",
            errorCase: error => error.Value
        );

        // Assert
        await Assert.That(output).IsEqualTo("Success");
    }

    [Test]
    public async Task Result_Match_Error() {
        // Arrange
        Result result = Result.FromError("Failure message");

        // Act
        string output = result.Match(
            stateCase: state => state ? "Success" : "Failure",
            errorCase: error => error.Value
        );

        // Assert
        await Assert.That(output).IsEqualTo("Failure message");
    }

    [Test]
    public async Task Result_Generic_Match_Success() {
        // Arrange
        Result<int> result = Result<int>.FromSuccess(42);

        // Act
        string output = result.Match(
            successCase: success => $"Value: {success}",
            errorCase: error => error.Value
        );

        // Assert
        await Assert.That(output).IsEqualTo("Value: 42");
    }

    [Test]
    public async Task Result_Generic_Match_Error() {
        // Arrange
        Result<int> result = Result<int>.FromError("Error occurred");

        // Act
        string output = result.Match(
            successCase: success => $"Value: {success}",
            errorCase: error => error.Value
        );

        // Assert
        await Assert.That(output).IsEqualTo("Error occurred");
    }

    [Test]
    public async Task Result_Switch_State() {
        // Arrange
        Result result = Result.FromState(true);

        // Act
        string? output = null;

        result.Switch(
            stateCase: state => output = state ? "Success" : "Failure",
            errorCase: error => output = error.Value
        );

        // Assert
        await Assert.That(output).IsEqualTo("Success");
    }

    [Test]
    public async Task Result_Generic_Switch_Success() {
        // Arrange
        Result<int> result = Result<int>.FromSuccess(42);

        // Act
        string? output = null;

        result.Switch(
            successCase: success => output = $"Value: {success}",
            errorCase: error => output = error.Value
        );

        // Assert
        await Assert.That(output).IsEqualTo("Value: 42");
    }
}
