namespace CleanArch.UnitTests;

public class ValidationBehaviorCommandHandlerTests
{
    public record TestCommand(string Value) : ICommand<string>;

    [Fact]
    public async Task HandleAsync_WhenNoValidators_ThenCallsInnerHandler()
    {
        // Arrange
        var mockInnerHandler = new Mock<ICommandHandler<TestCommand, string>>();
        mockInnerHandler
            .Setup(h => h.HandleAsync(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success("OK"));

        var handler = new ValidationBehavior.CommandHandler<TestCommand, string>(
            mockInnerHandler.Object,
            []
        );

        var command = new TestCommand("data");

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("OK", result.Value);
        mockInnerHandler.Verify(h => h.HandleAsync(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenValidationPasses_ThenCallsInnerHandler()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<TestCommand>>();
        mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestCommand>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var mockInnerHandler = new Mock<ICommandHandler<TestCommand, string>>();
        mockInnerHandler
            .Setup(h => h.HandleAsync(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success("OK"));

        var handler = new ValidationBehavior.CommandHandler<TestCommand, string>(
            mockInnerHandler.Object,
            [mockValidator.Object]
        );

        var command = new TestCommand("data");

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("OK", result.Value);
        mockInnerHandler.Verify(h => h.HandleAsync(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenValidationFails_ThenReturnsFailureAndSkipInnerHandler()
    {
        // Arrange
        var failures = new[]
        {
            new ValidationFailure("Field1", "Error 1") { ErrorCode = "Code1" },
            new ValidationFailure("Field2", "Error 2") { ErrorCode = "Code2" }
        };

        var mockValidator = new Mock<IValidator<TestCommand>>();
        mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestCommand>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        var mockInnerHandler = new Mock<ICommandHandler<TestCommand, string>>();

        var handler = new ValidationBehavior.CommandHandler<TestCommand, string>(
            mockInnerHandler.Object,
            [mockValidator.Object]
        );

        var command = new TestCommand("invalid");

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.IsType<ErrorList>(result.Error);

        var errorList = (ErrorList)result.Error;
        Assert.Equal("ValidationBehavior", errorList.Code);
        Assert.Equal(ErrorType.Validation, errorList.Type);
        Assert.Equal(2, errorList.Errors.Length);

        mockInnerHandler.Verify(h => h.HandleAsync(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

public class ValidationBehaviorCommandBaseHandlerTests
{
    public record TestCommand() : ICommand;

    [Fact]
    public async Task HandleAsync_WhenNoValidators_ThenCallsInnerHandler()
    {
        var mockInnerHandler = new Mock<ICommandHandler<TestCommand>>();
        mockInnerHandler
            .Setup(h => h.HandleAsync(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var handler = new ValidationBehavior.CommandBaseHandler<TestCommand>(
            mockInnerHandler.Object,
            []
        );

        var command = new TestCommand();
        var result = await handler.HandleAsync(command);

        Assert.True(result.IsSuccess);
        mockInnerHandler.Verify(h => h.HandleAsync(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenValidationFails_ThenReturnsFailureAndSkipInnerHandler()
    {
        var failures = new[]
        {
            new ValidationFailure("Field", "Base Error") { ErrorCode = "BaseCode" }
        };

        var mockValidator = new Mock<IValidator<TestCommand>>();
        mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestCommand>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        var mockInnerHandler = new Mock<ICommandHandler<TestCommand>>();

        var handler = new ValidationBehavior.CommandBaseHandler<TestCommand>(
            mockInnerHandler.Object,
            [mockValidator.Object]
        );

        var command = new TestCommand();
        var result = await handler.HandleAsync(command);

        Assert.False(result.IsSuccess);
        Assert.IsType<ErrorList>(result.Error);

        var errorList = (ErrorList)result.Error;
        Assert.Equal("ValidationBehavior", errorList.Code);
        Assert.Single(errorList.Errors);

        mockInnerHandler.Verify(h => h.HandleAsync(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}


public class ValidationBehaviorQueryHandlerTests
{
    public record TestQuery(string Query) : IQuery<int>;

    [Fact]
    public async Task HandleAsync_WhenValidationPasses_ThenCallsInnerHandler()
    {
        var mockValidator = new Mock<IValidator<TestQuery>>();
        mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestQuery>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var mockInnerHandler = new Mock<IQueryHandler<TestQuery, int>>();
        mockInnerHandler
            .Setup(h => h.HandleAsync(It.IsAny<TestQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(42));

        var handler = new ValidationBehavior.QueryHandler<TestQuery, int>(
            mockInnerHandler.Object,
            [mockValidator.Object]
        );

        var query = new TestQuery("ok");
        var result = await handler.HandleAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
        mockInnerHandler.Verify(h => h.HandleAsync(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenValidationFails_ThenReturnsFailureAndSkipInnerHandler()
    {
        var failures = new[]
        {
            new ValidationFailure("QueryField", "Invalid query") { ErrorCode = "QErr" }
        };

        var mockValidator = new Mock<IValidator<TestQuery>>();
        mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestQuery>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        var mockInnerHandler = new Mock<IQueryHandler<TestQuery, int>>();

        var handler = new ValidationBehavior.QueryHandler<TestQuery, int>(
            mockInnerHandler.Object,
            [mockValidator.Object]
        );

        var query = new TestQuery("bad");
        var result = await handler.HandleAsync(query);

        Assert.False(result.IsSuccess);
        Assert.IsType<ErrorList>(result.Error);

        var errorList = (ErrorList)result.Error;
        Assert.Equal("ValidationBehavior", errorList.Code);
        Assert.Single(errorList.Errors);

        mockInnerHandler.Verify(h => h.HandleAsync(It.IsAny<TestQuery>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

