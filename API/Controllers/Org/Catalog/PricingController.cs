using Catalog.Contracts.Pricing;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Catalog
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PricingController(ISender sender) : ControllerBase
    {
        [HttpGet("ResolvePrice")]
        public virtual async Task<OrgSys.SharedKernel.Result<ResolvedPriceDto?>> ResolvePrice(long productId, long? priceListId, decimal quantity, long? unitId, DateTime date, CancellationToken cancellationToken)
        {
            return await sender.Send(new ResolvePriceQuery(productId, priceListId, quantity, unitId, date), cancellationToken);
        }
    }
}
