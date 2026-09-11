namespace ParteIII.Core.Dtos;

public sealed class ResultDto<T>
{
    private ResultDto(bool success, T? value, IReadOnlyCollection<string> errors)
    {
        Success = success;
        Value = value;
        Errors = errors;
    }

    public bool Success { get; }

    public T? Value { get; }

    public IReadOnlyCollection<string> Errors { get; }

    public static ResultDto<T> Ok(T value) => new(true, value, []);

    public static ResultDto<T> Fail(string error) => new(false, default, [error]);

    public static ResultDto<T> Fail(IEnumerable<string> errors) =>
        new(false, default, errors.ToArray());
}