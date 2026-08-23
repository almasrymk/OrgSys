namespace Application.Commands.Org.Financials.Financial.Commands
{
    using Application.Abstraction.Command;
    using Application.Commands.Org.Setting.Product.Commands;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Domain.Entities;
    using Application.DTOs;
    using System.Net;

    public sealed class UpdateFinancialCommand : Application.DTOs.FinancialDto, ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Domain.Entities.Financial> _Repository ,
        IRepository<Domain.Entities.FinancialInvoice> _RepositoryFinancialInvoice,
        IRepository<Domain.Entities.Invoice> _RepositoryInvoice,
        IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateFinancialCommand, Domain.Entities.Financial>(_UnitOfWork, _Repository , mapper , _provider)
    {

        public override async Task<Result> Handle(UpdateFinancialCommand request, CancellationToken cancellationToken)
        {
            //_RepositoryFinancialInvoice.GetListByFilterAsync()
            //await _UnitOfWork.BeginTransactionAsync();

            var finanicial = await _Repository.GetByFilterAsync(e => e.Id == request.Id, "FinancialInvoices") ?? new Financial();

            // A Posted financial transaction (e.g. a Customer Receipt) is immutable — correct it via
            // Reverse, never by editing the row a Posted Journal Entry was already generated from.
            if (finanicial.Posted)
                return new Result(HttpStatusCode.Forbidden, [new Error("A Posted financial transaction cannot be edited. Use Reverse instead.")]);



            foreach (var item in finanicial.FinancialInvoices!)
            {
                var invoice = await _RepositoryInvoice.GetByFilterAsync(e => e.Id == item.InvoiceId,"");
                invoice!.Credit += item.Amount;
                invoice.Paid -= item.Amount;
               await _RepositoryInvoice.UpdateAsync(invoice);
            }

            foreach (var item in request.FinancialInvoices!)
            {
                var invoice = await _RepositoryInvoice.GetByFilterAsync(e => e.Id == item.InvoiceId, "");
                invoice!.Credit -= item.Amount;
                invoice.Paid += item.Amount;
                await _RepositoryInvoice.UpdateAsync(invoice);
            }

            finanicial.FinancialInvoices.Clear();
            await _RepositoryFinancialInvoice.CreateAsync(request.FinancialInvoices.ToList());
            return await base.Handle(request, cancellationToken);
        }

    }
}