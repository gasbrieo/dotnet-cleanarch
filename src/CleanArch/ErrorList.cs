namespace CleanArch;

public record ErrorList : Error
{
    public Error[] Errors { get; }

    public ErrorList(string code, Error[] errors) : base(code, "One or more validation errors occurred", ErrorType.Validation)
    {
        Errors = errors;
    }
}
