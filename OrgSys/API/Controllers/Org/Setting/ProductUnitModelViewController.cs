using Application.Commands.Org.Setting.ProductUnit.Queries;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class ProductUnitModelViewController(ISender sender) : CoreController<GetByIdProductUnitQuery, SearchProductUnitQuery, GetListProductUnitQuery, ProductUnitModelView>(sender)
    {
        
    }
}