using MasterData.Application.ReferenceTypes.Commands;
using MasterData.Application.ReferenceTypes.Queries;
using OrgSys.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class ReferenceTypeController(ISender sender) : BaseController<GetByIdReferenceTypeQuery, SearchReferenceTypeQuery , GetListReferenceTypeQuery, CreateReferenceTypeCommand, UpdateReferenceTypeCommand, DeleteReferenceTypeCommand, DeleteListReferenceTypeCommand, ReferenceTypeDto>(sender)
    {

    }
}
