namespace Application.Commands.Org.Financials.Journal.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Domain.Entities;

    public sealed class UpdateJournalCommand : Application.DTOs.JournalDto, ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Domain.Entities.Journal> _Repository,
        IRepository<Domain.Entities.JournalItem> _RepositoryJournalInvoice,
        IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateJournalCommand, Domain.Entities.Journal>(_UnitOfWork, _Repository, mapper, _provider)
    {

        override public async Task<bool> SaveDetials(UpdateJournalCommand request)
        {
            #region UpdateProduct
            var ids = request.JournalItems.Select(e => e.Id);
            var removeList = await _RepositoryJournalInvoice.GetListByFilterAsync(e => e.JournalId == request.Id && !ids.Contains(e.Id));

            var res = await RemoveDetails<JournalItem>(removeList!);
            if (!res) return false;
            var ob = mapper.Map<List<JournalItem>>(request.JournalItems);
            res = await UpdateDetails<JournalItem>(ob);
            #endregion 

            return res;
        }
    }
}