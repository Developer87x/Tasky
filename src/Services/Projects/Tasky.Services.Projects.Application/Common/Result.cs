namespace Tasky.Services.Projects.Application.Common;

public class Result
{
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }

    protected Result(bool isSuccess, string error)
    {
        IsSuccess =isSuccess;
        Error = error;
    }

    public static Result Success()=> new(true,string.Empty);
    public static Result Failure(string error)=> new (false,error);
}

public class Result<T> : Result
{
    public T Value { get; set; }
    protected Result(T value,bool isSuccess, string error) : base(isSuccess, error)
    {
        Value =value;
    }
    public static Result<T> Success(T value)=> new(value,true,string.Empty);
    public static new Result<T> Failure(string error)=> new(default!,false,error);
}