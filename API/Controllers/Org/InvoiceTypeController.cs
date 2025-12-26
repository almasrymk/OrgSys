using Application.Commands.Org.Setting.Dealer.Queries;
using Application.Commands.Org.Setting.InvoiceType.Commands;
using Application.Commands.Org.Setting.InvoiceType.Queries;
using Application.Interfaces.CQRS;
using Azure;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.InvoiceType
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceTypeController(ISender sender) : BaseController<GetByIdInvoiceTypeQuery, SearchInvoiceTypeQuery , GetListInvoiceTypeQuery, CreateInvoiceTypeCommand, UpdateInvoiceTypeCommand, DeleteInvoiceTypeCommand, DeleteListInvoiceTypeCommand, InvoiceTypeModelView>(sender)
    {

    }
}