namespace CleanArch.UnitTests;

public class ErrorListTests
{
    [Fact]
    public void Constructor_WhenCalled_ThenSetsPropertiesCorrectly()
    {
        // Arrange
        var errors = new[]
        {
            Error.Failure("Code1", "First error"),
            Error.Failure("Code2", "Second error")
        };

        // Act
        var errorList = new ErrorList("ValidationFailed", errors);

        // Assert
        Assert.Equal("ValidationFailed", errorList.Code);
        Assert.Equal("One or more validation errors occurred", errorList.Description);
        Assert.Equal(ErrorType.Validation, errorList.Type);
        Assert.Equal(errors, errorList.Errors);
    }
}
