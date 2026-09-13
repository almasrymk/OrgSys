using Parties.Application.Dealers.Queries;
using CommercialDocuments.Application.InvoiceTypes.Commands;
using CommercialDocuments.Application.InvoiceTypes.Queries;
using OrgSys.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceTypeController(ISender sender) : BaseController<GetByIdInvoiceTypeQuery, SearchInvoiceTypeQuery , GetListInvoiceTypeQuery, CreateInvoiceTypeCommand, UpdateInvoiceTypeCommand, DeleteInvoiceTypeCommand, DeleteListInvoiceTypeCommand, InvoiceTypeDto>(sender)
    {

    }
}