namespace CleanArch.UnitTests;

public class ResultTests
{
    [Fact]
    public void Constructor_WhenInvalidErrorCombination_ThenThrowsArgumentException()
    {
        // Assert
        Assert.Throws<ArgumentException>(() => new Result(true, Error.Failure("X", "Y")));
        Assert.Throws<ArgumentException>(() => new Result(false, Error.None));
    }

    [Fact]
    public void Success_WhenCalled_ThenCreatesSuccessResult()
    {
        // Act
        var result = Result.Success();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);
    }

    [Fact]
    public void Success_WhenCalledWithValue_ThenCreatesSuccessResultWithValue()
    {
        // Act
        var result = Result.Success("hello");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("hello", result.Value);
    }

    [Fact]
    public void Failure_WhenCalled_ThenCreatesFailureResult()
    {
        // Arrange
        var error = Error.Failure("FailureCode", "Something went wrong");

        // Act
        var result = Result.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Failure_WhenCalledWithError_ThenCreatesFailureResult()
    {
        var error = Error.Problem("P", "Problem");

        // Act
        var result = Result.Failure<string>(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
        Assert.Throws<InvalidOperationException>(() => _ = result.Value);
    }

    [Fact]
    public void ImplicitConversion_WhenValueIsNotNull_ThenCreatesSuccessResult()
    {
        // Act
        Result<string> result = "test";

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("test", result.Value);
    }

    [Fact]
    public void ImplicitConversion_WhenValueIsNull_ThenCreatesFailureWithNullValueError()
    {
        // Act
        Result<string> result = (string?)null;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.NullValue, result.Error);
    }
}
