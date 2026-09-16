using AutoMapper;
using CommercialDocuments.Contracts.Invoices;
using MediatR;
using System.Net;

namespace Treasury.Application.Financials.Commands
{
    public record RedoFinancialCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public class RedoFinancialCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Treasury.Domain.Financial> _Repository,
        ISender sender,
        IMapper mapper, IServiceProvider _provider
        )
        : UpdateCommandHandler<RedoFinancialCommand, Treasury.Domain.Financial>(_UnitOfWork, _Repository, mapper, _provider)
    {
        public override async Task<Result> Handle(RedoFinancialCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var finanicial = await _Repository
                    .GetByFilterAsync(e => e.Id == request.Id, "FinancialInvoices") ?? new();

                if (finanicial.Posted)
                    return new Result(HttpStatusCode.Forbidden, [new Error("A Posted financial transaction cannot be redone. Use Reverse instead.")]);

                foreach (var item in finanicial.FinancialInvoices ?? [])
                {
                    await sender.Send(new AdjustInvoiceSettlementCommand(item.InvoiceId ?? 0, item.Amount), cancellationToken);
                    item.Status = OrgSys.SharedKernel.Status.New;
                }

                finanicial.Status = OrgSys.SharedKernel.Status.New;
                await _UnitOfWork.SaveChangeAsync();
                return new Result(HttpStatusCode.OK, null);
            }
            catch (Exception)
            {
                return new Result(
                          HttpStatusCode.InternalServerError,
                          new List<Error> { new Error("Error") });
            }
        }
    }
}
