namespace CleanArch.UnitTests;

public class LoggingBehaviorCommandHandlerTests
{
    public record TestCommand(string Value) : ICommand<string>;

    [Fact]
    public async Task HandleAsync_WhenSuccess_ThenLogsStartAndCompletion()
    {
        // Arrange
        var mockInnerHandler = new Mock<ICommandHandler<TestCommand, string>>();
        mockInnerHandler
            .Setup(h => h.HandleAsync(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success("OK"));

        var mockLogger = new Mock<ILogger<LoggingBehavior.CommandHandler<TestCommand, string>>>();

        var handler = new LoggingBehavior.CommandHandler<TestCommand, string>(
            mockInnerHandler.Object,
            mockLogger.Object
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
    public async Task HandleAsync_WhenFailure_ThenLogsStartAndError()
    {
        // Arrange
        var error = Error.Failure("TestError", "Something failed");

        var mockInnerHandler = new Mock<ICommandHandler<TestCommand, string>>();
        mockInnerHandler
            .Setup(h => h.HandleAsync(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<string>(error));

        var mockLogger = new Mock<ILogger<LoggingBehavior.CommandHandler<TestCommand, string>>>();

        var handler = new LoggingBehavior.CommandHandler<TestCommand, string>(
            mockInnerHandler.Object,
            mockLogger.Object
        );

        var command = new TestCommand("data");

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);

        mockInnerHandler.Verify(h => h.HandleAsync(command, It.IsAny<CancellationToken>()), Times.Once);
    }
}

public class LoggingBehaviorCommandBaseHandlerTests
{
    public record TestCommand() : ICommand;

    [Fact]
    public async Task HandleAsync_WhenSuccess_ThenLogsStartAndCompletion()
    {
        // Arrange
        var mockInnerHandler = new Mock<ICommandHandler<TestCommand>>();
        mockInnerHandler
            .Setup(h => h.HandleAsync(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var mockLogger = new Mock<ILogger<LoggingBehavior.CommandBaseHandler<TestCommand>>>();

        var handler = new LoggingBehavior.CommandBaseHandler<TestCommand>(
            mockInnerHandler.Object,
            mockLogger.Object
        );

        var command = new TestCommand();

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);

        mockInnerHandler.Verify(h => h.HandleAsync(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenFailure_ThenLogsError()
    {
        // Arrange
        var error = Error.Failure("Err", "Failed");

        var mockInnerHandler = new Mock<ICommandHandler<TestCommand>>();
        mockInnerHandler
            .Setup(h => h.HandleAsync(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(error));

        var mockLogger = new Mock<ILogger<LoggingBehavior.CommandBaseHandler<TestCommand>>>();

        var handler = new LoggingBehavior.CommandBaseHandler<TestCommand>(
            mockInnerHandler.Object,
            mockLogger.Object
        );

        var command = new TestCommand();

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
    }
}

public class LoggingBehaviorQueryHandlerTests
{
    public record TestQuery(string Query) : IQuery<int>;

    [Fact]
    public async Task HandleAsync_WhenSuccess_ThenLogsStartAndCompletion()
    {
        // Arrange
        var mockInnerHandler = new Mock<IQueryHandler<TestQuery, int>>();
        mockInnerHandler
            .Setup(h => h.HandleAsync(It.IsAny<TestQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(42));

        var mockLogger = new Mock<ILogger<LoggingBehavior.QueryHandler<TestQuery, int>>>();

        var handler = new LoggingBehavior.QueryHandler<TestQuery, int>(
            mockInnerHandler.Object,
            mockLogger.Object
        );

        var query = new TestQuery("test");

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public async Task HandleAsync_WhenFailure_ThenLogsError()
    {
        // Arrange
        var error = Error.Failure("Err", "Query failed");

        var mockInnerHandler = new Mock<IQueryHandler<TestQuery, int>>();
        mockInnerHandler
            .Setup(h => h.HandleAsync(It.IsAny<TestQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<int>(error));

        var mockLogger = new Mock<ILogger<LoggingBehavior.QueryHandler<TestQuery, int>>>();

        var handler = new LoggingBehavior.QueryHandler<TestQuery, int>(
            mockInnerHandler.Object,
            mockLogger.Object
        );

        var query = new TestQuery("test");

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
    }
}
