// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class FailureTest {
    [Test]
    public async Task FailureEmpty_IsSame() {
        // Arrange
        var failure1 = Failure.Empty;
        var failure2 = Failure.Empty;
        
        // Act &  Assert
        await Assert.That(failure1).IsEqualTo(failure2);
    }

    [Test]
    public async Task GenericFailureEmpty_IsSame() {
        // Arrange
        var failure1 = Failure<string>.Empty;
        var failure2 = Failure<string>.Empty;
        
        // Act &  Assert
        await Assert.That(failure1).IsEqualTo(failure2);
    }
}
