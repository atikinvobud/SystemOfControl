namespace BackEnd.Share;

public class Result<T>
{
    public bool isSuccess { get; set; }
    public T? value { get; set; }
    public string? errorMessage { get; set; }
    public ErrorCode? code { get; set; }

    private Result(bool isSuccess, T? value, ErrorCode? code = null)
    {
        this.isSuccess = isSuccess;
        this.value = value;
        if (code.HasValue)
        {
            errorMessage = ErorrService.GetMessage(code);
            this.code = code;
        }
    }
    public static Result<T> Success(T value) => new(true, value);
    public static Result<T> Error(ErrorCode code) => new(false, default, code);
    public static Result<bool> CheckToken(Guid? userId)
    {
        if (userId == null)
            return new Result<bool>(false, default, ErrorCode.InvalidAccessToken);

        return Result<bool>.Success(true);
    }
}