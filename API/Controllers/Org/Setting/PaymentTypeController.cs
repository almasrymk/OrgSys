using MasterData.Application.PaymentTypes.Commands;
using MasterData.Application.PaymentTypes.Queries;
using OrgSys.SharedKernel;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentTypeController(ISender sender) : BaseController<GetByIdPaymentTypeQuery, SearchPaymentTypeQuery , GetListPaymentTypeQuery, CreatePaymentTypeCommand, UpdatePaymentTypeCommand, DeletePaymentTypeCommand, DeleteListPaymentTypeCommand, PaymentTypeDto>(sender)
    {

    }
}