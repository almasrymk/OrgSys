namespace Administration.Application.Preferences.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    
    using Administration.Domain;
    
    using System.Net;

    public sealed class UpdatePreferenceCommand : Administration.Application.PreferenceDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Administration.Domain.Preference> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdatePreferenceCommand, Administration.Domain.Preference>(_UnitOfWork, _Repository , mapper , _provider)
    {

        public override async Task<Result> Handle(UpdatePreferenceCommand request, CancellationToken cancellationToken)
        {
            var nextId = await _Repository.AnyAsync(r => true)
                ? await _Repository.GetMaxAsync(r => r.Id)
                : 0;

            foreach (var item in (request.PreferenceList ?? Enumerable.Empty<PreferenceDto>())
                .GroupBy(r => new { r.Key, r.TypeId, r.Reference })
                .Select(group => group.Last()))
            {
                var ob = await _Repository.GetByFilterAsync(r => r.Key == item.Key && r.TypeId == item.TypeId && r.Reference == item.Reference, "");

                if (ob != null)
                    ob.Value = item.Value;
                else
                    await _Repository.CreateAsync(new Administration.Domain.Preference
                    {
                        Id = ++nextId,
                        Key = item.Key,
                        Value = item.Value,
                        Reference = item.Reference,
                        TypeId = item.TypeId,
                        Hide = false
                    });
            }
            if(await _UnitOfWork.SaveChangeAsync() > 0)
            {

               return new Result(HttpStatusCode.OK, null);
            }

            return new Result(
                    HttpStatusCode.InternalServerError,
                    new List<Error> { new Error("Error") });
        }

    }
}
