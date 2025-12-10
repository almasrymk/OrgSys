namespace Domain.Shared
{
    using System.Net;

    public partial record Result(HttpStatusCode StatusCode, List<string>? Errors);
    public partial record Result<TResponse>(HttpStatusCode StatusCode, TResponse? Response, List<string>? Errors);    
    public partial record ResultCollection<TResponse>(HttpStatusCode StatusCode, List<TResponse> Response , List<string>? Errors);    
    public partial record ResultPagination<TResponse>(HttpStatusCode StatusCode, List<TResponse> Response , int Page , int PageSize , int PageCount, List<string>? Errors);    
    public partial record Pagination(int Page , int PageSize , int PageCount);    
}