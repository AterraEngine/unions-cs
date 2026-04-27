// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Unions;

namespace CodeOfChaosTests.Unions.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SuccessTest {
    [Test]
    public async Task SuccessEmpty_IsSame() {
        // Arrange
        var success1 = Success.Empty;
        var success2 = Success.Empty;
        
        // Act &  Assert
        await Assert.That(success1).IsEqualTo(success2);
    }

    [Test]
    public async Task GenericSuccessEmpty_IsSame() {
        // Arrange
        var success1 = Success<string>.Empty;
        var success2 = Success<string>.Empty;
        
        // Act &  Assert
        await Assert.That(success1).IsEqualTo(success2);
    }
}
