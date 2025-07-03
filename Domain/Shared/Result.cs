namespace Domain.Shared
{
    using System.Net;

    public partial record Result(HttpStatusCode StatusCode, List<string>? Errors);
    public partial record Result<TResponse>(HttpStatusCode StatusCode, TResponse? Response, List<string>? Errors);
    //public class MyResult<T>
    //{
    //    public bool Success { get; set; }
    //    public string? Message { get; set; }
    //    public T? Data { get; set; }

    //    public static MyResult<T> SuccessResult(T data, string message = "") =>
    //        new() { Success = true, Data = data, Message = message };

    //    public static MyResult<T> Failure(string message) =>
    //        new() { Success = false, Message = message };
    //}
}