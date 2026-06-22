using Application.Commands.Org.Setting.InvoiceType.Queries;
using Application.Commands.Org.Setting.PaymentType.Commands;
using Application.Commands.Org.Setting.PaymentType.Queries;
using Application.Interfaces.CQRS;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentTypeController(ISender sender) : BaseController<GetByIdPaymentTypeQuery, SearchPaymentTypeQuery , GetListPaymentTypeQuery, CreatePaymentTypeCommand, UpdatePaymentTypeCommand, DeletePaymentTypeCommand, DeleteListPaymentTypeCommand, PaymentTypeModelView>(sender)
    {

    }
}