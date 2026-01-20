namespace Domain.Shared
{
    using System.Net;

    public partial record Error(string MessageError , string Key = "");
    public partial record Result(HttpStatusCode StatusCode, List<Error>? Errors);
    public partial record Result<TResponse>(HttpStatusCode StatusCode, TResponse? Response, List<Error>? Errors);    
    public partial record ResultCollection<TResponse>(HttpStatusCode StatusCode, List<TResponse> Response , List<Error>? Errors);    
    public partial record ResultPagination<TResponse>(HttpStatusCode StatusCode, List<TResponse> Response , int Page , int PageSize , int PageCount, List<Error>? Errors);    
    public partial record Pagination(int Page , int PageSize , int PageCount);    
}