namespace Application.Common.Commands
{
    using Application.Abstraction.Command;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Domain.Entities;
    using Microsoft.Extensions.DependencyInjection;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public partial class UpdateCommandHandler<TDto, TModel>(IUnitOfWork _UnitOfWork, IRepository<TModel> _Repository, IMapper mapper, IServiceProvider _provider) : ICommandHandler<TDto>
        where TDto : ICommand
        where TModel : Domain.Entities.BaseModel
    {

        public virtual async Task<Result> Handle(TDto request, CancellationToken cancellationToken)
        {
            try
            {
                var ob = mapper.Map<TModel>(request);
                var res = await _Repository.UpdateAsync(ob);
                var resDetails = await SaveDetials(request);

                if (res && resDetails && _UnitOfWork.SaveChangeAsync().Result > 0)
                {
                    return new Result(HttpStatusCode.OK, null);
                }

                return new Result(
                    HttpStatusCode.InternalServerError,
                    new List<Error> { new Error("Error") });
            }
            catch (Exception ex)
            {
                return new Result(
                    HttpStatusCode.InternalServerError,
                    new List<Error> { new Error(ex.Message) });
            }
        }

        public virtual async Task<bool> SaveDetials(TDto request)
        {
            return true;
        }
        protected async Task<bool> UpdateDetails<TModelDetails>(IEnumerable<TModelDetails> Details)
          where TModelDetails : Domain.Entities.BaseModel
        {
            try
            {
                var repository = _provider.GetRequiredService<IRepository<TModelDetails>>();
                foreach (var item in Details)
                {
                    if (item.Id > 0)
                        await repository.UpdateAsync(item);
                    else
                        await repository.CreateAsync(item);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        protected async Task<bool> RemoveDetails<TModelDetails>(IEnumerable<TModelDetails> RemovedList)
          where TModelDetails : Domain.Entities.BaseModel
        {
            try
            {
                var repository = _provider.GetRequiredService<IRepository<TModelDetails>>();
                var res = await repository.ShiftDeleteAsync(e => RemovedList.Contains(e));
                return res;
            }
            catch
            {
                return false;
            }
        }
        protected async Task<string> CreateInclude()
        {
            return string.Empty;
        }
    }
}