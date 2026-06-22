using Application.Commands.Org.Setting.Dealer.Queries;
using Application.Commands.Org.Setting.InvoiceType.Commands;
using Application.Commands.Org.Setting.InvoiceType.Queries;
using Application.Interfaces.CQRS;
using Domain.Shared;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceTypeController(ISender sender) : BaseController<GetByIdInvoiceTypeQuery, SearchInvoiceTypeQuery , GetListInvoiceTypeQuery, CreateInvoiceTypeCommand, UpdateInvoiceTypeCommand, DeleteInvoiceTypeCommand, DeleteListInvoiceTypeCommand, InvoiceTypeModelView>(sender)
    {

    }
}