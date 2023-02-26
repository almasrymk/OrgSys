namespace OrgSys
{
    using X.PagedList;
    public class ReportResult<R>
    {
        public bool PrintMode  { get; set; }
        public R Data { get; set; }
        public IPagedList<R> Result { get; set; }
    }
}