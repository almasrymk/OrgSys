namespace OrgSys.SharedKernel
{
    using AutoMapper;
    using System.Linq.Expressions;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetMaxCommandHandler<TRequest, TModel>(IRepository<TModel> _Repository) : ICommandObHandler<TRequest, object>
        where TRequest : ICommandOb<object>
        where TModel : OrgSys.SharedKernel.BaseModel 
    {
        public virtual async Task<object> Handle(TRequest request, CancellationToken cancellationToken)
        {
            try
            {                
                return await _Repository.GetMaxByFilterAsync(CreateFilter(request), CreateSelector());               
            }
            catch
            {
                throw;
            }
        }

        public virtual Expression<Func<TModel, bool>> CreateFilter(TRequest request)
        {
            return e => true;
        }

        public virtual Expression<Func<TModel, object>> CreateSelector()
        {
            throw new NotImplementedException();
        }
    }
}