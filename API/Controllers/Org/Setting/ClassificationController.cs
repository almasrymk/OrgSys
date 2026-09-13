using MasterData.Application.Classifications.Commands;
using MasterData.Application.Classifications.Queries;
using OrgSys.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class ClassificationController(ISender sender) : BaseController<GetByIdClassificationQuery, SearchClassificationQuery , GetListClassificationQuery, CreateClassificationCommand, UpdateClassificationCommand, DeleteClassificationCommand, DeleteListClassificationCommand, ClassificationDto>(sender)
    {

    }
}