using Catalog.Application.PriceLists.Commands;
using Catalog.Application.PriceLists.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Catalog
{
    [ApiController]
    [Route("[controller]")]
    public class PriceListController(ISender sender) : BaseController<GetByIdPriceListQuery, SearchPriceListQuery, GetListPriceListQuery, CreatePriceListCommand, UpdatePriceListCommand, DeletePriceListCommand, DeleteListPriceListCommand, PriceListDto>(sender)
    {

    }
}
