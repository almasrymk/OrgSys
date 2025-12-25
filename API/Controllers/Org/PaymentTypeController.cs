using Application.Commands.Org.Setting.PaymentType.Commands;
using Application.Commands.Org.Setting.PaymentType.Queries;
using Application.Interfaces.CQRS;
using Azure;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.PaymentType
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentTypeController(ISender sender) : BaseController<GetByIdPaymentTypeQuery, SearchPaymentTypeQuery, CreatePaymentTypeCommand, UpdatePaymentTypeCommand, DeletePaymentTypeCommand, DeleteListPaymentTypeCommand, PaymentTypeModelView>(sender)
    {

    }
}
