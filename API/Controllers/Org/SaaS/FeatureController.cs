using SaaS.Application;
using SaaS.Application.Features.Commands;
using SaaS.Application.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.SaaS
{
    [ApiController]
    [Route("[controller]")]
    public class FeatureController(ISender sender) : BaseController<GetByIdFeatureQuery, SearchFeatureQuery, GetListFeatureQuery, CreateFeatureCommand, UpdateFeatureCommand, DeleteFeatureCommand, DeleteListFeatureCommand, FeatureDto>(sender)
    {
    }
}
