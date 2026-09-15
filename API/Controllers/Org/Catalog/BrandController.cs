using Catalog.Application.Brands.Commands;
using Catalog.Application.Brands.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Catalog
{
    [ApiController]
    [Route("[controller]")]
    public class BrandController(ISender sender) : BaseController<GetByIdBrandQuery, SearchBrandQuery, GetListBrandQuery, CreateBrandCommand, UpdateBrandCommand, DeleteBrandCommand, DeleteListBrandCommand, BrandDto>(sender)
    {

    }
}
