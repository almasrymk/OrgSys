using Application.Commands.Org.Financials.FinancialType.Commands;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Financials
{
    [Route("[controller]")]
    [ApiController]
    public class FinancialTypeController(ISender sender) : BaseController<GetByIdFinancialTypeQuery, SearchFinancialTypeQuery, GetListFinancialTypeQuery,
        CreateFinancialTypeCommand, UpdateFinancialTypeCommand, DeleteFinancialTypeCommand, DeleteListFinancialTypeCommand,
        GetMaxFinancialTypeQuery, FinancialTypeModelView>(sender)
    {
    }
}
