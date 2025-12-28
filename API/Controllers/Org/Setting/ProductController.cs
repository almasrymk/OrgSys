using Application.Commands.Org.Setting.Preference.Queries;
using Application.Commands.Org.Setting.Product.Commands;
using Application.Commands.Org.Setting.Product.Queries;
using Application.Interfaces.CQRS;
using Azure;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController(ISender sender) : BaseController<GetByIdProductQuery, SearchProductQuery , GetListProductQuery, CreateProductCommand, UpdateProductCommand, DeleteProductCommand, DeleteListProductCommand, ProductModelView>(sender)
    {

    }
}