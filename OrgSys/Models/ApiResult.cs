using System.Collections.Generic;

namespace OrgSys.Models
{
    public class ApiResultCollaction<TDto>
    {
        public int StatusCode { get; set; }
        public List<TDto> Response { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public object Errors { get; set; }
    }
}