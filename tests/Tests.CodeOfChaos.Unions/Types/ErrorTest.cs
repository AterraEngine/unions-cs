// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Unions;

namespace Tests.CodeOfChaos.Unions.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ErrorTest {
    [Test]
    public async Task ErrorEmpty_IsSame() {
        // Arrange
        var error1 = Error.Empty;
        var error2 = Error.Empty;
        
        // Act &  Assert
        await Assert.That(error1).IsEqualTo(error2);
    }

    [Test]
    public async Task GenericErrorEmpty_IsSame() {
        // Arrange
        var error1 = Error<string>.Empty;
        var error2 = Error<string>.Empty;
        
        // Act &  Assert
        await Assert.That(error1).IsEqualTo(error2);
    }
}
